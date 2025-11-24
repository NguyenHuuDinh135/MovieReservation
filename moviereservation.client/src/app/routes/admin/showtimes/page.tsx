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
import { useAdminShows } from "@/hooks/use-admin-data"
import { IconPlus } from "@tabler/icons-react"

const formatDate = (value: string) => {
  const date = new Date(value)
  if (Number.isNaN(date.getTime())) return value
  return date.toLocaleDateString("vi-VN")
}

const formatTime = (value: string) => value?.slice(0, 5) ?? value

export default function AdminShowtimesPage() {
  const {
    data: shows,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminShows()

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Suất Chiếu</h2>
          <p className="text-muted-foreground">
            Thống kê lịch chiếu và trạng thái đặt vé
          </p>
        </div>
        <Button>
          <IconPlus className="mr-2 h-4 w-4" />
          Thêm Suất Chiếu
        </Button>
      </div>
      <Card>
        <CardHeader>
          <CardTitle>Danh Sách Suất Chiếu</CardTitle>
          <CardDescription>
            {shows?.length ?? 0} suất chiếu được đồng bộ từ API
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={isLoading}
            isError={isError}
            error={error}
            dataLength={shows?.length}
            onRetry={refetch}
            emptyMessage="Chưa có suất chiếu nào."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Ngày chiếu</TableHead>
                  <TableHead>Khung giờ</TableHead>
                  <TableHead>Phim</TableHead>
                  <TableHead>Rạp</TableHead>
                  <TableHead>Trạng thái</TableHead>
                  <TableHead>Định dạng</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {shows?.map((show) => (
                  <TableRow key={show.id}>
                    <TableCell>{formatDate(show.date)}</TableCell>
                    <TableCell>
                      {formatTime(show.startTime)} - {formatTime(show.endTime)}
                    </TableCell>
                    <TableCell>#{show.movieId}</TableCell>
                    <TableCell>#{show.theaterId}</TableCell>
                    <TableCell>
                      <Badge variant="secondary" className="capitalize">
                        {show.status?.toString() || "N/A"}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      <Badge variant="outline" className="uppercase">
                        {show.type?.toString()}
                      </Badge>
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
