import * as React from "react"
import { useParams, Link } from "react-router-dom"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { IconRefresh, IconCheck, IconX, IconInfoCircle } from "@tabler/icons-react"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { AdminDataState } from "@/components/admin-data-state"
import { useQueryClient } from "@tanstack/react-query"
import { permissionsApi, type PermissionKey } from "@/lib/api-permissions"
import { useQuery, useMutation } from "@tanstack/react-query"
import { useAllPermissions, useAllRoles } from "@/hooks/use-permissions-data"
import { toast } from "sonner"
import {
  Breadcrumb,
  BreadcrumbList,
  BreadcrumbItem,
  BreadcrumbLink,
  BreadcrumbPage,
  BreadcrumbSeparator,
} from "@/components/ui/breadcrumb"

export default function UserGroupPermissionsPage() {
  const { roleId } = useParams<{ roleId: string }>()
  const [permissionSearch, setPermissionSearch] = React.useState("")
  const queryClient = useQueryClient()

  // Lấy thông tin role
  const { data: roles = [] } = useAllRoles()
  const currentRole = roles.find((r) => r.id === roleId)

  // Lấy tất cả permissions từ PermissionConstants để hiển thị
  const {
    data: allPermissions = [],
    isLoading: isLoadingPermissions,
  } = useAllPermissions()

  // Lấy permissions của role từ AspNetRoleClaims
  const {
    data: rolePermissions = [],
    isLoading: isLoadingRolePermissions,
    refetch: refetchRolePermissions,
  } = useQuery({
    queryKey: ["permissions", "roles", roleId],
    queryFn: () => {
      if (!roleId) return Promise.resolve([])
      return permissionsApi.getRolePermissions(roleId).then(res => res.data || [])
    },
    enabled: !!roleId,
  })

  // Mutation để thêm permission cho role
  const addPermissionMutation = useMutation({
    mutationFn: (permission: PermissionKey) =>
      permissionsApi.addPermissionToRole(roleId!, permission),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["permissions", "roles"] })
      refetchRolePermissions()
      toast.success("Đã thêm quyền thành công")
    },
    onError: (error: any) => {
      toast.error(error?.response?.data?.message || "Không thể thêm quyền")
    },
  })

  // Mutation để xóa permission khỏi role
  const removePermissionMutation = useMutation({
    mutationFn: (permission: PermissionKey) =>
      permissionsApi.removePermissionFromRole(roleId!, permission),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["permissions", "roles"] })
      queryClient.invalidateQueries({ queryKey: ["permissions", "users"] })
      queryClient.invalidateQueries({ queryKey: ["admin", "users"] })
      refetchRolePermissions()
      toast.success("Đã xóa quyền thành công. Các UserClaims tương ứng cũng đã được xóa.")
    },
    onError: (error: any) => {
      toast.error(error?.response?.data?.message || "Không thể xóa quyền")
    },
  })

  // Filter permissions based on search query
  const filteredPermissions = React.useMemo(() => {
    if (!permissionSearch.trim()) return allPermissions
    const query = permissionSearch.toLowerCase()
    return allPermissions.filter((permission) =>
      permission.toLowerCase().includes(query)
    )
  }, [allPermissions, permissionSearch])

  const handleTogglePermission = (permission: PermissionKey) => {
    if (!roleId) return

    const hasPermission = rolePermissions.includes(permission)
    if (hasPermission) {
      removePermissionMutation.mutate(permission)
    } else {
      addPermissionMutation.mutate(permission)
    }
  }

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ["permissions", "roles"] })
    refetchRolePermissions()
  }

  if (!roleId || !currentRole) {
    return (
      <div className="flex flex-col h-full bg-background">
        <div className="flex-1 flex items-center justify-center">
          <div className="text-center">
            <p className="text-lg font-semibold mb-2">Không tìm thấy role</p>
            <Link to="/admin/settings/user-groups" className="text-blue-600 hover:underline">
              Quay lại danh sách user groups
            </Link>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div className="flex flex-col h-full bg-background">
      {/* Breadcrumb */}
      <div className="border-b bg-background px-8 py-4">
        <Breadcrumb>
          <BreadcrumbList>
            <BreadcrumbItem>
              <BreadcrumbLink asChild>
                <Link to="/admin/settings">Cài Đặt</Link>
              </BreadcrumbLink>
            </BreadcrumbItem>
            <BreadcrumbSeparator />
            <BreadcrumbItem>
              <BreadcrumbLink asChild>
                <Link to="/admin/settings/user-groups">Nhóm Người Dùng</Link>
              </BreadcrumbLink>
            </BreadcrumbItem>
            <BreadcrumbSeparator />
            <BreadcrumbItem>
              <BreadcrumbPage>{currentRole.name}</BreadcrumbPage>
            </BreadcrumbItem>
            <BreadcrumbSeparator />
            <BreadcrumbItem>
              <BreadcrumbPage>Permissions</BreadcrumbPage>
            </BreadcrumbItem>
          </BreadcrumbList>
        </Breadcrumb>
      </div>

      {/* Header */}
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <div className="flex items-center gap-2 mb-1">
            <h1 className="text-2xl font-semibold">RoleClaims: {currentRole.name}</h1>
            <IconInfoCircle className="h-4 w-4 text-muted-foreground" />
          </div>
          <p className="text-sm text-muted-foreground">
            Các PermissionConstants được cấp cho role {currentRole.name} (từ AspNetRoleClaims). Khi xóa permission khỏi role, các UserClaims tương ứng của users thuộc role này cũng sẽ bị xóa.
          </p>
        </div>
      </div>

      {/* Actions */}
      <div className="border-b bg-background px-8 py-4">
        <div className="flex items-center justify-between">
          <div className="flex-1 max-w-md">
            <Input
              placeholder="Tìm kiếm permission"
              value={permissionSearch}
              onChange={(e) => setPermissionSearch(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-2">
            <Button variant="outline" size="icon" onClick={handleRefresh}>
              <IconRefresh className="h-4 w-4" />
            </Button>
          </div>
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 overflow-auto p-8">
        <Card>
          <CardHeader>
            <CardTitle>Danh Sách Permissions</CardTitle>
            <CardDescription>
              Tất cả các permissions từ PermissionConstants. Đánh dấu để thêm/xóa quyền cho role này.
            </CardDescription>
          </CardHeader>
          <CardContent className="p-0">
            <AdminDataState
              isLoading={isLoadingPermissions || isLoadingRolePermissions}
              isError={false}
              dataLength={filteredPermissions.length}
              emptyMessage="Không tìm thấy permission nào."
            >
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-12">Trạng thái</TableHead>
                    <TableHead>Permission Name</TableHead>
                    <TableHead className="w-24">Thao tác</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredPermissions.map((permission) => {
                    const hasPermission = rolePermissions.includes(permission)
                    const isMutating =
                      addPermissionMutation.isPending ||
                      removePermissionMutation.isPending

                    return (
                      <TableRow key={permission}>
                        <TableCell>
                          {hasPermission ? (
                            <IconCheck className="h-4 w-4 text-green-600" />
                          ) : (
                            <IconX className="h-4 w-4 text-gray-400" />
                          )}
                        </TableCell>
                        <TableCell>
                          <span className="font-mono text-sm">{permission}</span>
                        </TableCell>
                        <TableCell>
                          <Button
                            size="sm"
                            variant={hasPermission ? "destructive" : "default"}
                            disabled={isMutating}
                            onClick={() => handleTogglePermission(permission)}
                          >
                            {hasPermission ? "Xóa" : "Thêm"}
                          </Button>
                        </TableCell>
                      </TableRow>
                    )
                  })}
                </TableBody>
              </Table>
            </AdminDataState>
          </CardContent>
        </Card>
      </div>
    </div>
  )
}
