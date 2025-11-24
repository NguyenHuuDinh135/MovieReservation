import { AdminDataState } from "@/components/admin-data-state"
import { Button } from "@/components/ui/button"
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
import { useAdminTheaters } from "@/hooks/use-admin-data"
import { IconPlus } from "@tabler/icons-react"

export default function AdminCinemasPage() {
  const {
    data: theaters,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminTheaters()

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Rạp Chiếu</h2>
          <p className="text-muted-foreground">
            Thông tin chi tiết về từng phòng chiếu
          </p>
        </div>
        <Button>
          <IconPlus className="mr-2 h-4 w-4" />
          Thêm Rạp Mới
        </Button>
      </div>
      <Card>
        <CardHeader>
          <CardTitle>Danh Sách Rạp Chiếu</CardTitle>
          <CardDescription>
            {theaters?.length ?? 0} rạp đang hoạt động trong hệ thống
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={isLoading}
            isError={isError}
            error={error}
            onRetry={refetch}
            dataLength={theaters?.length}
            emptyMessage="Chưa có rạp nào được cấu hình."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Tên rạp</TableHead>
                  <TableHead>Loại</TableHead>
                  <TableHead>Số hàng ghế</TableHead>
                  <TableHead>Ghế/hàng</TableHead>
                  <TableHead>Ghế bị thiếu</TableHead>
                  <TableHead>Ghế bị chặn</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {theaters?.map((theater) => (
                  <TableRow key={theater.name}>
                    <TableCell className="font-medium">{theater.name}</TableCell>
                    <TableCell>
                      <Badge variant="secondary" className="capitalize">
                        {theater.type?.toString()}
                      </Badge>
                    </TableCell>
                    <TableCell>{theater.numOfRows}</TableCell>
                    <TableCell>{theater.seatsPerRow}</TableCell>
                    <TableCell>
                      {theater.missing.length
                        ? theater.missing
                            .map((seat) => `${seat.seatRow}${seat.seatNumber}`)
                            .join(", ")
                        : "—"}
                    </TableCell>
                    <TableCell>
                      {theater.blocked.length
                        ? theater.blocked
                            .map((seat) => `${seat.seatRow}${seat.seatNumber}`)
                            .join(", ")
                        : "—"}
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
