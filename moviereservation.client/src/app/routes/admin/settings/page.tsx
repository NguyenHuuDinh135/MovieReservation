import * as React from "react"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

export default function SettingsDashboardPage() {
  return (
    <div className="flex flex-col h-full bg-background">
      <div className="border-b bg-background">
        <div className="px-8 py-6">
          <h1 className="text-2xl font-semibold">Dashboard</h1>
          <p className="text-sm text-muted-foreground mt-1">
            Tổng quan về quản lý danh tính và truy cập
          </p>
        </div>
      </div>
      <div className="flex-1 p-8">
        <div className="grid gap-6 md:grid-cols-2 lg:grid-cols-3">
          <Card>
            <CardHeader>
              <CardTitle className="text-base">User Groups</CardTitle>
              <CardDescription>
                Quản lý các nhóm người dùng và quyền của họ
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">0</div>
              <p className="text-xs text-muted-foreground mt-1">Nhóm người dùng</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle className="text-base">Users</CardTitle>
              <CardDescription>
                Quản lý người dùng và quyền truy cập
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">0</div>
              <p className="text-xs text-muted-foreground mt-1">Người dùng</p>
            </CardContent>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle className="text-base">Roles</CardTitle>
              <CardDescription>
                Quản lý các vai trò và quyền của chúng
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">0</div>
              <p className="text-xs text-muted-foreground mt-1">Vai trò</p>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  )
}
