import axios, { AxiosError, type AxiosInstance, type AxiosRequestConfig, type InternalAxiosRequestConfig } from 'axios'
import { getAccessToken, setAccessToken, clearAccessToken } from './auth'

// Extend type to include _retry property
interface CustomAxiosRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean
}

type QueueItem = {
  resolve: (value?: unknown) => void
  reject: (error: unknown) => void
  config: AxiosRequestConfig
}

const api: AxiosInstance = axios.create({
  baseURL: '/api',
  withCredentials: true, // send HttpOnly refresh cookie to server
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

// Attach access token
api.interceptors.request.use((config) => {
  // Do NOT attach Authorization header when calling the refresh endpoint.
  const url = (config.url || '').toString().toLowerCase()
  if (url.includes('/auth/refresh-token')) {
    return config
  }

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

    if (!originalRequest) {
      return Promise.reject(error)
    }

    // Prevent infinite loop and handle only 401
    if ((error.response?.status === 401 || (error.response?.status === 400 && (error.response?.data as any)?.message?.toLowerCase?.()?.includes('token'))) && !originalRequest._retry) {
      originalRequest._retry = true

      if (isRefreshing) {
        // queue the request until refresh completes
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject, config: originalRequest })
        })
      }

      isRefreshing = true

      try {
        // call refresh endpoint; withCredentials is true so server can read the HttpOnly cookie
        const refreshResponse = await api.post('/auth/refresh-token', {}, { withCredentials: true })

        // Backend returns shape: { message, succeeded, data: { accessToken, expiresIn } } OR data.accessToken
        const newToken =
          (refreshResponse.data && (refreshResponse.data.data?.accessToken || refreshResponse.data.data?.AccessToken)) ||
          (refreshResponse.data?.accessToken || refreshResponse.data?.AccessToken) ||
          null

        if (!newToken) {
          throw new Error('No access token returned from refresh.')
        }

        setAccessToken(newToken)

        processQueue(null, newToken)
        isRefreshing = false

        // update original request header and retry
        if (originalRequest.headers) {
          originalRequest.headers['Authorization'] = `Bearer ${newToken}`
        }
        return api(originalRequest)
      } catch (refreshError) {
        processQueue(refreshError, null)
        isRefreshing = false
        console.error('Refresh token failed:', refreshError)
        // clear tokens and redirect to login
        clearAccessToken()
        try {
          // optional: navigate to login page
          window.location.href = '/auth/login'
        } catch {}
        return Promise.reject(refreshError)
      }
    }

    return Promise.reject(error)
  }
)

export default api