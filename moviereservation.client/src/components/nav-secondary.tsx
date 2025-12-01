import * as React from "react"
import { type Icon } from "@tabler/icons-react"
import { Link } from "react-router-dom"
import { IconChevronRight } from "@tabler/icons-react"

import {
  SidebarGroup,
  SidebarGroupContent,
  SidebarMenu,
  SidebarMenuButton,
  SidebarMenuItem,
  SidebarMenuSub,
  SidebarMenuSubItem,
  SidebarMenuSubButton,
} from "@/components/ui/sidebar"
import { Collapsible, CollapsibleContent, CollapsibleTrigger } from "@/components/ui/collapsible"

export type NavSecondaryItem = {
  title: string
  url?: string
  icon: Icon
  items?: NavSecondaryItem[]
}

export function NavSecondary({
  items,
  currentPath,
  ...props
}: {
  items: NavSecondaryItem[]
  currentPath?: string
} & React.ComponentPropsWithoutRef<typeof SidebarGroup>) {
  const hasActiveSubItem = (item: NavSecondaryItem): boolean => {
    if (!item.items) return false
    return item.items.some(
      (subItem) =>
        (subItem.url && currentPath === subItem.url) ||
        (subItem.url && currentPath?.startsWith(subItem.url + "/"))
    )
  }

  const renderItem = (item: NavSecondaryItem) => {
    const hasSubItems = item.items && item.items.length > 0
    const isActive = Boolean(
      (item.url && currentPath === item.url) ||
      (item.url && currentPath?.startsWith(item.url + "/"))
    )
    const hasActiveChild = hasActiveSubItem(item)
    const isOpen = hasActiveChild

    if (hasSubItems) {
      return (
        <Collapsible key={item.title} defaultOpen={isOpen} className="group/collapsible">
          <SidebarMenuItem>
            <CollapsibleTrigger asChild>
              <Link to={item.url || "#"}>
                <SidebarMenuButton tooltip={item.title} isActive={isActive || hasActiveChild}>
                    {item.icon && <item.icon />}
                  <span>{item.title}</span>
                  <IconChevronRight className="ml-auto transition-transform duration-200 group-data-[state=open]/collapsible:rotate-90" />
                </SidebarMenuButton>
              </Link>
            </CollapsibleTrigger>
            <CollapsibleContent className="overflow-hidden transition-all data-[state=closed]:animate-collapsible-up data-[state=open]:animate-collapsible-down">
              <SidebarMenuSub>
                {item.items!.map((subItem) => {
                  const isSubActive = Boolean(
                    (subItem.url && currentPath === subItem.url) ||
                    (subItem.url && currentPath?.startsWith(subItem.url + "/"))
                  )
                  return (
                    <SidebarMenuSubItem key={subItem.title}>
                      {subItem.url ? (
                        <SidebarMenuSubButton asChild isActive={isSubActive}>
                          <Link to={subItem.url}>
                            {subItem.icon && <subItem.icon />}
                            <span>{subItem.title}</span>
                          </Link>
                        </SidebarMenuSubButton>
                      ) : (
                        <SidebarMenuSubButton isActive={isSubActive}>
                          {subItem.icon && <subItem.icon />}
                          <span>{subItem.title}</span>
                        </SidebarMenuSubButton>
                      )}
                    </SidebarMenuSubItem>
                  )
                })}
              </SidebarMenuSub>
            </CollapsibleContent>
          </SidebarMenuItem>
        </Collapsible>
      )
    }

    return (
      <SidebarMenuItem key={item.title}>
        {item.url ? (
          <SidebarMenuButton tooltip={item.title} asChild isActive={isActive}>
            <Link to={item.url}>
              {item.icon && <item.icon />}
              <span>{item.title}</span>
            </Link>
          </SidebarMenuButton>
        ) : (
          <SidebarMenuButton tooltip={item.title} isActive={isActive}>
            {item.icon && <item.icon />}
            <span>{item.title}</span>
          </SidebarMenuButton>
        )}
      </SidebarMenuItem>
    )
  }

  return (
    <SidebarGroup {...props}>
      <SidebarGroupContent className="flex flex-col gap-2">
        <SidebarMenu>
          {items.map(renderItem)}
        </SidebarMenu>
      </SidebarGroupContent>
    </SidebarGroup>
  )
}
