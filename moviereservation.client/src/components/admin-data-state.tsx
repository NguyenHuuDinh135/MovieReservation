import { Icons } from "@/components/icons"
import { Button } from "@/components/ui/button"

type AdminDataStateProps = {
  isLoading: boolean
  isError: boolean
  error?: unknown
  dataLength?: number
  emptyMessage?: string
  onRetry?: () => void
  children: React.ReactNode
}

export function AdminDataState({
  isLoading,
  isError,
  error,
  dataLength,
  emptyMessage = "Hiện chưa có dữ liệu để hiển thị",
  onRetry,
  children,
}: AdminDataStateProps) {
  if (isLoading) {
    return (
      <div className="flex flex-col items-center justify-center gap-3 py-10 text-sm text-muted-foreground">
        <Icons.spinner className="size-5 animate-spin" />
        <span>Đang tải dữ liệu...</span>
      </div>
    )
  }

  if (isError) {
    return (
      <div className="flex flex-col items-center justify-center gap-3 py-10 text-center text-sm text-destructive">
        <p>
          {error instanceof Error
            ? error.message
            : "Không thể tải dữ liệu. Vui lòng thử lại sau."}
        </p>
        {onRetry ? (
          <Button size="sm" onClick={onRetry} variant="outline">
            Thử lại
          </Button>
        ) : null}
      </div>
    )
  }

  if (!dataLength) {
    return (
      <div className="py-10 text-center text-sm text-muted-foreground">
        {emptyMessage}
      </div>
    )
  }

  return <>{children}</>
}

