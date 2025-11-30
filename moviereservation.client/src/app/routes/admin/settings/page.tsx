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
import { Label } from "@/components/ui/label"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { permissionsApi, type PermissionKey } from "@/lib/api-permissions"

const ALL_PERMISSIONS: PermissionKey[] = [
  "Permission.ManagePermissions",
  "Permission.Movies.View",
  "Permission.Movies.Create",
  "Permission.Movies.Edit",
  "Permission.Movies.Delete",
  "Permission.Shows.View",
  "Permission.Shows.Create",
  "Permission.Shows.Edit",
  "Permission.Shows.Delete",
  "Permission.Theaters.View",
  "Permission.Theaters.Create",
  "Permission.Theaters.Edit",
  "Permission.Theaters.Delete",
  "Permission.Bookings.View",
  "Permission.Bookings.Create",
  "Permission.Bookings.Edit",
  "Permission.Bookings.Delete",
  "Permission.Genres.View",
  "Permission.Genres.Create",
  "Permission.Genres.Edit",
  "Permission.Genres.Delete",
  "Permission.Users.View",
  "Permission.Users.Create",
  "Permission.Users.Edit",
  "Permission.Users.Delete",
  "Permission.Payments.View",
  "Permission.Payments.Create",
  "Permission.Payments.Edit",
  "Permission.Payments.Delete",
]

type PermissionTarget = {
  id: string
  type: "role" | "user"
}

export default function AdminSettingsPage() {
  const [currentTarget, setCurrentTarget] = React.useState<PermissionTarget>({
    id: "",
    type: "role",
  })
  const [loadedPermissions, setLoadedPermissions] = React.useState<PermissionKey[]>([])
  const [isLoading, setIsLoading] = React.useState(false)
  const [error, setError] = React.useState<string | null>(null)

  const loadPermissions = async () => {
    if (!currentTarget.id.trim()) return
    setIsLoading(true)
    setError(null)
    try {
      const res =
        currentTarget.type === "role"
          ? await permissionsApi.getRolePermissions(currentTarget.id)
          : await permissionsApi.getUserPermissions(currentTarget.id)
      setLoadedPermissions(res.data ?? [])
    } catch (e: any) {
      setError(e?.response?.data?.message || e?.message || "Không thể tải quyền.")
    } finally {
      setIsLoading(false)
    }
  }

  const togglePermission = async (permission: PermissionKey) => {
    if (!currentTarget.id.trim()) return
    const has = loadedPermissions.includes(permission)
    setIsLoading(true)
    setError(null)
    try {
      if (currentTarget.type === "role") {
        if (has) {
          await permissionsApi.removePermissionFromRole(currentTarget.id, permission)
        } else {
          await permissionsApi.addPermissionToRole(currentTarget.id, permission)
        }
      } else {
        if (has) {
          await permissionsApi.removePermissionFromUser(currentTarget.id, permission)
        } else {
          await permissionsApi.addPermissionToUser(currentTarget.id, permission)
        }
      }
      setLoadedPermissions((prev) =>
        has ? prev.filter((p) => p !== permission) : [...prev, permission],
      )
    } catch (e: any) {
      setError(e?.response?.data?.message || e?.message || "Không thể cập nhật quyền.")
    } finally {
      setIsLoading(false)
    }
  }

  const renderPermissionRow = (permission: PermissionKey) => {
    const checked = loadedPermissions.includes(permission)
    return (
      <div
        key={permission}
        className="flex items-center justify-between rounded-md border bg-card px-3 py-2 text-sm"
      >
        <div className="flex flex-col">
          <span className="font-mono text-xs text-muted-foreground">
            {permission}
          </span>
        </div>
        <Button
          type="button"
          size="sm"
          variant={checked ? "outline" : "default"}
          disabled={isLoading}
          onClick={() => togglePermission(permission)}
        >
          {checked ? "Bỏ quyền" : "Cấp quyền"}
        </Button>
      </div>
    )
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6">
        <h2 className="text-2xl font-bold tracking-tight">Cài Đặt</h2>
        <p className="text-muted-foreground">
          Quản lý cài đặt hệ thống và cấu hình
        </p>
      </div>
      <Tabs defaultValue="security" className="space-y-4">
        <TabsList>
          <TabsTrigger value="general">Tổng Quan</TabsTrigger>
          <TabsTrigger value="notifications">Thông Báo</TabsTrigger>
          <TabsTrigger value="security">Bảo Mật</TabsTrigger>
          <TabsTrigger value="system">Hệ Thống</TabsTrigger>
        </TabsList>

        <TabsContent value="security" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Bảo Mật &amp; Phân Quyền</CardTitle>
              <CardDescription>
                Cấu hình quyền cho từng role hoặc từng user. Chỉ admin có quyền{" "}
                <code className="mx-1 rounded bg-muted px-1 py-0.5 text-xs">
                  Permission.ManagePermissions
                </code>
                mới truy cập được trang này.
              </CardDescription>
            </CardHeader>
            <CardContent className="space-y-6">
              <div className="grid gap-4 md:grid-cols-[260px_minmax(0,1fr)]">
                <div className="space-y-4">
                  <div className="space-y-2">
                    <Label>Đối tượng</Label>
                    <div className="flex gap-2">
                      <Button
                        type="button"
                        variant={currentTarget.type === "role" ? "default" : "outline"}
                        size="sm"
                        onClick={() =>
                          setCurrentTarget((prev) => ({ ...prev, type: "role" }))
                        }
                      >
                        Role
                      </Button>
                      <Button
                        type="button"
                        variant={currentTarget.type === "user" ? "default" : "outline"}
                        size="sm"
                        onClick={() =>
                          setCurrentTarget((prev) => ({ ...prev, type: "user" }))
                        }
                      >
                        User
                      </Button>
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label htmlFor="target-id">
                      {currentTarget.type === "role" ? "Role Id" : "User Id"}
                    </Label>
                    <Input
                      id="target-id"
                      placeholder={
                        currentTarget.type === "role"
                          ? "VD: Admin role id trong AspNetRoles"
                          : "VD: user Guid trong AspNetUsers"
                      }
                      value={currentTarget.id}
                      onChange={(e) =>
                        setCurrentTarget((prev) => ({ ...prev, id: e.target.value }))
                      }
                    />
                  </div>
                  <Button
                    type="button"
                    size="sm"
                    onClick={loadPermissions}
                    disabled={isLoading || !currentTarget.id.trim()}
                  >
                    Tải quyền
                  </Button>
                  {error ? (
                    <p className="text-xs text-destructive">{error}</p>
                  ) : null}
                  <Separator />
                  <p className="text-xs text-muted-foreground">
                    Gợi ý: bạn có thể copy Id role/user từ database hoặc xây thêm
                    UI quản lý user/role sau.
                  </p>
                </div>

                <div className="space-y-3">
                  <div className="flex items-center justify-between">
                    <div>
                      <p className="font-medium">Danh sách permission</p>
                      <p className="text-xs text-muted-foreground">
                        Tick để cấp quyền, bỏ tick để thu hồi cho{" "}
                        {currentTarget.type === "role" ? "role" : "user"} hiện tại.
                      </p>
                    </div>
                  </div>
                  <Separator />
                  {isLoading && (
                    <div className="py-8 text-center text-sm text-muted-foreground">
                      Đang tải / cập nhật quyền...
                    </div>
                  )}
                  {!isLoading && !currentTarget.id.trim() && (
                    <div className="py-8 text-center text-sm text-muted-foreground">
                      Nhập Id role/user và bấm &quot;Tải quyền&quot; để xem / chỉnh sửa.
                    </div>
                  )}
                  {!isLoading && currentTarget.id.trim() && (
                    <div className="grid gap-2">
                      {ALL_PERMISSIONS.map(renderPermissionRow)}
                    </div>
                  )}
                </div>
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="general" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Cài Đặt Chung</CardTitle>
              <CardDescription>
                Cấu hình các thông tin chung của hệ thống
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="py-8 text-center text-muted-foreground">
                Cài đặt chung sẽ được hiển thị ở đây
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="notifications" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Cài Đặt Thông Báo</CardTitle>
              <CardDescription>
                Quản lý các thông báo và email
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="py-8 text-center text-muted-foreground">
                Cài đặt thông báo sẽ được hiển thị ở đây
              </div>
            </CardContent>
          </Card>
        </TabsContent>

        <TabsContent value="system" className="space-y-4">
          <Card>
            <CardHeader>
              <CardTitle>Cài Đặt Hệ Thống</CardTitle>
              <CardDescription>
                Cấu hình hệ thống và tích hợp
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="py-8 text-center text-muted-foreground">
                Cài đặt hệ thống sẽ được hiển thị ở đây
              </div>
            </CardContent>
          </Card>
        </TabsContent>
      </Tabs>
    </div>
  )
}

