import * as React from "react"
import {
  IconDashboard,
  IconHelp,
  IconInnerShadowTop,
  IconMovie,
  IconBuildingStore,
  IconCalendarTime,
  IconTicket,
  IconUsers,
  IconSettings,
  IconSearch,
  IconCategory,
} from "@tabler/icons-react"
import { Link, useLocation } from "react-router-dom"

import { NavMain } from "@/components/nav-main"
import { NavSecondary } from "@/components/nav-secondary"
import { NavUser } from "@/components/admin-nav"
import {
  Sidebar,
  SidebarContent,
  SidebarFooter,
  SidebarHeader,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
} from "@/components/ui/sidebar"

const data = {
  // Default user data
  user: {
    name: "Admin",
    email: "admin@moviereservation.com",
    avatar: "/avatars/admin.jpg",
  },
  navMain: [
    {
      title: "Dashboard",
      url: "/admin/dashboard",
      icon: IconDashboard,
    },
    {
      title: "Phim",
      url: "/admin/movies",
      icon: IconMovie,
    },
    {
      title: "Thể Loại",
      url: "/admin/genres",
      icon: IconCategory,
    },
    {
      title: "Rạp Chiếu",
      url: "/admin/cinemas",
      icon: IconBuildingStore,
    },
    {
      title: "Suất Chiếu",
      url: "/admin/showtimes",
      icon: IconCalendarTime,
    },
    {
      title: "Đặt Vé",
      url: "/admin/bookings",
      icon: IconTicket,
    },
    {
      title: "Người Dùng",
      url: "/admin/users",
      icon: IconUsers,
    },
  ],
  navSecondary: [
    {
      title: "Cài Đặt",
      url: "/admin/settings",
      icon: IconSettings,
    },
    {
      title: "Trợ Giúp",
      url: "#",
      icon: IconHelp,
    },
    {
      title: "Tìm Kiếm",
      url: "#",
      icon: IconSearch,
    },
  ],
}

export function AppSidebar({ ...props }: React.ComponentProps<typeof Sidebar>) {
  const location = useLocation()
  const [user, setUser] = React.useState(data.user)

  React.useEffect(() => {
    try {
      const stored = localStorage.getItem("user")
      if (stored) {
        const parsed = JSON.parse(stored) as Partial<typeof data.user>
        setUser({
          name: parsed.name || data.user.name,
          email: parsed.email || data.user.email,
          avatar: parsed.avatar || data.user.avatar,
        })
      }
    } catch (error) {
      console.error("Failed to load user from localStorage", error)
    }
  }, [])

  return (
    <Sidebar collapsible="offcanvas" {...props}>
      <SidebarHeader>
        <SidebarMenu>
          <SidebarMenuItem>
            <SidebarMenuButton
              asChild
              className="data-[slot=sidebar-menu-button]:!p-1.5"
            >
              <Link to="/admin/dashboard">
                <IconInnerShadowTop className="!size-5" />
                <span className="text-base font-semibold">Movie Reservation</span>
              </Link>
            </SidebarMenuButton>
          </SidebarMenuItem>
        </SidebarMenu>
      </SidebarHeader>
      <SidebarContent>
        <NavMain items={data.navMain} currentPath={location.pathname} />
        <NavSecondary items={data.navSecondary} className="mt-auto" />
      </SidebarContent>
      <SidebarFooter>
        <NavUser user={user} />
      </SidebarFooter>
    </Sidebar>
  )
}


