import * as React from "react"
import * as z from "zod"
import { Link, useNavigate, useSearchParams, useLocation } from 'react-router-dom'; // Sửa từ 'react-router' → 'react-router-dom'
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { registerSchema } from "@/lib/validations/auth"
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { toast } from "sonner"
import { GalleryVerticalEnd } from "lucide-react"
import { paths } from '@/config/paths';
import { Icons } from "./icons";
import apiClient from "@/lib/api-client"

type FormData = z.infer<typeof registerSchema>

export function RegisterForm({
  className,
  ...props
}: React.HTMLAttributes<HTMLDivElement>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(registerSchema),
  })
  const navigate = useNavigate()
  const location = useLocation()
  const [isLoading, setIsLoading] = React.useState<boolean>(false)
  const [isGoogleLoading, setIsGoogleLoading] = React.useState<boolean>(false)
  const [searchParams] = useSearchParams()
  const redirectTo = searchParams?.get("redirectTo")

  async function onSubmit(data: FormData) {
    setIsLoading(true)
    try {
      const res = await apiClient.auth.register({
        userName: (data as any).name ?? (data as any).username ?? "",
        email: (data as any).email,
        password: (data as any).password,
      })
      const result = res.data
      toast("Registration submitted", {
        description: "Please verify OTP sent to your email",
      })
      navigate(paths.auth.otp.getHref(), {
        state: {
          email: result.email,
          from: location.state?.from,
        },
      })
    } catch (err: any) {
      console.error("Register error:", err)
      const msg = err?.response?.data?.message || err?.message || "Registration failed"
      toast("Registration failed", { description: String(msg) })
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <div className={cn("flex flex-col gap-6", className)} {...props}>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex flex-col gap-6">
          <div className="flex flex-col items-center gap-2">
            <Link
              to={paths.home.getHref()}
              className="flex flex-col items-center gap-2 font-medium"
            >
              <div className="flex size-8 items-center justify-center rounded-md">
                <GalleryVerticalEnd className="size-6" />
              </div>
              <span className="sr-only">Acme Inc.</span>
            </Link>
            <h1 className="text-xl font-bold">Create an account</h1>
          </div>

          <div className="flex flex-col gap-6">
            <div className="grid gap-3">
              <Label htmlFor="name">Username</Label>
              <Input id="name" {...register("name" as any)} placeholder="johndoe" required />
              {errors?.name && <p className="px-1 text-xs text-red-600">{(errors as any).name?.message}</p>}
            </div>

            <div className="grid gap-3">
              <Label htmlFor="email">Email</Label>
              <Input id="email" type="email" {...register("email" as any)} placeholder="m@example.com" required />
              {errors?.email && <p className="px-1 text-xs text-red-600">{(errors as any).email?.message}</p>}
            </div>

            <div className="grid gap-3">
              <Label htmlFor="password">Password</Label>
              <Input id="password" type="password" {...register("password" as any)} placeholder="********" required />
              {errors?.password && <p className="px-1 text-xs text-red-600">{(errors as any).password?.message}</p>}
            </div>

            <Button type="submit" className="w-full" disabled={isLoading}>
              {isLoading && <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />}
              Sign up
            </Button>
          </div>

          <div className="after:border-border relative text-center text-sm after:absolute after:inset-0 after:top-1/2 after:z-0 after:flex after:items-center after:border-t">
            <span className="bg-background text-muted-foreground relative z-10 px-2">
              Or
            </span>
          </div>

          <div className="grid gap-4 sm:grid-cols-1">
            <Button
              variant="outline"
              type="button"
              className="w-full"
              onClick={() => {
                setIsGoogleLoading(true)
                // TODO: Implement Google OAuth
                setTimeout(() => setIsGoogleLoading(false), 2000)
              }}
              disabled={isLoading || isGoogleLoading}
            >
              {isGoogleLoading ? (
                <Icons.spinner className="mr-2 h-4 w-4 animate-spin" />
              ) : (
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 24 24"
                  className="mr-2 h-4 w-4"
                >
                  <path
                    d="M12.48 10.92v3.28h7.84c-.24 1.84-.853 3.187-1.787 4.133-1.147 1.147-2.933 2.4-6.053 2.4-4.827 0-8.6-3.893-8.6-8.72s3.773-8.72 8.6-8.72c2.6 0 4.507 1.027 5.907 2.347l2.307-2.307C18.747 1.44 16.133 0 12.48 0 5.867 0 .307 5.387.307 12s5.56 12 12.173 12c3.573 0 6.267-1.173 8.373-3.36 2.16-2.16 2.84-5.213 2.84-7.667 0-.76-.053-1.467-.173-2.053H12.48z"
                    fill="currentColor"
                  />
                </svg>
              )}
              Continue with Google
            </Button>

            <div className="text-center text-sm">
              Already have an account?{" "}
              <Link to={paths.auth.login.getHref()} className="underline underline-offset-4">
                Sign in
              </Link>
            </div>
          </div>
        </div>
      </form>
    </div>
  )
}