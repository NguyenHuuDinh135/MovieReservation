import * as React from "react"
import * as z from "zod"
import { useNavigate, useLocation } from "react-router"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { otpSchema } from "@/lib/validations/auth"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSeparator,
  InputOTPSlot,
} from "@/components/ui/input-otp"
import { toast } from "sonner"
import { Icons } from "./icons"
import { paths } from "@/config/paths"
import { setAccessToken } from "@/lib/auth"
import { getRedirectPathAfterLogin } from "@/lib/api-permissions"

type FormData = z.infer<typeof otpSchema>

export function OtpForm({
  className,
  ...props
}: React.ComponentProps<"div">) {
  const {
    handleSubmit,
    setValue,
    watch,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(otpSchema),
    defaultValues: {
      otp: "",
    },
  })

  const navigate = useNavigate()
  const location = useLocation()
  const [isLoading, setIsLoading] = React.useState<boolean>(false)

  const otpValue = watch("otp")

  const onSubmit = async (data: FormData) => {
    setIsLoading(true)

    try {
      const response = await fetch("/api/auth/verify-otp", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: location.state?.email,
          otpCode: data.otp,
        }),
      })

      if (!response.ok) {
        const errorData = await response.json()
        toast("Invalid OTP", {
          description: errorData.errors?.[0] || "Please check your OTP and try again.",
        })
        setIsLoading(false)
        return
      }

      const result = await response.json()
      if (result.succeeded) {
        if (result.data?.accessToken) {
          // Sử dụng setAccessToken từ auth.ts để đảm bảo key nhất quán
          setAccessToken(result.data.accessToken)
          // Lưu thêm thông tin user vào localStorage
          localStorage.setItem("user", JSON.stringify({
            name: result.data.username,
            email: result.data.email,
            avatar: "https://github.com/shadcn.png"
          }));
          toast("Login successful", {
            description: "You have been logged in.",
          })

          // Determine redirect path: check if user is admin, redirect to /admin, otherwise use default
          const defaultPath = location.state?.from?.pathname || paths.home.path
          const redirectPath = await getRedirectPathAfterLogin(defaultPath)

          navigate(redirectPath, { 
            state: {
              username: result.data.username,
              email: result.data.email,
              from: location.state?.from,
            },
          })
        } else {
          console.warn("No access token in response, but verification succeeded")

          toast("Login successful", {
            description: "You have been logged in (refresh token set in cookie).",
          })
        }
      } else {
        toast("Verification failed", {
          description: result.errors?.[0] || "Please try again.",
        })
      }
    } catch (error) {
      console.error("Error verifying OTP:", error)
      toast("Network error", {
        description: "Please check your connection and try again.",
      })
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <Card>
        <CardHeader className="text-center">
          <CardTitle className="text-xl">Enter OTP Code</CardTitle>
          <CardDescription>
            Please enter the 6-digit code sent to your email.
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)}>
            <div className="grid gap-6">
              <div className="flex items-center justify-center">
                <InputOTP
                  maxLength={6}
                  autoFocus
                  value={otpValue}
                  onChange={(value) => setValue("otp", value, { shouldValidate: true })}
                >
                  <InputOTPGroup>
                    <InputOTPSlot index={0} />
                    <InputOTPSlot index={1} />
                    <InputOTPSlot index={2} />
                  </InputOTPGroup>
                  <InputOTPSeparator />
                  <InputOTPGroup>
                    <InputOTPSlot index={3} />
                    <InputOTPSlot index={4} />
                    <InputOTPSlot index={5} />
                  </InputOTPGroup>
                </InputOTP>
              </div>

              {errors.otp && (
                <p className="px-1 text-center text-xs text-red-600">
                  {errors.otp.message}
                </p>
              )}

              <Button 
                type="submit" 
                className="w-full" 
                disabled={isLoading || otpValue?.length !== 6}
              >
                {isLoading && <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />}
                Confirm
              </Button>
            </div>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}