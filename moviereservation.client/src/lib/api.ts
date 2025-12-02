import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig, type InternalAxiosRequestConfig } from 'axios'
import { getAccessToken, setAccessToken, clearAllAuthData } from './auth'

// Extend type to include _retry property
interface CustomAxiosRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean
}

type QueueItem = {
  resolve: (value?: unknown) => void
  reject: (error: unknown) => void
  config: AxiosRequestConfig
}

// Main API instance — có interceptor
const api: AxiosInstance = axios.create({
  baseURL: '/api',
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Axios riêng cho refresh — KHÔNG có interceptor
const refreshApi: AxiosInstance = axios.create({
  baseURL: '/api',
  withCredentials: true,
  headers: {
    'Content-Type': 'application/json'
  }
})

let isRefreshing = false
let failedQueue: QueueItem[] = []

const processQueue = (error: unknown, token: string | null = null) => {
  failedQueue.forEach(({ resolve, reject, config }) => {
    if (error) {
      reject(error)
    } else {
      if (token && config.headers) {
        config.headers['Authorization'] = `Bearer ${token}`
      }
      resolve(api(config))
    }
  })
  failedQueue = []
}

// Attach access token (CHỈ chạy cho api, KHÔNG chạy cho refreshApi)
api.interceptors.request.use((config) => {
  const token = getAccessToken()
  if (token && config.headers) {
    config.headers['Authorization'] = `Bearer ${token}`
  }
  return config
})

// Handle 401 -> try refresh
api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as CustomAxiosRequestConfig

    if (!originalRequest) return Promise.reject(error)

    const isTokenExpired =
      error.response?.status === 401 ||
      (error.response?.status === 400 && (error.response?.data as any)?.message?.toLowerCase?.()?.includes('token'))

    if (isTokenExpired && !originalRequest._retry) {
      originalRequest._retry = true

      // Nếu đang refresh → đưa vào queue
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject, config: originalRequest })
        })
      }

      isRefreshing = true

      try {
        // Refresh bằng axios riêng — không có Authorization
        const refreshResponse = await refreshApi.post('/auth/refresh-token')

        const newToken =
          refreshResponse.data?.data?.accessToken ||
          refreshResponse.data?.data?.AccessToken ||
          refreshResponse.data?.accessToken ||
          refreshResponse.data?.AccessToken ||
          null

        if (!newToken) {
          throw new Error('No access token returned from refresh.')
        }

        // Lưu token mới
        setAccessToken(newToken)

        // Giải queue
        processQueue(null, newToken)
        isRefreshing = false

        // Retry lại request gốc
        if (originalRequest.headers) {
          originalRequest.headers['Authorization'] = `Bearer ${newToken}`
        }
        return api(originalRequest)
      } catch (refreshError) {
        processQueue(refreshError, null)
        isRefreshing = false

        console.error('Refresh token failed:', refreshError)
        clearAllAuthData()
        window.location.href = '/auth/login'

        return Promise.reject(refreshError)
      }
    }

    return Promise.reject(error)
  }
)

export default api
