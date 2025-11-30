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
import { Badge } from "@/components/ui/badge"
import { useAdminUsers } from "@/hooks/use-admin-data"
import { AdminDataState } from "@/components/admin-data-state"
import { useQueryClient } from "@tanstack/react-query"

export default function SettingsUsersPage() {
  const [selectedUsers, setSelectedUsers] = React.useState<string[]>([])
  const [searchQuery, setSearchQuery] = React.useState("")
  const queryClient = useQueryClient()

  const {
    data: users = [],
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminUsers()

  // Filter users based on search query
  const filteredUsers = React.useMemo(() => {
    if (!searchQuery.trim()) return users
    const query = searchQuery.toLowerCase()
    return users.filter(
      (user) =>
        user.userName.toLowerCase().includes(query) ||
        user.email.toLowerCase().includes(query) ||
        user.id.toLowerCase().includes(query)
    )
  }, [users, searchQuery])

  const handleSelectAll = (checked: boolean) => {
    if (checked) {
      setSelectedUsers(filteredUsers.map((u) => u.id))
    } else {
      setSelectedUsers([])
    }
  }

  const handleSelectUser = (userId: string, checked: boolean) => {
    if (checked) {
      setSelectedUsers((prev) => [...prev, userId])
    } else {
      setSelectedUsers((prev) => prev.filter((id) => id !== userId))
    }
  }

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ["admin", "users"] })
    queryClient.invalidateQueries({ queryKey: ["permissions", "users"] })
    refetch()
  }

  return (
    <div className="flex flex-col h-full bg-background">
      {/* Header */}
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <div className="flex items-center gap-2 mb-1">
            <h1 className="text-2xl font-semibold">Người Dùng ({filteredUsers.length})</h1>
            <IconInfoCircle className="h-4 w-4 text-muted-foreground" />
          </div>
          <p className="text-sm text-muted-foreground">
            Quản lý người dùng và các quyền hạn. Click vào user name để xem và quản lý UserClaims (các PermissionConstants được cấp động).
          </p>
        </div>
      </div>

      {/* Actions */}
      <div className="border-b bg-background px-8 py-4">
        <div className="flex items-center justify-between">
          <div className="flex-1 max-w-md">
            <Input
              placeholder="Tìm kiếm user"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-2">
            <Button variant="outline" size="icon" onClick={handleRefresh}>
              <IconRefresh className="h-4 w-4" />
            </Button>
            <Button variant="outline" disabled={selectedUsers.length === 0}>
              <IconTrash className="h-4 w-4 mr-2" />
              Xóa
            </Button>
            <Button>
              <IconPlus className="h-4 w-4 mr-2" />
              Tạo user
            </Button>
          </div>
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 overflow-auto p-8">
        <Card>
          <CardHeader>
            <CardTitle>Danh Sách Người Dùng</CardTitle>
            <CardDescription>
              Click vào user name để xem và quản lý UserClaims (permissions)
            </CardDescription>
          </CardHeader>
          <CardContent className="p-0">
            <AdminDataState
              isLoading={isLoading}
              isError={isError}
              error={error}
              onRetry={refetch}
              dataLength={filteredUsers.length}
              emptyMessage="Không tìm thấy user nào."
            >
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-12">
                      <Checkbox
                        checked={
                          filteredUsers.length > 0 &&
                          selectedUsers.length === filteredUsers.length
                        }
                        onCheckedChange={handleSelectAll}
                      />
                    </TableHead>
                    <TableHead>Tên Người Dùng</TableHead>
                    <TableHead>Email</TableHead>
                    <TableHead>Roles</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredUsers.map((user) => (
                    <TableRow key={user.id}>
                      <TableCell>
                        <Checkbox
                          checked={selectedUsers.includes(user.id)}
                          onCheckedChange={(checked) =>
                            handleSelectUser(user.id, checked as boolean)
                          }
                        />
                      </TableCell>
                      <TableCell>
                        <Link
                          to={`/admin/settings/users/${user.id}/permissions`}
                          className="font-medium hover:underline transition-all"
                        >
                          {user.userName}
                        </Link>
                      </TableCell>
                      <TableCell>
                        <span className="text-sm text-muted-foreground">
                          {user.email}
                        </span>
                      </TableCell>
                      <TableCell>
                        {user.roles.length > 0 ? (
                          <div className="flex flex-wrap gap-1">
                            {user.roles.slice(0, 2).map((role) => (
                              <Badge key={role} variant="secondary" className="text-xs">
                                {role}
                              </Badge>
                            ))}
                            {user.roles.length > 2 && (
                              <Badge variant="secondary" className="text-xs">
                                +{user.roles.length - 2}
                              </Badge>
                            )}
                          </div>
                        ) : (
                          <span className="text-sm text-muted-foreground">-</span>
                        )}
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </AdminDataState>
          </CardContent>
        </Card>

        {/* Pagination */}
        {filteredUsers.length > 0 && (
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
