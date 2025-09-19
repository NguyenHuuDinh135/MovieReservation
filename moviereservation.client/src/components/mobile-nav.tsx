"use client"

import React from "react"
import { Popover, PopoverContent, PopoverTrigger } from "./ui/popover"
import { Button } from "./ui/button"
import { cn } from "@/lib/utils"
import { Link, useNavigate } from "react-router-dom"
import type { LinkProps } from "react-router-dom"
import { siteConfig } from "@/config/site" // ✅ Import config của bạn

export function MobileNav({ className }: { className?: string }) {
  const [open, setOpen] = React.useState(false)

  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="ghost"
          className={cn(
            "extend-touch-target h-8 touch-manipulation items-center justify-start gap-2.5 !p-0 hover:bg-transparent focus-visible:bg-transparent focus-visible:ring-0 active:bg-transparent dark:hover:bg-transparent",
            className
          )}
        >
          <div className="relative flex h-8 w-4 items-center justify-center">
            <div className="relative size-4">
              <span
                className={cn(
                  "bg-foreground absolute left-0 block h-0.5 w-4 transition-all duration-100",
                  open ? "top-[0.4rem] -rotate-45" : "top-1"
                )}
              />
              <span
                className={cn(
                  "bg-foreground absolute left-0 block h-0.5 w-4 transition-all duration-100",
                  open ? "top-[0.4rem] rotate-45" : "top-2.5"
                )}
              />
            </div>
            <span className="sr-only">Toggle Menu</span>
          </div>
          <span className="flex h-8 items-center text-lg leading-none font-medium">
            Menu
          </span>
        </Button>
      </PopoverTrigger>

      <PopoverContent
        className="bg-background/90 no-scrollbar h-(--radix-popper-available-height) w-(--radix-popper-available-width) overflow-y-auto rounded-none border-none p-0 shadow-none backdrop-blur duration-100"
        align="start"
        side="bottom"
        alignOffset={-16}
        sideOffset={14}
      >
        <div className="flex flex-col gap-12 overflow-auto px-6 py-6">
          <div className="flex flex-col gap-4">
            <div className="text-muted-foreground text-sm font-medium">
              Navigation
            </div>
            <div className="flex flex-col gap-3">
              {siteConfig.mainNav.map((item) => (
                <MobileLink
                  key={item.to}
                  to={item.to}
                  onOpenChange={setOpen}
                >
                  {item.title}
                </MobileLink>
              ))}
            </div>
          </div>
        </div>
      </PopoverContent>
    </Popover>
  )
}

function MobileLink({
  to,
  onOpenChange,
  className,
  children,
  ...props
}: LinkProps & {
  onOpenChange?: (open: boolean) => void
  children: React.ReactNode
  className?: string
}) {
  const navigate = useNavigate()

  const handleClick = (e: React.MouseEvent<HTMLAnchorElement>) => {
    e.preventDefault()
    navigate(to)
    onOpenChange?.(false)
  }

  return (
    <Link
      to={to}
      onClick={handleClick}
      className={cn("text-2xl font-medium", className)}
      {...props}
    >
      {children}
    </Link>
  )
}