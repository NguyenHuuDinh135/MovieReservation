import api from './api'

/**
 * Admin API surface (protected endpoints).
 * Put admin-only controller wrappers here.
 * Requires Authorization header (api instance attaches access token from auth helper).
 */

// --- Shows (admin) ---
export interface CreateShowCommand {
  title: string
  description?: string
  theaterId?: number
  startTime?: string
  endTime?: string
  // extend with server fields
}

export interface UpdateShowCommand {
  id: number
  title?: string
  description?: string
  // other optional fields
}

export const adminShowsApi = {
  create: (payload: CreateShowCommand) => api.post<number>('/shows/create', payload),
  update: (payload: UpdateShowCommand) => api.put('/shows/update', payload),
  delete: (id: number) => api.delete(`/shows/id/${id}`),
}

// --- Placeholder: add other admin controllers here ---
// e.g. usersApi, theatersApi, bookingsAdminApi, paymentsAdminApi
// implement once corresponding server controllers exist.

const apiAdmin = {
  shows: adminShowsApi,
}

export default apiAdmin