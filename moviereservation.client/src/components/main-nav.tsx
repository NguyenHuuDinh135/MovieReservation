
import React from "react"
import { Link, useLocation } from "react-router"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { siteConfig } from "@/config/site"

export function MainNav({ className, ...props }: React.ComponentProps<"nav">) {
  const location = useLocation()

  return (
    <nav className={cn("flex items-center gap-0.5", className)} {...props}>
      {siteConfig.mainNav.map((item) => (
        <Button key={item.to} variant="ghost" asChild size="sm">
          <Link
            to={item.to}
            className={cn(
              "transition-colors hover:text-primary",
              location.pathname === item.to && "text-primary"
            )}
          >
            {item.title}
          </Link>
        </Button>
      ))}
    </nav>
  )
}