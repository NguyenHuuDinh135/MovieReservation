import * as React from "react"
import { Controller, useForm } from "react-hook-form"
import { IconPencil, IconPlus, IconTrash } from "@tabler/icons-react"

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
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { useAdminMovies } from "@/hooks/use-admin-data"
import { useAdminMoviesMutations } from "@/hooks/use-admin-mutations"
import type {
  CreateMovieCommand,
  MovieDto,
  MovieType,
  UpdateMovieCommand,
} from "@/lib/api-admin"

const movieTypes: MovieType[] = ["ComingSoon", "NowShowing", "Removed"]

export default function AdminMoviesPage() {
  const {
    data: movies,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminMovies()

  const { createMovie, updateMovie, deleteMovie } = useAdminMoviesMutations()
  const [isCreateOpen, setCreateOpen] = React.useState(false)
  const [editingMovie, setEditingMovie] = React.useState<MovieDto | null>(null)

  const createForm = useForm<CreateMovieCommand>({
    defaultValues: {
      title: "",
      year: new Date().getFullYear(),
      summary: "",
      trailerUrl: "",
      posterUrl: "",
      movieType: "ComingSoon",
    },
  })

  const updateForm = useForm<UpdateMovieCommand>({
    defaultValues: { id: 0, movieType: "NowShowing" },
  })

  React.useEffect(() => {
    if (editingMovie) {
      updateForm.reset({
        id: editingMovie.id,
        movieType: (editingMovie.movieType?.toString() ||
          "NowShowing") as MovieType,
      })
    }
  }, [editingMovie, updateForm])

  const closeCreateDialog = () => {
    setCreateOpen(false)
    createForm.reset()
  }

  const closeEditDialog = () => {
    setEditingMovie(null)
    updateForm.reset()
  }

  const handleDeleteMovie = (movie: MovieDto) => {
    if (!window.confirm(`Xác nhận xóa phim "${movie.title}"?`)) return
    deleteMovie.mutate(movie.id)
  }

  const onSubmitCreate = (values: CreateMovieCommand) => {
    createMovie.mutate(values, {
      onSuccess: closeCreateDialog,
    })
  }

  const onSubmitUpdate = (values: UpdateMovieCommand) => {
    updateMovie.mutate(values, {
      onSuccess: closeEditDialog,
    })
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Phim</h2>
          <p className="text-muted-foreground">
            Danh sách phim từ hệ thống backend
          </p>
        </div>
        <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
          <DialogTrigger asChild>
            <Button>
              <IconPlus className="mr-2 h-4 w-4" />
              Thêm Phim Mới
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Thêm phim mới</DialogTitle>
              <DialogDescription>
                Điền đầy đủ thông tin để tạo phim trong hệ thống.
              </DialogDescription>
            </DialogHeader>
            <form
              className="space-y-4"
              onSubmit={createForm.handleSubmit(onSubmitCreate)}
            >
              <div className="grid gap-2">
                <Label htmlFor="title">Tiêu đề</Label>
                <Input
                  id="title"
                  placeholder="Ví dụ: Gọi Tên Em Đi"
                  {...createForm.register("title", { required: true })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="year">Năm phát hành</Label>
                <Input
                  id="year"
                  type="number"
                  min={1900}
                  max={2100}
                  {...createForm.register("year", { valueAsNumber: true })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="summary">Tóm tắt</Label>
                <textarea
                  id="summary"
                  className="border-input focus-visible:ring-ring min-h-[120px] w-full rounded-md border bg-background px-3 py-2 text-sm outline-none transition focus-visible:ring-2 focus-visible:ring-offset-2"
                  {...createForm.register("summary", { required: true })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="posterUrl">Poster URL</Label>
                <Input
                  id="posterUrl"
                  placeholder="https://..."
                  {...createForm.register("posterUrl")}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="trailerUrl">Trailer URL</Label>
                <Input
                  id="trailerUrl"
                  placeholder="https://youtube.com/..."
                  {...createForm.register("trailerUrl")}
                />
              </div>
              <div className="grid gap-2">
                <Label>Trạng thái</Label>
                <Controller
                  control={createForm.control}
                  name="movieType"
                  render={({ field }) => (
                    <Select
                      value={field.value?.toString()}
                      onValueChange={(value) =>
                        field.onChange(value as MovieType)
                      }
                    >
                      <SelectTrigger className="w-full">
                        <SelectValue placeholder="Chọn trạng thái" />
                      </SelectTrigger>
                      <SelectContent>
                        {movieTypes.map((type) => (
                          <SelectItem key={type} value={String(type)}>
                            {type}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
              </div>
              <DialogFooter>
                <Button
                  type="button"
                  variant="outline"
                  onClick={closeCreateDialog}
                >
                  Hủy
                </Button>
                <Button type="submit" disabled={createMovie.isPending}>
                  {createMovie.isPending ? "Đang lưu..." : "Tạo mới"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
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
                  <TableHead className="w-[160px] text-right">Hành động</TableHead>
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
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="outline"
                          size="icon"
                          onClick={() => setEditingMovie(movie)}
                        >
                          <IconPencil className="size-4" />
                          <span className="sr-only">Chỉnh sửa</span>
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="text-destructive hover:text-destructive"
                          onClick={() => handleDeleteMovie(movie)}
                        >
                          <IconTrash className="size-4" />
                          <span className="sr-only">Xóa</span>
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </AdminDataState>
        </CardContent>
      </Card>

      <Dialog open={Boolean(editingMovie)} onOpenChange={(value) => !value && closeEditDialog()}>
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Cập nhật phim</DialogTitle>
            <DialogDescription>
              Điều chỉnh trạng thái phim để quản lý lịch chiếu.
            </DialogDescription>
          </DialogHeader>
          <form
            className="space-y-4"
            onSubmit={updateForm.handleSubmit(onSubmitUpdate)}
          >
            <Input type="hidden" {...updateForm.register("id", { valueAsNumber: true })} />
            <div className="grid gap-2">
              <Label>Trạng thái</Label>
              <Controller
                control={updateForm.control}
                name="movieType"
                render={({ field }) => (
                  <Select
                    value={field.value?.toString()}
                    onValueChange={(value) =>
                      field.onChange(value as MovieType)
                    }
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Chọn trạng thái" />
                    </SelectTrigger>
                    <SelectContent>
                      {movieTypes.map((type) => (
                        <SelectItem key={type} value={String(type)}>
                          {type}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                )}
              />
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={closeEditDialog}>
                Hủy
              </Button>
              <Button type="submit" disabled={updateMovie.isPending}>
                {updateMovie.isPending ? "Đang lưu..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}
