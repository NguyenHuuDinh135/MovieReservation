import { AdminDataState } from "@/components/admin-data-state"
import { Badge } from "@/components/ui/badge"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { useAdminBookings } from "@/hooks/use-admin-data"

const currencyFormatter = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0,
})

export default function AdminBookingsPage() {
  const {
    data: bookings,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminBookings()

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6">
        <h2 className="text-2xl font-bold tracking-tight">Quản Lý Đặt Vé</h2>
        <p className="text-muted-foreground">
          Theo dõi trạng thái ghế và doanh thu theo từng đơn đặt
        </p>
      </div>
      <Card>
        <CardHeader>
          <CardTitle>Danh Sách Đặt Vé</CardTitle>
          <CardDescription>
            Đã ghi nhận {bookings?.length ?? 0} giao dịch
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={isLoading}
            isError={isError}
            error={error}
            onRetry={refetch}
            dataLength={bookings?.length}
            emptyMessage="Chưa có đơn đặt vé nào."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Mã đơn</TableHead>
                  <TableHead>Người dùng</TableHead>
                  <TableHead>Suất chiếu</TableHead>
                  <TableHead>Ghế</TableHead>
                  <TableHead>Giá vé</TableHead>
                  <TableHead>Trạng thái</TableHead>
                  <TableHead>Thời gian đặt</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {bookings?.map((booking) => (
                  <TableRow key={booking.id}>
                    <TableCell className="font-medium">#{booking.id}</TableCell>
                    <TableCell>{booking.userId}</TableCell>
                    <TableCell>#{booking.showId}</TableCell>
                    <TableCell>
                      {booking.seatRow}
                      {booking.seatNumber}
                    </TableCell>
                    <TableCell>
                      {currencyFormatter.format(booking.price ?? 0)}
                    </TableCell>
                    <TableCell>
                      <Badge variant="outline" className="capitalize">
                        {booking.status?.toString()}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      {new Date(booking.bookingDateTime).toLocaleString("vi-VN")}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </AdminDataState>
        </CardContent>
      </Card>
    </div>
  )
}
