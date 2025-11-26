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
import {
  useAdminMovies,
  useAdminShows,
  useAdminTheaters,
} from "@/hooks/use-admin-data"
import { useAdminShowsMutations } from "@/hooks/use-admin-mutations"
import type {
  CreateShowCommand,
  ShowDto,
  ShowStatus,
  ShowType,
  UpdateShowCommand,
} from "@/lib/api-admin"

const statusOptions: ShowStatus[] = ["Free", "AlmostFull", "Full"]
const typeOptions: ShowType[] = ["TwoD", "ThreeD"]

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
  const { data: movies } = useAdminMovies()
  const { data: theaters } = useAdminTheaters()
  const { createShow, updateShow, deleteShow } = useAdminShowsMutations()

  const [isCreateOpen, setCreateOpen] = React.useState(false)
  const [editingShow, setEditingShow] = React.useState<ShowDto | null>(null)

  const createForm = useForm<CreateShowCommand>({
    defaultValues: {
      date: new Date().toISOString().slice(0, 10),
      startTime: "09:00",
      endTime: "11:00",
      movieId: undefined,
      theaterId: undefined,
      status: "Free",
      type: "TwoD",
    },
  })

  const updateForm = useForm<UpdateShowCommand>({
    defaultValues: {
      id: 0,
      theaterId: 0,
      date: new Date().toISOString().slice(0, 10),
      startTime: "09:00",
      endTime: "11:00",
    },
  })

  React.useEffect(() => {
    if (editingShow) {
      updateForm.reset({
        id: editingShow.id,
        theaterId: editingShow.theaterId,
        date: editingShow.date?.slice(0, 10),
        startTime: editingShow.startTime?.slice(0, 5),
        endTime: editingShow.endTime?.slice(0, 5),
      })
    }
  }, [editingShow, updateForm])

  const closeCreateDialog = () => {
    setCreateOpen(false)
    createForm.reset()
  }

  const closeEditDialog = () => {
    setEditingShow(null)
    updateForm.reset()
  }

  const onSubmitCreate = (values: CreateShowCommand) => {
    createShow.mutate(
      {
        ...values,
        movieId: Number(values.movieId),
        theaterId: Number(values.theaterId),
      },
      { onSuccess: closeCreateDialog }
    )
  }

  const onSubmitUpdate = (values: UpdateShowCommand) => {
    updateShow.mutate(
      {
        ...values,
        theaterId: Number(values.theaterId),
      },
      { onSuccess: closeEditDialog }
    )
  }

  const handleDelete = (show: ShowDto) => {
    if (!window.confirm("Xác nhận xóa suất chiếu này?")) return
    deleteShow.mutate(show.id)
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Suất Chiếu</h2>
          <p className="text-muted-foreground">
            Thống kê lịch chiếu và trạng thái đặt vé
          </p>
        </div>
        <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
          <DialogTrigger asChild>
            <Button>
              <IconPlus className="mr-2 h-4 w-4" />
              Thêm Suất Chiếu
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Thêm suất chiếu</DialogTitle>
              <DialogDescription>
                Chọn phim, rạp và thời gian để tạo suất chiếu mới.
              </DialogDescription>
            </DialogHeader>
            <form
              className="grid gap-4"
              onSubmit={createForm.handleSubmit(onSubmitCreate)}
            >
              <div className="grid gap-2">
                <Label htmlFor="date">Ngày chiếu</Label>
                <Input
                  id="date"
                  type="date"
                  {...createForm.register("date", { required: true })}
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="startTime">Giờ bắt đầu</Label>
                  <Input
                    id="startTime"
                    type="time"
                    {...createForm.register("startTime", { required: true })}
                  />
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="endTime">Giờ kết thúc</Label>
                  <Input
                    id="endTime"
                    type="time"
                    {...createForm.register("endTime", { required: true })}
                  />
                </div>
              </div>
              <div className="grid gap-2">
                <Label>Phim</Label>
                <Controller
                  control={createForm.control}
                  name="movieId"
                  rules={{ required: true }}
                  render={({ field }) => (
                    <Select
                      value={field.value ? String(field.value) : undefined}
                      onValueChange={(value) => field.onChange(Number(value))}
                      disabled={!movies?.length}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn phim" />
                      </SelectTrigger>
                      <SelectContent>
                        {movies?.map((movie) => (
                          <SelectItem key={movie.id} value={String(movie.id)}>
                            #{movie.id} • {movie.title}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
              </div>
              <div className="grid gap-2">
                <Label>Rạp chiếu</Label>
                <Controller
                  control={createForm.control}
                  name="theaterId"
                  rules={{ required: true }}
                  render={({ field }) => (
                    <Select
                      value={field.value ? String(field.value) : undefined}
                      onValueChange={(value) => field.onChange(Number(value))}
                      disabled={!theaters?.length}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn rạp" />
                      </SelectTrigger>
                      <SelectContent>
                        {theaters?.map((theater) => (
                          <SelectItem key={theater.id} value={String(theater.id)}>
                            #{theater.id} • {theater.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
              </div>
              <div className="grid gap-2">
                <Label>Trạng thái</Label>
                <Controller
                  control={createForm.control}
                  name="status"
                  render={({ field }) => (
                    <Select
                      value={field.value?.toString()}
                      onValueChange={(value) =>
                        field.onChange(value as ShowStatus)
                      }
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn trạng thái" />
                      </SelectTrigger>
                      <SelectContent>
                        {statusOptions.map((status) => (
                          <SelectItem key={status} value={String(status)}>
                            {status}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  )}
                />
              </div>
              <div className="grid gap-2">
                <Label>Định dạng</Label>
                <Controller
                  control={createForm.control}
                  name="type"
                  render={({ field }) => (
                    <Select
                      value={field.value?.toString()}
                      onValueChange={(value) => field.onChange(value as ShowType)}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn định dạng" />
                      </SelectTrigger>
                      <SelectContent>
                        {typeOptions.map((type) => (
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
                <Button type="submit" disabled={createShow.isPending}>
                  {createShow.isPending ? "Đang lưu..." : "Tạo suất chiếu"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
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
                  <TableHead className="w-[150px] text-right">Hành động</TableHead>
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
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="outline"
                          size="icon"
                          onClick={() => setEditingShow(show)}
                        >
                          <IconPencil className="size-4" />
                          <span className="sr-only">Chỉnh sửa</span>
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="text-destructive hover:text-destructive"
                          onClick={() => handleDelete(show)}
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

      <Dialog
        open={Boolean(editingShow)}
        onOpenChange={(value) => !value && closeEditDialog()}
      >
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Cập nhật suất chiếu</DialogTitle>
            <DialogDescription>
              Điều chỉnh thời gian hoặc rạp chiếu cho suất chiếu này.
            </DialogDescription>
          </DialogHeader>
          <form
            className="grid gap-4"
            onSubmit={updateForm.handleSubmit(onSubmitUpdate)}
          >
            <Input type="hidden" {...updateForm.register("id", { valueAsNumber: true })} />
            <div className="grid gap-2">
              <Label htmlFor="edit-date">Ngày chiếu</Label>
              <Input
                id="edit-date"
                type="date"
                {...updateForm.register("date", { required: true })}
              />
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="edit-start">Giờ bắt đầu</Label>
                <Input
                  id="edit-start"
                  type="time"
                  {...updateForm.register("startTime", { required: true })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="edit-end">Giờ kết thúc</Label>
                <Input
                  id="edit-end"
                  type="time"
                  {...updateForm.register("endTime", { required: true })}
                />
              </div>
            </div>
            <div className="grid gap-2">
              <Label>Rạp chiếu</Label>
              <Controller
                control={updateForm.control}
                name="theaterId"
                render={({ field }) => (
                  <Select
                    value={field.value ? String(field.value) : undefined}
                    onValueChange={(value) => field.onChange(Number(value))}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Chọn rạp" />
                    </SelectTrigger>
                    <SelectContent>
                      {theaters?.map((theater) => (
                        <SelectItem key={theater.id} value={String(theater.id)}>
                          #{theater.id} • {theater.name}
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
              <Button type="submit" disabled={updateShow.isPending}>
                {updateShow.isPending ? "Đang lưu..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}
