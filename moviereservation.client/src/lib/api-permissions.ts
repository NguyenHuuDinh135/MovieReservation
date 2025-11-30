import api from "@/lib/api"

export type PermissionKey = string

export interface PermissionDto {
permission: string
}

export const permissionsApi = {
getMyPermissions: () => api.get<PermissionKey[]>("/permissions/me"),

// roles
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


