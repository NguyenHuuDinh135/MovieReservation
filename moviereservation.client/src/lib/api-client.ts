import api from './api'
import { setAccessToken } from './auth'

/**
 * Client API for public / user-facing endpoints (Auth, Shows, etc.)
 * Keep DTOs in sync with backend (MovieReservation.Server.Web.Controllers).
 */

// --- DTOs ---
export type TokenResponseDto = {
  AccessToken?: string
  accessToken?: string
  Username?: string
  username?: string
  Email?: string
  email?: string
  ExpiresIn?: number
  expiresIn?: number
}

const normalizeAccessToken = (data?: any): string | null =>
  data?.accessToken ?? data?.AccessToken ?? null

// --- Auth endpoints ---
export interface LoginCommand {
  email: string
  password: string
  callbackUrl?: string
}
export interface LoginResponse {
  message: string
  email?: string
}

export interface RegisterCommand {
  userName: string
  email: string
  password: string
}
export interface RegisterResponse {
  message: string
  email?: string
}

export interface VerifyOtpCommand {
  email: string
  otpCode: string
}
export interface VerifyOtpResponse {
  message: string
  succeeded: boolean
  data?: TokenResponseDto
  error?: string
}

export const authApi = {
  login: (payload: LoginCommand) => api.post<LoginResponse>('/auth/login', payload),
  register: (payload: RegisterCommand) => api.post<RegisterResponse>('/auth/register', payload),

  // verify-otp returns TokenResponseDto in data when successful
  verifyOtp: async (payload: VerifyOtpCommand) => {
    const res = await api.post<VerifyOtpResponse>('/auth/verify-otp', payload)
    const token = normalizeAccessToken(res.data?.data)
    if (token) setAccessToken(token)
    return res
  },

  // Backend currently accepts refresh token in body OR reads HttpOnly cookie.
  // api has withCredentials enabled so cookie flows will work.
  refreshToken: async (payload?: { refreshToken?: string }) => {
    const res = await api.post('/auth/refresh-token', payload ?? {})
    const token = normalizeAccessToken(res.data?.data)
    if (token) setAccessToken(token)
    return res
  },

  logout: (payload?: object) => api.post('/auth/logout', payload ?? {}),
}

// --- Shows endpoints (public) ---
export type ShowDto = {
  id: number
  title: string
  description?: string
  // extend with backend fields as needed
}

export const showsApi = {
  getAll: () => api.get<ShowDto[]>('/shows/all'),
  getById: (id: number) => api.get<ShowDto>(`/shows/id/${id}`),
  // date can be ISO string or Date (axios will serialize)
  getFiltered: (date: string | Date) => api.get('/shows/filters', { params: { date } }),
}

// Grouped default export
const apiClient = {
  auth: authApi,
  shows: showsApi,
}

export default apiClient