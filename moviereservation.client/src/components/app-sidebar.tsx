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
} from "@tabler/icons-react"
import { Link, useLocation } from "react-router-dom"

import { NavMain } from "@/components/nav-main"
import { NavSecondary } from "@/components/nav-secondary"
import { NavUser } from "@/components/nav-user"
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
        <NavUser user={data.user} />
      </SidebarFooter>
    </Sidebar>
  )
}


