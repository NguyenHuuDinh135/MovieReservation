import api from "@/lib/api"

export type PermissionKey = string

export interface PermissionDto {
permission: string
}

// Admin permissions list - permissions that grant access to admin area
export const ADMIN_PERMISSIONS = [
  "Permission.ManagePermissions",
  "Permission.Movies.Create",
  "Permission.Movies.Edit",
  "Permission.Movies.Delete",
  "Permission.Shows.Create",
  "Permission.Shows.Edit",
  "Permission.Shows.Delete",
  "Permission.Theaters.Create",
  "Permission.Theaters.Edit",
  "Permission.Theaters.Delete",
  "Permission.Bookings.Create",
  "Permission.Bookings.Edit",
  "Permission.Bookings.Delete",
  "Permission.Genres.Create",
  "Permission.Genres.Edit",
  "Permission.Genres.Delete",
  "Permission.Users.Create",
  "Permission.Users.Edit",
  "Permission.Users.Delete",
  "Permission.Payments.Create",
  "Permission.Payments.Edit",
  "Permission.Payments.Delete",
]

export interface RoleDto {
  id: string
  name: string
  normalizedName: string
  permissionsCount: number
}

export const permissionsApi = {
getMyPermissions: () => api.get<PermissionKey[]>("/permissions/me"),

// permissions
getAllPermissions: () => api.get<PermissionKey[]>("/permissions/all"),
getAssignablePermissions: () => api.get<PermissionKey[]>("/permissions/assignable"),

// roles
getAllRoles: () => api.get<RoleDto[]>("/permissions/roles/all"),
getRolePermissions: (roleId: string) =>
   api.get<PermissionKey[]>(`/permissions/roles/${roleId}`),
addPermissionToRole: (roleId: string, permission: PermissionKey) =>
   api.post<void>(`/permissions/roles/${roleId}`, { permission } satisfies PermissionDto),
removePermissionFromRole: (roleId: string, permission: PermissionKey) =>
   api.delete<void>(`/permissions/roles/${roleId}`, {
      data: { permission } satisfies PermissionDto,
   }),

// users
getUserPermissions: (userId: string) =>
   api.get<PermissionKey[]>(`/permissions/users/${userId}`),
addPermissionToUser: (userId: string, permission: PermissionKey) =>
   api.post<void>(`/permissions/users/${userId}`, { permission } satisfies PermissionDto),
removePermissionFromUser: (userId: string, permission: PermissionKey) =>
   api.delete<void>(`/permissions/users/${userId}`, {
      data: { permission } satisfies PermissionDto,
   }),
}

/**
 * Check if user has any admin permission
 */
export function hasAdminPermission(permissions: PermissionKey[]): boolean {
  return permissions.some((p) => ADMIN_PERMISSIONS.includes(p))
}

/**
 * Check if user has a specific permission
 */
export function hasPermission(permissions: PermissionKey[], permission: PermissionKey): boolean {
  return permissions.includes(permission)
}

/**
 * Determine redirect path after login based on permissions
 * Calls API to get permissions and checks if user is admin
 * Note: Token should already be saved to localStorage before calling this function
 */
export async function getRedirectPathAfterLogin(defaultPath: string): Promise<string> {
  try {
    const response = await permissionsApi.getMyPermissions()
    const permissions = response.data || []
    
    if (hasAdminPermission(permissions)) {
      return "/admin"
    }
    
    return defaultPath
  } catch (error) {
    console.error("Failed to fetch permissions for redirect:", error)
    // If error, fallback to default path
    return defaultPath
  }
}


