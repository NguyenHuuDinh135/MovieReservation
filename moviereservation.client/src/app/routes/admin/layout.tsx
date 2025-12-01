import * as React from "react"
import { AdminAppSidebar } from "@/components/admin-app-sidebar"
import { AdminSiteHeader } from "@/components/admin-site-header"
import {
  SidebarInset,
  SidebarProvider,
} from "@/components/ui/sidebar"
import { Navigate, Outlet, useLocation, useRouteError, isRouteErrorResponse, useNavigate } from "react-router-dom"
import { getAccessToken } from "@/lib/auth"
import { paths } from "@/config/paths"
import { useMyPermissions } from "@/hooks/use-permissions"
import { toast } from "sonner"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { AlertCircle } from "lucide-react"
import { ADMIN_PERMISSIONS } from "@/lib/api-permissions"
import { ServerError } from "@/components/errors/500"
import { NotFoundError } from "@/components/errors/404"
import { GenericError } from "@/components/errors/generic"

export function ErrorBoundary() {
  const error = useRouteError()
  const navigate = useNavigate()

  const handleReset = () => {
    navigate(0) // Reload current page
  }

  if (isRouteErrorResponse(error)) {
    if (error.status === 404) {
      return <NotFoundError />
    }
    if (error.status === 500) {
      return <ServerError error={error.data as Error} resetErrorBoundary={handleReset} />
    }
    return (
      <GenericError
        error={error.data as Error}
        resetErrorBoundary={handleReset}
        title={`${error.status} - Lỗi`}
        description={error.statusText || 'Đã xảy ra lỗi không mong muốn'}
      />
    )
  }

  const errorMessage =
    error instanceof Error ? error.message : 'Đã xảy ra lỗi không mong muốn'
  return (
    <GenericError
      error={error instanceof Error ? error : undefined}
      resetErrorBoundary={handleReset}
      title="Lỗi quản trị"
      description={errorMessage}
    />
  )
}

export default function AdminLayout() {
  const location = useLocation()
  const token = getAccessToken() ?? undefined
  const { data: permissions, isLoading, isError } = useMyPermissions(token)

  // Tính toán hasAdminPermission ngay sau khi có dữ liệu permissions
  const hasAdminPermission = permissions?.some((p) =>
    ADMIN_PERMISSIONS.includes(p)
  )

  // ❗ Tất cả hooks phải được gọi TRƯỚC các early returns
  React.useEffect(() => {
    // Chỉ chạy khi đã có token và đã load xong permissions
    if (token && !isLoading && hasAdminPermission === false) {
      toast.error("Không có quyền truy cập", {
        description:
          "Bạn không có quyền truy cập vào trang quản trị. Vui lòng liên hệ quản trị viên.",
      })
      const timer = setTimeout(() => {
        window.location.href = paths.home.path
      }, 5000)
      return () => clearTimeout(timer)
    }
  }, [token, isLoading, hasAdminPermission])

  // Các return sớm (early return) SAU KHI tất cả hooks đã được gọi
  if (!token) {
    return (
      <Navigate
        to={paths.auth.login.path}
        replace
        state={{ from: location }}
      />
    )
  }

  if (isLoading) {
    return (
      <div className="flex h-screen items-center justify-center">
        <div className="text-center">
          <div className="mb-4 inline-block h-8 w-8 animate-spin rounded-full border-4 border-solid border-current border-r-transparent"></div>
          <p className="text-sm text-muted-foreground">
            Đang kiểm tra quyền truy cập...
          </p>
        </div>
      </div>
    )
  }

  if (isError) {
    return (
      <div className="flex h-screen items-center justify-center p-4">
        <Card className="max-w-md border-destructive">
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-destructive">
              <AlertCircle className="h-5 w-5" />
              Lỗi
            </CardTitle>
            <CardDescription>
              Không thể kiểm tra quyền truy cập. Vui lòng thử lại sau.
            </CardDescription>
          </CardHeader>
        </Card>
      </div>
    )
  }

  if (!hasAdminPermission) {
    return (
      <div className="flex h-screen items-center justify-center p-4">
        <Card className="max-w-md border-destructive">
          <CardHeader>
            <CardTitle className="flex items-center gap-2 text-destructive">
              <AlertCircle className="h-5 w-5" />
              Không có quyền truy cập
            </CardTitle>
            <CardDescription>
              Bạn không có quyền truy cập vào trang quản trị. Vui lòng liên hệ quản trị viên để được cấp quyền.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <p className="text-sm text-muted-foreground">
              Đang chuyển hướng về trang chủ...
            </p>
          </CardContent>
        </Card>
      </div>
    )
  }

  return (
    <SidebarProvider
      style={
        {
          "--sidebar-width": "calc(var(--spacing) * 72)",
          "--header-height": "calc(var(--spacing) * 12)",
        } as React.CSSProperties
      }
    >
      <AdminAppSidebar variant="inset" />
      <SidebarInset>
        <AdminSiteHeader />
        <div className="flex flex-1 flex-col">
          <div className="@container/main flex flex-1 flex-col gap-2">
            <div className="flex flex-col gap-4 py-4 md:gap-6 md:py-6 relative">
              <Outlet />
            </div>
          </div>
        </div>
      </SidebarInset>
    </SidebarProvider>
  )
}
