import type { Icons } from "@/components/icons"

export interface NavItem {
  title: string
  to?: string
  active?: boolean
  disabled?: boolean
  external?: boolean
  icon?: keyof typeof Icons
  label?: string
  description?: string
}

export interface NavItemWithChildren extends NavItem {
  items?: NavItemWithChildren[]
}

export interface FooterItem {
  title: string
  items: {
    title: string
    to: string
    external?: boolean
  }[]
}

export type MainNavItem = NavItemWithChildren

export type SidebarNavItem = NavItemWithChildren

export interface FooterItem {
  title: string
  items: {
    title: string
    to: string
    external?: boolean
  }[]
}