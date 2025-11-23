// small helper to manage access token (stored in localStorage)
// you may replace with a more secure in-memory store if needed
export const TOKEN_KEY = 'mr_access_token'

export function getAccessToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setAccessToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token)
}

export function clearAccessToken() {
  localStorage.removeItem(TOKEN_KEY)
}