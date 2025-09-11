import * as React from "react"
import { Link } from "react-router-dom"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Button, type ButtonProps } from "@/components/ui/button"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuGroup,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuShortcut,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Skeleton } from "@/components/ui/skeleton"


// interface AuthDropdownProps extends React.ComponentPropsWithoutRef<typeof DropdownMenuTrigger>, 
// ButtonProps {
//   user: User | null
// }
// export function AuthDropdown({ user, className,...props }: AuthDropdownProps) {
//     return (
//       <Button size="sm" >
//         <Link to="/signin">
//           Sign In
//           <span className="sr-only">Sign In</span>
//         </Link>
//       </Button>
//     )
//  }

export function AuthDropdown() {
    return (
      <Button size="sm" >
        <Link to="/signin">
          Sign In
          <span className="sr-only">Sign In</span>
        </Link>
      </Button>
    )
 }

