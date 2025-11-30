import * as React from "react"
import { Link } from "react-router-dom"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { IconPlus, IconRefresh, IconTrash, IconInfoCircle } from "@tabler/icons-react"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { Checkbox } from "@/components/ui/checkbox"
import { AdminDataState } from "@/components/admin-data-state"
import { useQueryClient } from "@tanstack/react-query"
import { useAllRoles } from "@/hooks/use-permissions-data"
import { useAdminUsers } from "@/hooks/use-admin-data"

export default function UserGroupsPage() {
  const [selectedGroups, setSelectedGroups] = React.useState<string[]>([])
  const [searchQuery, setSearchQuery] = React.useState("")
  const queryClient = useQueryClient()

  // Lấy roles từ API - User groups sẽ là AspRole (AspNetRoles)
  const {
    data: roles = [],
    isLoading: isLoadingRoles,
    isError: isErrorRoles,
    error: errorRoles,
    refetch: refetchRoles,
  } = useAllRoles()

  // Lấy users để đếm số users trong mỗi group (role)
  const { data: users = [] } = useAdminUsers()

  // Tạo user groups từ roles - đếm số users trong mỗi role
  const userGroups = React.useMemo(() => {
    return roles.map((role) => {
      const usersInRole = users.filter((user) => user.roles.includes(role.name)).length
      return {
        id: role.id,
        name: role.name,
        users: usersInRole,
        permissionsCount: role.permissionsCount,
      }
    })
  }, [roles, users])

  // Filter groups based on search query
  const filteredGroups = React.useMemo(() => {
    if (!searchQuery.trim()) return userGroups
    const query = searchQuery.toLowerCase()
    return userGroups.filter((group) => group.name.toLowerCase().includes(query))
  }, [userGroups, searchQuery])

  const handleSelectAll = (checked: boolean) => {
    if (checked) {
      setSelectedGroups(filteredGroups.map((g) => g.id))
    } else {
      setSelectedGroups([])
    }
  }

  const handleSelectGroup = (groupId: string, checked: boolean) => {
    if (checked) {
      setSelectedGroups((prev) => [...prev, groupId])
    } else {
      setSelectedGroups((prev) => prev.filter((id) => id !== groupId))
    }
  }

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ["permissions", "roles"] })
    queryClient.invalidateQueries({ queryKey: ["admin", "users"] })
    refetchRoles()
  }

  return (
    <div className="flex flex-col h-full bg-background">
      {/* Header */}
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <div className="flex items-center gap-2 mb-1">
            <h1 className="text-2xl font-semibold">Nhóm Người Dùng ({filteredGroups.length})</h1>
            <IconInfoCircle className="h-4 w-4 text-muted-foreground" />
          </div>
          <p className="text-sm text-muted-foreground">
            A user group is a collection of IAM users. User groups = AspNetRoles. Click vào role name để xem và quản lý RoleClaims (permissions).
          </p>
        </div>
      </div>

      {/* Actions */}
      <div className="border-b bg-background px-8 py-4">
        <div className="flex items-center justify-between">
          <div className="flex-1 max-w-md">
            <Input
              placeholder="Tìm kiếm group"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-2">
            <Button variant="outline" size="icon" onClick={handleRefresh}>
              <IconRefresh className="h-4 w-4" />
            </Button>
            <Button variant="outline" disabled={selectedGroups.length === 0}>
              <IconTrash className="h-4 w-4 mr-2" />
              Xóa
            </Button>
            <Button>
              <IconPlus className="h-4 w-4 mr-2" />
              Tạo group
            </Button>
          </div>
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 overflow-auto p-8">
        <Card>
          <CardHeader>
            <CardTitle>Danh Sách Nhóm Người Dùng</CardTitle>
            <CardDescription>
              Danh sách các AspNetRoles. Click vào role name để xem và quản lý RoleClaims (permissions).
            </CardDescription>
          </CardHeader>
          <CardContent className="p-0">
            <AdminDataState
              isLoading={isLoadingRoles}
              isError={isErrorRoles}
              error={errorRoles}
              onRetry={refetchRoles}
              dataLength={filteredGroups.length}
              emptyMessage="Không tìm thấy group nào."
            >
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-12">
                      <Checkbox
                        checked={
                          filteredGroups.length > 0 &&
                          selectedGroups.length === filteredGroups.length
                        }
                        onCheckedChange={handleSelectAll}
                      />
                    </TableHead>
                    <TableHead>Tên Nhóm</TableHead>
                    <TableHead>Số Users</TableHead>
                    <TableHead>Số Permissions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredGroups.map((group) => (
                    <TableRow key={group.id}>
                      <TableCell>
                        <Checkbox
                          checked={selectedGroups.includes(group.id)}
                          onCheckedChange={(checked) =>
                            handleSelectGroup(group.id, checked as boolean)
                          }
                        />
                      </TableCell>
                      <TableCell>
                        <Link
                          to={`/admin/settings/user-groups/${group.id}/permissions`}
                          className="font-medium hover:underline transition-all"
                        >
                          {group.name}
                        </Link>
                      </TableCell>
                      <TableCell>{group.users}</TableCell>
                      <TableCell>{group.permissionsCount}</TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </AdminDataState>
          </CardContent>
        </Card>

        {/* Pagination */}
        {filteredGroups.length > 0 && (
          <div className="flex items-center justify-end gap-2 mt-4">
            <Button variant="outline" size="icon" disabled>
              &lt;
            </Button>
            <span className="text-sm text-muted-foreground">1</span>
            <Button variant="outline" size="icon" disabled>
              &gt;
            </Button>
          </div>
        )}
      </div>
    </div>
  )
}
