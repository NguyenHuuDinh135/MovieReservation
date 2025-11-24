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
import { useAdminMovies } from "@/hooks/use-admin-data"
import { IconPlus } from "@tabler/icons-react"

export default function AdminMoviesPage() {
  const {
    data: movies,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminMovies()

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Phim</h2>
          <p className="text-muted-foreground">
            Danh sách phim từ hệ thống backend
          </p>
        </div>
        <Button>
          <IconPlus className="mr-2 h-4 w-4" />
          Thêm Phim Mới
        </Button>
      </div>
      <Card>
        <CardHeader>
          <CardTitle>Danh Sách Phim</CardTitle>
          <CardDescription>
            Tổng cộng {movies?.length ?? 0} phim được đồng bộ từ API
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={isLoading}
            isError={isError}
            error={error}
            dataLength={movies?.length}
            onRetry={refetch}
            emptyMessage="Chưa có phim nào được tạo."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Tiêu đề</TableHead>
                  <TableHead>Năm</TableHead>
                  <TableHead>Loại phim</TableHead>
                  <TableHead>Điểm</TableHead>
                  <TableHead>Thể loại</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {movies?.map((movie) => (
                  <TableRow key={movie.id}>
                    <TableCell>
                      <div className="max-w-xs">
                        <p className="font-medium">{movie.title}</p>
                        <p className="line-clamp-2 text-xs text-muted-foreground">
                          {movie.summary}
                        </p>
                      </div>
                    </TableCell>
                    <TableCell>{movie.year}</TableCell>
                    <TableCell>
                      <Badge variant="outline" className="capitalize">
                        {movie.movieType?.toString() || "Unknown"}
                      </Badge>
                    </TableCell>
                    <TableCell>
                      {movie.rating ? movie.rating.toFixed(1) : "—"}
                    </TableCell>
                    <TableCell className="text-muted-foreground">
                      {movie.genres.length ? (
                        <div className="flex flex-wrap gap-1">
                          {movie.genres.map((genre) => (
                            <Badge key={genre.id} variant="secondary">
                              {genre.name}
                            </Badge>
                          ))}
                        </div>
                      ) : (
                        <span className="text-xs">Chưa có thể loại</span>
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
