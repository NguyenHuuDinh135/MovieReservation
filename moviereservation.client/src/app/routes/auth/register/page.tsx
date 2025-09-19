import { Icons } from "@/components/icons"
import { RegisterForm } from "@/components/register-form"
import { buttonVariants } from "@/components/ui/button"
import { cn } from "@/lib/utils"
import { useNavigate } from "react-router-dom"
export default function RegisterPage() {
  const navigate = useNavigate()

  const handleGoBack = () => {
    navigate(-1) // Quay lại trang trước
  }
  return (
    <div className="bg-background flex min-h-svh flex-col items-center justify-center gap-6 p-6 md:p-10">
      <button
        onClick={handleGoBack}
        className={cn(
          buttonVariants({ variant: "ghost" }),
          "absolute left-4 top-4 md:left-8 md:top-8"
        )}
      >
        <>
          <Icons.chevronLeft className="mr-2 h-4 w-4" />
          Back
        </>
      </button>
      <div className="w-full max-w-sm">
        <RegisterForm />
      </div>
    </div>
  )
}