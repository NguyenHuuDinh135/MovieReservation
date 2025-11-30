// small helper to manage access token (stored in localStorage)
// you may replace with a more secure in-memory store if needed
export const TOKEN_KEY = 'mr_access_token'
const OLD_TOKEN_KEY = 'token' // Key cũ được dùng trước đây

export function getAccessToken(): string | null {
  // Thử lấy token từ key mới trước
  let token = localStorage.getItem(TOKEN_KEY)
  
  // Nếu không có token mới, thử migrate từ key cũ
  if (!token) {
    const oldToken = localStorage.getItem(OLD_TOKEN_KEY)
    if (oldToken) {
      // Migrate token cũ sang key mới
      setAccessToken(oldToken)
      // Xóa token cũ để tránh trùng lặp
      localStorage.removeItem(OLD_TOKEN_KEY)
      token = oldToken
    }
  }
  
  return token
}

export function setAccessToken(token: string) {
  localStorage.setItem(TOKEN_KEY, token)
}

export function clearAccessToken() {
  localStorage.removeItem(TOKEN_KEY)
}

/**
 * Xóa toàn bộ dữ liệu xác thực khỏi localStorage
 * Bao gồm: access token (mới và cũ), user data
 */
export function clearAllAuthData() {
  // Xóa token mới
  clearAccessToken()
  // Xóa token cũ nếu có
  localStorage.removeItem(OLD_TOKEN_KEY)
  // Xóa user data
  localStorage.removeItem("user")
}