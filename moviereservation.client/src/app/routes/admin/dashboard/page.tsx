import { useMemo } from "react"

import { AdminDataState } from "@/components/admin-data-state"
import { AdminSectionCards } from "@/components/admin-section-cards"
import { ChartAreaInteractive } from "@/components/chart-area-interactive"
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
import {
  useAdminBookings,
  useAdminMovies,
  useAdminPayments,
  useAdminShows,
} from "@/hooks/use-admin-data"

const currencyFormatter = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0,
})

export default function AdminDashboardPage() {
  const moviesQuery = useAdminMovies()
  const bookingsQuery = useAdminBookings()
  const paymentsQuery = useAdminPayments()
  const showsQuery = useAdminShows()

  const revenue = useMemo(
    () => paymentsQuery.data?.reduce((sum, payment) => sum + (payment.amount ?? 0), 0) ?? 0,
    [paymentsQuery.data]
  )
  const latestBookings = useMemo(
    () => bookingsQuery.data?.slice(0, 5) ?? [],
    [bookingsQuery.data]
  )
  const summaryLoading =
    moviesQuery.isLoading ||
    bookingsQuery.isLoading ||
    paymentsQuery.isLoading ||
    showsQuery.isLoading

  return (
    <div className="space-y-6">
      <AdminSectionCards
        stats={{
          revenue,
          tickets: bookingsQuery.data?.length,
          movies: moviesQuery.data?.length,
          shows: showsQuery.data?.length,
        }}
        isLoading={summaryLoading}
      />
      <div className="px-4 lg:px-6">
        <ChartAreaInteractive />
      </div>
      <Card className="mx-4 lg:mx-6">
        <CardHeader>
          <CardTitle>Đơn Đặt Vé Gần Đây</CardTitle>
          <CardDescription>
            Theo dõi những đơn đặt mới nhất để phát hiện sự cố nhanh chóng
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={bookingsQuery.isLoading}
            isError={bookingsQuery.isError}
            error={bookingsQuery.error}
            onRetry={bookingsQuery.refetch}
            dataLength={latestBookings.length}
            emptyMessage="Chưa có đơn đặt vé nào."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Mã đơn</TableHead>
                  <TableHead>User</TableHead>
                  <TableHead>Suất chiếu</TableHead>
                  <TableHead>Ghế</TableHead>
                  <TableHead>Số tiền</TableHead>
                  <TableHead>Thời gian</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {latestBookings.map((booking) => (
                  <TableRow key={booking.id}>
                    <TableCell>#{booking.id}</TableCell>
                    <TableCell className="font-medium">
                      {booking.userId}
                    </TableCell>
                    <TableCell>#{booking.showId}</TableCell>
                    <TableCell>
                      {booking.seatRow}
                      {booking.seatNumber}
                    </TableCell>
                    <TableCell>
                      {currencyFormatter.format(booking.price ?? 0)}
                    </TableCell>
                    <TableCell>
                      {new Date(booking.bookingDateTime).toLocaleString(
                        "vi-VN"
                      )}
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
