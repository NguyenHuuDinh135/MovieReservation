import { useQuery } from "@tanstack/react-query"
import { permissionsApi, type RoleDto, type PermissionKey } from "@/lib/api-permissions"

const extractData = async <T>(promise: Promise<{ data: T }>) => {
  const res = await promise
  return res.data
}

const fetchRoles = () => extractData<RoleDto[]>(permissionsApi.getAllRoles())
const fetchAllPermissions = () => extractData<PermissionKey[]>(permissionsApi.getAllPermissions())

export const useAllRoles = () =>
  useQuery({
    queryKey: ["permissions", "roles"],
    queryFn: fetchRoles,
  })

export const useAllPermissions = () =>
  useQuery({
    queryKey: ["permissions", "all"],
    queryFn: fetchAllPermissions,
  })
