// import * as React from "react"
// import { Outlet, Link, useLocation, useNavigate } from "react-router-dom"
// import {
//   IconDashboard,
//   IconUsers,
//   IconUsersGroup,
//   IconShield,
//   IconChevronLeft,
//   IconSearch,
// } from "@tabler/icons-react"
// import { cn } from "@/lib/utils"
// import { Input } from "@/components/ui/input"
// import { Button } from "@/components/ui/button"
// import {
//   Accordion,
//   AccordionContent,
//   AccordionItem,
//   AccordionTrigger,
// } from "@/components/ui/accordion"

// const navigationItems = [
//   {
//     title: "Dashboard",
//     url: "/admin/settings",
//     icon: IconDashboard,
//   },
// ]

// const accessManagementItems = [
//   {
//     title: "User groups",
//     url: "/admin/settings/user-groups",
//     icon: IconUsersGroup,
//   },
//   {
//     title: "Users",
//     url: "/admin/settings/users",
//     icon: IconUsers,
//   },
//   {
//     title: "Roles",
//     url: "/admin/settings/roles",
//     icon: IconShield,
//   },
// ]

// export default function SettingsLayout() {
//   const location = useLocation()
//   const navigate = useNavigate()
//   const [searchQuery, setSearchQuery] = React.useState("")

//   const isActive = (url: string) => {
//     if (url === "/admin/settings") {
//       return location.pathname === url
//     }
//     return location.pathname.startsWith(url)
//   }

//   return (
//     <div className="flex absolute inset-0 -mx-4 -my-4 md:-mx-6 md:-my-6">
//       {/* Sidebar */}
//       <aside className="w-64 border-r bg-background flex flex-col flex-shrink-0">
//         {/* Header */}
//         <div className="p-4 border-b">
//           <div className="flex items-center gap-2 mb-4">
//             <Button
//               variant="ghost"
//               size="icon"
//               className="h-8 w-8"
//               onClick={() => navigate("/admin/dashboard")}
//             >
//               <IconChevronLeft className="h-4 w-4" />
//             </Button>
//             <h2 className="text-lg font-semibold">Identity and Access Management (IAM)</h2>
//           </div>
//           <div className="relative">
//             <IconSearch className="absolute left-2 top-1/2 transform -translate-y-1/2 h-4 w-4 text-muted-foreground" />
//             <Input
//               placeholder="Search IAM"
//               value={searchQuery}
//               onChange={(e) => setSearchQuery(e.target.value)}
//               className="pl-8"
//             />
//           </div>
//         </div>

//         {/* Navigation Content */}
//         <div className="flex-1 overflow-y-auto p-4 space-y-2">
//           {/* Dashboard */}
//           {navigationItems.map((item) => (
//             <Link
//               key={item.url}
//               to={item.url}
//               className={cn(
//                 "flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors",
//                 isActive(item.url)
//                   ? "bg-primary text-primary-foreground"
//                   : "hover:bg-accent hover:text-accent-foreground"
//               )}
//             >
//               <item.icon className="h-4 w-4" />
//               <span>{item.title}</span>
//             </Link>
//           ))}

//           {/* Access Management Section */}
//           <Accordion type="single" collapsible defaultValue="access-management" className="w-full">
//             <AccordionItem value="access-management" className="border-none">
//               <AccordionTrigger className="px-3 py-2 text-sm font-medium hover:no-underline">
//                 Access management
//               </AccordionTrigger>
//               <AccordionContent className="pt-2">
//                 <div className="space-y-1">
//                   {accessManagementItems.map((item) => (
//                     <Link
//                       key={item.url}
//                       to={item.url}
//                       className={cn(
//                         "flex items-center gap-3 px-3 py-2 rounded-md text-sm transition-colors ml-4",
//                         isActive(item.url)
//                           ? "bg-primary text-primary-foreground"
//                           : "hover:bg-accent hover:text-accent-foreground"
//                       )}
//                     >
//                       <item.icon className="h-4 w-4" />
//                       <span>{item.title}</span>
//                     </Link>
//                   ))}
//                 </div>
//               </AccordionContent>
//             </AccordionItem>
//           </Accordion>
//         </div>
//       </aside>

//       {/* Main Content */}
//       <div className="flex-1 overflow-hidden flex flex-col bg-background">
//         <Outlet />
//       </div>
//     </div>
//   )
// }
