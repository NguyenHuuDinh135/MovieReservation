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
import { useAdminTheaters } from "@/hooks/use-admin-data"
import { useAdminTheatersMutations } from "@/hooks/use-admin-mutations"
import type {
  CreateTheaterCommand,
  TheaterDto,
  TheaterType,
  UpdateTheaterCommand,
} from "@/lib/api-admin"

const theaterTypes: TheaterType[] = ["Normal", "Royal"]

export default function AdminCinemasPage() {
  const {
    data: theaters,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminTheaters()
  const { createTheater, updateTheater, deleteTheater } =
    useAdminTheatersMutations()

  const [isCreateOpen, setCreateOpen] = React.useState(false)
  const [editingTheater, setEditingTheater] = React.useState<TheaterDto | null>(
    null
  )

  const createForm = useForm<CreateTheaterCommand>({
    defaultValues: {
      name: "",
      numOfRows: 10,
      seatsPerRow: 12,
      type: "Normal",
    },
  })

  const updateForm = useForm<UpdateTheaterCommand>({
    defaultValues: {
      id: 0,
      theaterType: "Normal",
    },
  })

  React.useEffect(() => {
    if (editingTheater?.id) {
      updateForm.reset({
        id: editingTheater.id,
        theaterType: (editingTheater.type?.toString() || "Normal") as TheaterType,
      })
    }
  }, [editingTheater, updateForm])

  const closeCreateDialog = () => {
    setCreateOpen(false)
    createForm.reset()
  }

  const closeEditDialog = () => {
    setEditingTheater(null)
    updateForm.reset()
  }

  const handleCreate = (values: CreateTheaterCommand) => {
    createTheater.mutate(values, { onSuccess: closeCreateDialog })
  }

  const handleUpdate = (values: UpdateTheaterCommand) => {
    updateTheater.mutate(values, { onSuccess: closeEditDialog })
  }

  const handleDelete = (theater: TheaterDto) => {
    if (!theater.id) return
    if (!window.confirm(`Xóa rạp "${theater.name}"?`)) return
    deleteTheater.mutate(theater.id)
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Rạp Chiếu</h2>
          <p className="text-muted-foreground">
            Thông tin chi tiết về từng phòng chiếu
          </p>
        </div>
        <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
          <DialogTrigger asChild>
            <Button>
              <IconPlus className="mr-2 h-4 w-4" />
              Thêm Rạp Mới
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Thêm rạp chiếu</DialogTitle>
              <DialogDescription>
                Cung cấp thông tin cơ bản để tạo một phòng chiếu mới.
              </DialogDescription>
            </DialogHeader>
            <form
              className="grid gap-4"
              onSubmit={createForm.handleSubmit(handleCreate)}
            >
              <div className="grid gap-2">
                <Label htmlFor="name">Tên rạp</Label>
                <Input
                  id="name"
                  placeholder="Cinema Room 1"
                  {...createForm.register("name", { required: true })}
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="numOfRows">Số hàng ghế</Label>
                  <Input
                    id="numOfRows"
                    type="number"
                    min={1}
                    {...createForm.register("numOfRows", { valueAsNumber: true })}
                  />
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="seatsPerRow">Ghế mỗi hàng</Label>
                  <Input
                    id="seatsPerRow"
                    type="number"
                    min={1}
                    {...createForm.register("seatsPerRow", {
                      valueAsNumber: true,
                    })}
                  />
                </div>
              </div>
              <div className="grid gap-2">
                <Label>Loại rạp</Label>
                <Controller
                  control={createForm.control}
                  name="type"
                  render={({ field }) => (
                    <Select
                      value={field.value?.toString()}
                      onValueChange={(value) =>
                        field.onChange(value as TheaterType)
                      }
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn loại" />
                      </SelectTrigger>
                      <SelectContent>
                        {theaterTypes.map((type) => (
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
                <Button type="submit" disabled={createTheater.isPending}>
                  {createTheater.isPending ? "Đang lưu..." : "Tạo rạp"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
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
                  <TableHead className="w-[150px] text-right">Hành động</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {theaters?.map((theater) => (
                  <TableRow key={`${theater.id}-${theater.name}`}>
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
                    <TableCell className="text-right">
                      {theater.id ? (
                        <div className="flex items-center justify-end gap-2">
                          <Button
                            variant="outline"
                            size="icon"
                            onClick={() => setEditingTheater(theater)}
                          >
                            <IconPencil className="size-4" />
                            <span className="sr-only">Chỉnh sửa</span>
                          </Button>
                          <Button
                            variant="ghost"
                            size="icon"
                            className="text-destructive hover:text-destructive"
                            onClick={() => handleDelete(theater)}
                          >
                            <IconTrash className="size-4" />
                            <span className="sr-only">Xóa</span>
                          </Button>
                        </div>
                      ) : null}
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </AdminDataState>
        </CardContent>
      </Card>

      <Dialog
        open={Boolean(editingTheater)}
        onOpenChange={(value) => !value && closeEditDialog()}
      >
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Cập nhật loại rạp</DialogTitle>
            <DialogDescription>
              Chỉ số ghế giữ nguyên, bạn có thể đổi loại phòng chiếu.
            </DialogDescription>
          </DialogHeader>
          <form
            className="grid gap-4"
            onSubmit={updateForm.handleSubmit(handleUpdate)}
          >
            <Input type="hidden" {...updateForm.register("id", { valueAsNumber: true })} />
            <div className="grid gap-2">
              <Label>Loại rạp</Label>
              <Controller
                control={updateForm.control}
                name="theaterType"
                render={({ field }) => (
                  <Select
                    value={field.value?.toString()}
                    onValueChange={(value) =>
                      field.onChange(value as TheaterType)
                    }
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Chọn loại" />
                    </SelectTrigger>
                    <SelectContent>
                      {theaterTypes.map((type) => (
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
              <Button type="submit" disabled={updateTheater.isPending}>
                {updateTheater.isPending ? "Đang lưu..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}
