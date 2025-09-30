
import { Link } from "react-router"
import { siteConfig } from "@/config/site" // ✅ Lấy cấu hình site (mainNav, name, links...)
import { Icons } from "@/components/icons"
import { MainNav } from "@/components/main-nav"
import { MobileNav } from "@/components/mobile-nav"
import { NavUser } from "@/components/nav-user"
import { ModeSwitcher } from "@/components/mode-switcher"
import { Button } from "@/components/ui/button" // ✅ Đảm bảo import đúng từ UI của bạn
import { Separator } from "@/components/ui/separator"
import React from "react"
import { CommandMenu } from "@/components/command-menu"
import { SiteConfig } from "@/components/site-config"
// import { SiteConfig } from "@/components/site-config"
export function SiteHeader() {
  //const location = useLocation()
  const [user, setUser] = React.useState<{
    name: string | null
    email: string | null
    avatar: string | null
  } | null>(null)

  React.useEffect(() => {
    const storedUser = localStorage.getItem("user")
    if (storedUser) {
      try {
        setUser(JSON.parse(storedUser))
      } catch (e) {
        console.error("Failed to parse user from localStorage", e)
        setUser(null)
      }
    }
  }, [])
  return (
    <header className="bg-background sticky top-0 z-50 w-full">
      <div className="container-wrapper 3xl:fixed:px-0 px-6">
        <div className="3xl:fixed:container flex h-(--header-height) items-center gap-2 **:data-[slot=separator]:!h-4">
          <MobileNav
            className="flex lg:hidden"
          />
          <Button
            asChild
            variant="ghost"
            size="icon"
            className="hidden size-8 lg:flex"
          >
            <Link to="/">
              <Icons.logo className="size-5" />
              <span className="sr-only">{siteConfig.name}</span>
            </Link>
          </Button>
          <MainNav className="hidden lg:flex" />
          <div className="ml-auto flex items-center gap-2 md:flex-1 md:justify-end">
            <div className="hidden w-full flex-1 md:flex md:w-auto md:flex-none">
              <CommandMenu
                
              />
            </div>
            <Separator
              orientation="vertical"
              className="ml-2 hidden lg:block"
            />
            <ModeSwitcher />
            <Separator orientation="vertical" className="3xl:flex hidden" />
            <SiteConfig className="3xl:flex hidden" />
            
            <Separator orientation="vertical" />
            
            <NavUser user={user || { name: null, email: null, avatar: null }} />
          </div>
        </div>
      </div>
    </header>
  )
}