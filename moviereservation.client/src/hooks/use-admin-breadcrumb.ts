import { useMemo } from "react"
import { useLocation, useParams } from "react-router-dom"
import { useAdminUsers } from "./use-admin-data"
import { useAllRoles } from "./use-permissions-data"

export interface BreadcrumbItem {
  label: string
  href?: string
}

// Route path to label mapping
const routeLabels: Record<string, string> = {
  dashboard: "Dashboard",
  movies: "Phim",
  genres: "Thể Loại",
  cinemas: "Rạp Chiếu",
  showtimes: "Suất Chiếu",
  bookings: "Đặt Vé",
  users: "Người Dùng",
  settings: "Cài Đặt",
  "user-groups": "Nhóm Người Dùng",
  roles: "Quyền",
  permissions: "Permissions",
}

export function useAdminBreadcrumb(): BreadcrumbItem[] {
  const location = useLocation()
  const params = useParams()
  const { data: users = [] } = useAdminUsers()
  const { data: roles = [] } = useAllRoles()

  return useMemo(() => {
    const pathname = location.pathname
    const pathParts = pathname.split("/").filter(Boolean)
    
    // Must start with admin
    if (pathParts[0] !== "admin") {
      return []
    }

    const breadcrumbs: BreadcrumbItem[] = []
    let currentPath = ""

    for (let i = 0; i < pathParts.length; i++) {
      const part = pathParts[i]
      currentPath += `/${part}`

      // Add admin segment to breadcrumbs (but skip it later)
      if (part === "admin") {
        continue
      }

      // Check if this is a UUID (dynamic parameter value)
      const isUUID = /^[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}$/i.test(part)
      
      if (isUUID) {
        // Determine what type of UUID this is based on route params
        if (params.userId === part) {
          // This is a userId
          const user = users.find((u) => u.id === part)
          if (user) {
            breadcrumbs.push({
              label: user.userName || user.email || part,
            })
            continue
          }
        }
        
        if (params.roleId === part) {
          // This is a roleId
          const role = roles.find((r) => r.id === part)
          if (role) {
            breadcrumbs.push({
              label: role.name || part,
            })
            continue
          }
        }
      }

      // Get label from mapping or format the segment
      const label = routeLabels[part] || part
        .split("-")
        .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
        .join(" ")

      // Last segment is current page (no href)
      const isLast = i === pathParts.length - 1
      
      breadcrumbs.push({
        label,
        href: isLast ? undefined : currentPath,
      })
    }

    return breadcrumbs
  }, [location.pathname, params, users, roles])
}
