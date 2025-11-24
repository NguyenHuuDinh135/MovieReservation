import { AdminDataState } from "@/components/admin-data-state"
import { Button } from "@/components/ui/button"
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
import { useAdminGenres } from "@/hooks/use-admin-data"
import { IconPlus } from "@tabler/icons-react"

export default function AdminGenresPage() {
  const {
    data: genres,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminGenres()

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Thể Loại Phim</h2>
          <p className="text-muted-foreground">
            Quản lý danh sách thể loại để sử dụng cho phim
          </p>
        </div>
        <Button>
          <IconPlus className="mr-2 h-4 w-4" />
          Thêm Thể Loại
        </Button>
      </div>
      <Card>
        <CardHeader>
          <CardTitle>Danh Sách Thể Loại</CardTitle>
          <CardDescription>
            Hiện có {genres?.length ?? 0} thể loại trong hệ thống
          </CardDescription>
        </CardHeader>
        <CardContent>
          <AdminDataState
            isLoading={isLoading}
            isError={isError}
            error={error}
            onRetry={refetch}
            dataLength={genres?.length}
            emptyMessage="Chưa có thể loại nào."
          >
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>ID</TableHead>
                  <TableHead>Tên thể loại</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {genres?.map((genre) => (
                  <TableRow key={genre.id}>
                    <TableCell className="font-medium">{genre.id}</TableCell>
                    <TableCell>{genre.name}</TableCell>
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

