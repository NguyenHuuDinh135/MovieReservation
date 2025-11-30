import { useLocation } from "react-router-dom"
import { Button } from "@/components/ui/button"
import { Separator } from "@/components/ui/separator"
import { SidebarTrigger } from "@/components/ui/sidebar"

const getPageTitle = (pathname: string): string => {
  const titles: Record<string, string> = {
    "/admin/dashboard": "Dashboard",
    "/admin/movies": "Quản Lý Phim",
    "/admin/cinemas": "Quản Lý Rạp Chiếu",
    "/admin/showtimes": "Quản Lý Suất Chiếu",
    "/admin/bookings": "Quản Lý Đặt Vé",
    "/admin/users": "Quản Lý Người Dùng",
    "/admin/settings": "Cài Đặt",
  }
  return titles[pathname] || "Admin"
}

export function AdminSiteHeader() {
  const location = useLocation()
  const pageTitle = getPageTitle(location.pathname)

  return (
    <header className="flex h-(--header-height) shrink-0 items-center gap-2 border-b transition-[width,height] ease-linear group-has-data-[collapsible=icon]/sidebar-wrapper:h-(--header-height)">
      <div className="flex w-full items-center gap-1 px-4 lg:gap-2 lg:px-6">
        <SidebarTrigger className="-ml-1" />
        <Separator
          orientation="vertical"
          className="mx-2 data-[orientation=vertical]:h-4"
        />
        <h1 className="text-base font-medium">{pageTitle}</h1>
      </div>
    </header>
  )
}

