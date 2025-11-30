
import { Button, type ButtonProps } from "@/components/ui/button"
import {
  BadgeCheck,
  Bell, 
  CreditCard,
  LogOut,
  Sparkles,
} from "lucide-react"
import { useNavigate } from "react-router"
import {
  Avatar,
  AvatarFallback,
  AvatarImage,
} from "@/components/ui/avatar"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { paths } from "@/config/paths"
import { Link } from "react-router"
import { cn } from "@/lib/utils"
import { clearAllAuthData } from "@/lib/auth"
import { authApi } from "@/lib/api-client"

interface NavUserProps 
  extends React.ComponentPropsWithRef<typeof DropdownMenuTrigger>,
    ButtonProps {
      user: {
    name: string | null
    email: string | null
    avatar: string | null
  }
}
export function NavUser({
  user,
  className,
  ...props
}: NavUserProps) {
  const navigate = useNavigate()
  if (!user.name && !user.email && !user.avatar ) {
    return (
      <Button size="sm" className={cn(className)} {...props} asChild>
        <Link to={paths.auth.login.getHref()}>
          Sign In
          <span className="sr-only">Sign In</span>
        </Link>
      </Button>
    )
  }
  return (
    <DropdownMenu>
      <DropdownMenuTrigger asChild>
        <div className="flex cursor-pointer items-center gap-2">
            <div className="grid flex-1 text-left text-sm leading-tight">
            <span className="truncate font-medium">{user.name}</span>
            <span className="truncate text-xs">{user.email}</span>
          </div>
          <Avatar className="h-8 w-8 rounded-lg">
            <AvatarImage src={user.avatar ?? "https://github.com/shadcn.png"} alt={user.name ?? ""} />
            <AvatarFallback className="rounded-lg">CN</AvatarFallback>
          </Avatar>
          
        </div>
      </DropdownMenuTrigger>

      <DropdownMenuContent
        className="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
        side="bottom"
        align="end"
        sideOffset={4}
      >
        <DropdownMenuLabel className="p-0 font-normal">
          <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
            <Avatar className="h-8 w-8 rounded-lg">
              <AvatarImage src={user.avatar ?? "https://github.com/shadcn.png"} alt={user.name ?? ""} />
              <AvatarFallback className="rounded-lg">CN</AvatarFallback>
            </Avatar>
            <div className="grid flex-1 text-left text-sm leading-tight">
              <span className="truncate font-medium">{user.name}</span>
              <span className="truncate text-xs">{user.email}</span>
            </div>
          </div>
        </DropdownMenuLabel>

        <DropdownMenuSeparator />

        <DropdownMenuGroup>
          <DropdownMenuItem>
            <Sparkles className="mr-2 size-4" />
            Upgrade to Pro
          </DropdownMenuItem>
        </DropdownMenuGroup>

        <DropdownMenuSeparator />

        <DropdownMenuGroup>
          <DropdownMenuItem>
            <BadgeCheck className="mr-2 size-4" />
            Account
          </DropdownMenuItem>
          <DropdownMenuItem>
            <CreditCard className="mr-2 size-4" />
            Billing
          </DropdownMenuItem>
          <DropdownMenuItem>
            <Bell className="mr-2 size-4" />
            Notifications
          </DropdownMenuItem>
        </DropdownMenuGroup>

        <DropdownMenuSeparator />

        <DropdownMenuItem 
          className="text-red-600 focus:text-red-600 focus:bg-red-50 dark:focus:bg-red-950/20" 
          onClick={async () => {
            try {
              // Gọi API logout để xóa refresh token ở server
              await authApi.logout()
            } catch (error) {
              // Bỏ qua lỗi nếu API không thành công (có thể đã mất kết nối)
              console.error("Logout API error:", error)
            }
            // Xóa toàn bộ dữ liệu xác thực ở client
            clearAllAuthData()
            navigate(paths.auth.login.path)
          }}
        >
          <LogOut className="mr-2 size-4" />
          Log out
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
  
