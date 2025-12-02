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
import { useAssignablePermissions } from "@/hooks/use-permissions-data"
import { useAdminUsers } from "@/hooks/use-admin-data"
import { toast } from "sonner"

export default function UserPermissionsPage() {
  const { userId } = useParams<{ userId: string }>()
  const [permissionSearch, setPermissionSearch] = React.useState("")
  const queryClient = useQueryClient()

  // Lấy thông tin user
  const { data: users = [] } = useAdminUsers()
  const currentUser = users.find((u) => u.id === userId)

  // Lấy các permissions mà admin có thể cấp (từ roles của admin)
  const {
    data: assignablePermissions = [],
    isLoading: isLoadingPermissions,
  } = useAssignablePermissions()

  // Lấy UserClaims (permissions) của user từ AspNetUserClaims
  const {
    data: userPermissions = [],
    isLoading: isLoadingUserPermissions,
    refetch: refetchUserPermissions,
  } = useQuery({
    queryKey: ["permissions", "users", userId],
    queryFn: () => {
      if (!userId) return Promise.resolve([])
      return permissionsApi.getUserPermissions(userId).then(res => res.data || [])
    },
    enabled: !!userId,
  })

  // Mutation để thêm permission cho user
  const addPermissionMutation = useMutation({
    mutationFn: (permission: PermissionKey) =>
      permissionsApi.addPermissionToUser(userId!, permission),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["permissions", "users"] })
      refetchUserPermissions()
      toast.success("Đã thêm quyền thành công")
    },
    onError: (error: any) => {
      toast.error(error?.response?.data?.message || "Không thể thêm quyền")
    },
  })

  // Mutation để xóa permission khỏi user
  const removePermissionMutation = useMutation({
    mutationFn: (permission: PermissionKey) =>
      permissionsApi.removePermissionFromUser(userId!, permission),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["permissions", "users"] })
      refetchUserPermissions()
      toast.success("Đã xóa quyền thành công")
    },
    onError: (error: any) => {
      toast.error(error?.response?.data?.message || "Không thể xóa quyền")
    },
  })

  // Filter permissions based on search query
  const filteredPermissions = React.useMemo(() => {
    if (!permissionSearch.trim()) return assignablePermissions
    const query = permissionSearch.toLowerCase()
    return assignablePermissions.filter((permission) =>
      permission.toLowerCase().includes(query)
    )
  }, [assignablePermissions, permissionSearch])

  const handleTogglePermission = (permission: PermissionKey) => {
    if (!userId) return

    const hasPermission = userPermissions.includes(permission)
    if (hasPermission) {
      removePermissionMutation.mutate(permission)
    } else {
      addPermissionMutation.mutate(permission)
    }
  }

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ["permissions", "users"] })
    queryClient.invalidateQueries({ queryKey: ["permissions", "assignable"] })
    queryClient.invalidateQueries({ queryKey: ["admin", "users"] })
    refetchUserPermissions()
  }

  if (!userId || !currentUser) {
    return (
      <div className="flex flex-col h-full bg-background">
        <div className="flex-1 flex items-center justify-center">
          <div className="text-center">
            <p className="text-lg font-semibold mb-2">Không tìm thấy user</p>
            <Link to="/admin/settings/users" className="text-blue-600 hover:underline">
              Quay lại danh sách users
            </Link>
          </div>
        </div>
      </div>
    )
  }

  return (
    <div className="flex flex-col h-full bg-background">
      {/* Header */}
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <div className="flex items-center gap-2 mb-1">
            <h1 className="text-2xl font-semibold">UserClaims: {currentUser.userName}</h1>
            <IconInfoCircle className="h-4 w-4 text-muted-foreground" />
          </div>
          <p className="text-sm text-muted-foreground">
            Các PermissionConstants được cấp động cho user {currentUser.userName} (từ AspNetUserClaims). Chỉ hiển thị các quyền mà bạn có thể cấp (từ roles của bạn).
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
            <CardTitle>Danh Sách Permissions Có Thể Cấp</CardTitle>
            <CardDescription>
              Chỉ hiển thị các permissions mà bạn có thể cấp (từ roles của bạn). Đánh dấu để thêm/xóa quyền cho user này.
            </CardDescription>
          </CardHeader>
          <CardContent className="p-0">
            <AdminDataState
              isLoading={isLoadingPermissions || isLoadingUserPermissions}
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
                    const hasPermission = userPermissions.includes(permission)
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
