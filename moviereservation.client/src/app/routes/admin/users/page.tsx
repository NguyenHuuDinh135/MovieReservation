import * as React from "react"

import { AdminDataState } from "@/components/admin-data-state"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { useAdminBookingsByUser } from "@/hooks/use-admin-data"
import { IconSearch } from "@tabler/icons-react"

export default function AdminUsersPage() {
  const [userIdInput, setUserIdInput] = React.useState("")
  const [selectedUserId, setSelectedUserId] = React.useState<string | undefined>()
  const {
    data: bookingsByUser = [],
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminBookingsByUser(selectedUserId)

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setSelectedUserId(userIdInput.trim() || undefined)
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6">
        <h2 className="text-2xl font-bold tracking-tight">Hoạt Động Người Dùng</h2>
        <p className="text-muted-foreground">
          Tra cứu lịch sử đặt vé dựa trên mã người dùng từ hệ thống Identity
        </p>
      </div>
      <Card className="mb-6">
        <CardHeader>
          <CardTitle>Tìm kiếm theo User ID</CardTitle>
          <CardDescription>
            Nhập mã người dùng (Guid) để đồng bộ đơn đặt vé của họ
          </CardDescription>
        </CardHeader>
        <CardContent>
          <form
            onSubmit={handleSubmit}
            className="flex flex-col gap-3 md:flex-row md:items-center"
          >
            <Input
              placeholder="VD: 7f13c28c-xxxx-xxxx-xxxx-7a4250..."
              value={userIdInput}
              onChange={(event) => setUserIdInput(event.target.value)}
            />
            <Button type="submit" className="md:w-fit">
              <IconSearch className="mr-2 h-4 w-4" />
              Tra cứu
            </Button>
          </form>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Lịch Sử Đặt Vé</CardTitle>
          <CardDescription>
            {selectedUserId
              ? `Kết quả cho user ${selectedUserId}`
              : "Nhập user ID để xem danh sách đặt vé"}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {selectedUserId ? (
            <AdminDataState
              isLoading={isLoading}
              isError={isError}
              error={error}
              onRetry={refetch}
              dataLength={bookingsByUser.length}
              emptyMessage="Người dùng này chưa có lịch sử đặt vé."
            >
              <div className="space-y-4">
                {bookingsByUser.map((group) => (
                  <div
                    key={`${group.title}-${group.show.showId}`}
                    className="rounded-lg border p-4"
                  >
                    <div className="flex items-center justify-between">
                      <div>
                        <p className="font-semibold">{group.title}</p>
                        <p className="text-sm text-muted-foreground">
                          Suất chiếu #{group.show.showId} •{" "}
                          {new Date(group.show.showDatetime).toLocaleString(
                            "vi-VN"
                          )}
                        </p>
                      </div>
                      <Badge variant="outline" className="uppercase">
                        {group.show.showType}
                      </Badge>
                    </div>
                    <div className="mt-4 grid gap-2 text-sm">
                      {group.bookings.map((booking) => (
                        <div
                          key={booking.bookingId}
                          className="flex flex-wrap items-center justify-between rounded-md bg-muted/40 px-3 py-2"
                        >
                          <div className="font-medium">
                            Ghế {booking.seatRow}
                            {booking.seatNumber}
                          </div>
                          <div className="flex items-center gap-3">
                            <Badge variant="secondary" className="capitalize">
                              {booking.bookingStatus}
                            </Badge>
                            <span>
                              {new Date(
                                booking.bookingDateTime
                              ).toLocaleString("vi-VN")}
                            </span>
                          </div>
                        </div>
                      ))}
                    </div>
                  </div>
                ))}
              </div>
            </AdminDataState>
          ) : (
            <div className="py-10 text-center text-sm text-muted-foreground">
              Hãy nhập user ID và bấm &quot;Tra cứu&quot; để xem dữ liệu.
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}
