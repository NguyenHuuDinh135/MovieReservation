import * as React from "react"
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
import { useAllPermissions } from "@/hooks/use-permissions-data"

export default function SettingsRolesPage() {
  const [selectedPermissions, setSelectedPermissions] = React.useState<string[]>([])
  const [searchQuery, setSearchQuery] = React.useState("")
  const queryClient = useQueryClient()

  // Lấy tất cả permissions từ PermissionConstants
  const {
    data: permissions = [],
    isLoading,
    isError,
    error,
    refetch,
  } = useAllPermissions()

  // Filter permissions based on search query
  const filteredPermissions = React.useMemo(() => {
    if (!searchQuery.trim()) return permissions
    const query = searchQuery.toLowerCase()
    return permissions.filter((permission) =>
      permission.toLowerCase().includes(query)
    )
  }, [permissions, searchQuery])

  const handleSelectAll = (checked: boolean) => {
    if (checked) {
      setSelectedPermissions(filteredPermissions)
    } else {
      setSelectedPermissions([])
    }
  }

  const handleSelectPermission = (permission: string, checked: boolean) => {
    if (checked) {
      setSelectedPermissions((prev) => [...prev, permission])
    } else {
      setSelectedPermissions((prev) => prev.filter((p) => p !== permission))
    }
  }

  const handleRefresh = () => {
    queryClient.invalidateQueries({ queryKey: ["permissions", "all"] })
    refetch()
  }

  return (
    <div className="flex flex-col h-full bg-background">
      {/* Header */}
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <div className="flex items-center gap-2 mb-1">
            <h1 className="text-2xl font-semibold">Quyền Hạn ({filteredPermissions.length})</h1>
            <IconInfoCircle className="h-4 w-4 text-muted-foreground" />
          </div>
          <p className="text-sm text-muted-foreground">
            Danh sách tất cả các quyền hạn (permissions) từ PermissionConstants. Các permissions này có thể được gán cho roles hoặc users.
          </p>
        </div>
      </div>

      {/* Actions */}
      <div className="border-b bg-background px-8 py-4">
        <div className="flex items-center justify-between">
          <div className="flex-1 max-w-md">
            <Input
              placeholder="Tìm kiếm permission"
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>
          <div className="flex items-center gap-2">
            <Button variant="outline" size="icon" onClick={handleRefresh}>
              <IconRefresh className="h-4 w-4" />
            </Button>
            <Button variant="outline" disabled={selectedPermissions.length === 0}>
              <IconTrash className="h-4 w-4 mr-2" />
              Xóa
            </Button>
            <Button>
              <IconPlus className="h-4 w-4 mr-2" />
              Tạo permission
            </Button>
          </div>
        </div>
      </div>

      {/* Content */}
      <div className="flex-1 overflow-auto p-8">
        <Card>
          <CardHeader>
            <CardTitle>Danh Sách Quyền Hạn</CardTitle>
            <CardDescription>
              Tất cả các quyền hạn (permissions) được định nghĩa trong PermissionConstants
            </CardDescription>
          </CardHeader>
          <CardContent className="p-0">
            <AdminDataState
              isLoading={isLoading}
              isError={isError}
              error={error}
              onRetry={refetch}
              dataLength={filteredPermissions.length}
              emptyMessage="Không tìm thấy permission nào."
            >
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-12">
                      <Checkbox
                        checked={
                          filteredPermissions.length > 0 &&
                          selectedPermissions.length === filteredPermissions.length
                        }
                        onCheckedChange={handleSelectAll}
                      />
                    </TableHead>
                    <TableHead>
                      Permission Name
                      <span className="ml-1 text-xs">↑</span>
                    </TableHead>
                    <TableHead>
                      Module
                      <span className="ml-1 text-xs">↓</span>
                    </TableHead>
                    <TableHead>
                      Action
                      <span className="ml-1 text-xs">↓</span>
                    </TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredPermissions.map((permission) => {
                    // Parse permission name để lấy module và action
                    const parts = permission.split('.')
                    const module = parts.length > 1 ? parts[1] : '-'
                    const action = parts.length > 2 ? parts.slice(2).join('.') : '-'

                    return (
                      <TableRow key={permission}>
                        <TableCell>
                          <Checkbox
                            checked={selectedPermissions.includes(permission)}
                            onCheckedChange={(checked) =>
                              handleSelectPermission(permission, checked as boolean)
                            }
                          />
                        </TableCell>
                        <TableCell>
                          <span className="font-mono text-sm">{permission}</span>
                        </TableCell>
                        <TableCell>{module}</TableCell>
                        <TableCell>{action}</TableCell>
                      </TableRow>
                    )
                  })}
                </TableBody>
              </Table>
            </AdminDataState>
          </CardContent>
        </Card>

        {/* Pagination */}
        {filteredPermissions.length > 0 && (
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
