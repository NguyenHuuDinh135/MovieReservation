import { Link, useNavigate } from "react-router-dom"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { IconHome, IconArrowLeft, IconAlertCircle } from "@tabler/icons-react"
import { paths } from "@/config/paths"
import { useLocation } from "react-router-dom"

export function NotFoundError() {
  const navigate = useNavigate()
  const location = useLocation()
  const isAdminRoute = location.pathname.startsWith('/admin')

  const handleGoBack = () => {
    if (window.history.length > 1) {
      navigate(-1)
    } else {
      navigate(isAdminRoute ? paths.admin.root.path : paths.home.path)
    }
  }

  return (
    <div className="flex min-h-[calc(100vh-var(--header-height))] items-center justify-center bg-background p-4">
      <Card className="max-w-md w-full">
        <CardHeader className="text-center">
          <div className="mx-auto mb-4 flex h-20 w-20 items-center justify-center rounded-full bg-destructive/10">
            <IconAlertCircle className="h-10 w-10 text-destructive" />
          </div>
          <CardTitle className="text-4xl font-bold">404</CardTitle>
          <CardDescription className="text-lg">
            Trang không tồn tại
          </CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <p className="text-center text-sm text-muted-foreground">
            Xin lỗi, trang bạn đang tìm kiếm không tồn tại hoặc đã bị di chuyển.
          </p>
          <div className="flex flex-col gap-2">
            <Button asChild className="w-full">
              <Link to={isAdminRoute ? paths.admin.root.path : paths.home.path}>
                <IconHome className="mr-2 h-4 w-4" />
                {isAdminRoute ? "Về Dashboard" : "Về trang chủ"}
              </Link>
            </Button>
            <Button variant="outline" className="w-full" onClick={handleGoBack}>
              <IconArrowLeft className="mr-2 h-4 w-4" />
              Quay lại
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}
