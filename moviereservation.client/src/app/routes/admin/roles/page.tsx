import { useState } from "react"

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
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  useAdminMovieRoles,
  useAdminMoviesOptions,
} from "@/hooks/use-admin-data"

export default function AdminRolesPage() {
  const { data: movies } = useAdminMoviesOptions()
  const [selectedMovie, setSelectedMovie] = useState<number | undefined>()
  const {
    data: roleData,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminMovieRoles(selectedMovie)

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6">
        <h2 className="text-2xl font-bold tracking-tight">Quản Lý Diễn Viên</h2>
        <p className="text-muted-foreground">
          Chọn phim để xem danh sách diễn viên đã được gán trong backend
        </p>
      </div>
      <Card>
        <CardHeader className="flex flex-col gap-4 lg:flex-row lg:items-center lg:justify-between">
          <div>
            <CardTitle>Vai Diễn Theo Phim</CardTitle>
            <CardDescription>
              {roleData?.roles.length
                ? `${roleData.roles.length} diễn viên`
                : "Chọn phim để xem danh sách diễn viên"}
            </CardDescription>
          </div>
          <Select
            value={selectedMovie?.toString()}
            onValueChange={(value) => setSelectedMovie(Number(value))}
          >
            <SelectTrigger className="w-full lg:w-72">
              <SelectValue placeholder="Chọn phim" />
            </SelectTrigger>
            <SelectContent>
              {movies?.map((movie) => (
                <SelectItem key={movie.id} value={movie.id.toString()}>
                  {movie.title}
                </SelectItem>
              ))}
            </SelectContent>
          </Select>
        </CardHeader>
        <CardContent>
          {selectedMovie ? (
            <AdminDataState
              isLoading={isLoading}
              isError={isError}
              error={error}
              onRetry={refetch}
              dataLength={roleData?.roles.length}
              emptyMessage="Phim này chưa có diễn viên nào."
            >
              <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                {roleData?.roles.map((role) => (
                  <div
                    key={role.id}
                    className="rounded-lg border bg-card p-4 shadow-sm"
                  >
                    <div className="mb-1 flex items-center justify-between">
                      <p className="font-semibold">{role.fullName}</p>
                      <Badge variant="secondary">{role.age} tuổi</Badge>
                    </div>
                    <p className="text-sm text-muted-foreground">
                      ID: #{role.id}
                    </p>
                    {role.pictureUrl ? (
                      <img
                        src={role.pictureUrl}
                        alt={role.fullName}
                        className="mt-3 h-36 w-full rounded-md object-cover"
                      />
                    ) : null}
                  </div>
                ))}
              </div>
            </AdminDataState>
          ) : (
            <div className="py-10 text-center text-sm text-muted-foreground">
              Hãy chọn một phim để bắt đầu.
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  )
}

