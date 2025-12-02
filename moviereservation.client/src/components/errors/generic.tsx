import { Link, useLocation } from "react-router-dom"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { IconHome, IconRefresh, IconAlertCircle } from "@tabler/icons-react"
import { paths } from "@/config/paths"

interface GenericErrorProps {
  error?: Error
  resetErrorBoundary?: () => void
  title?: string
  description?: string
}

export function GenericError({
  error,
  resetErrorBoundary,
  title = "Đã xảy ra lỗi",
  description = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại sau.",
}: GenericErrorProps) {
  const location = useLocation()
  const isAdminRoute = location.pathname.startsWith('/admin')

  const handleRefresh = () => {
    if (resetErrorBoundary) {
      resetErrorBoundary()
    } else {
      window.location.reload()
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-var(--header-height))] items-center justify-center bg-background p-4">
      <Card className="max-w-md w-full border-destructive">
        <CardHeader className="text-center">
          <div className="mx-auto mb-4 flex h-20 w-20 items-center justify-center rounded-full bg-destructive/10">
            <IconAlertCircle className="h-10 w-10 text-destructive" />
          </div>
          <CardTitle className="text-2xl font-bold text-destructive">
            {title}
          </CardTitle>
          <CardDescription className="text-base">
            {description}
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          {error && import.meta.env.DEV && (
            <div className="rounded-md bg-muted p-3">
              <p className="text-xs font-mono text-muted-foreground break-all">
                {error.message}
              </p>
              {error.stack && (
                <details className="mt-2">
                  <summary className="cursor-pointer text-xs text-muted-foreground">
                    Chi tiết lỗi
                  </summary>
                  <pre className="mt-2 overflow-auto text-xs text-muted-foreground whitespace-pre-wrap max-h-40">
                    {error.stack}
                  </pre>
                </details>
              )}
            </div>
          )}
          <div className="flex flex-col gap-2">
            <Button onClick={handleRefresh} className="w-full">
              <IconRefresh className="mr-2 h-4 w-4" />
              Thử lại
            </Button>
            <Button variant="outline" asChild className="w-full">
              <Link to={isAdminRoute ? paths.admin.root.path : paths.home.path}>
                <IconHome className="mr-2 h-4 w-4" />
                {isAdminRoute ? "Về Dashboard" : "Về trang chủ"}
              </Link>
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
