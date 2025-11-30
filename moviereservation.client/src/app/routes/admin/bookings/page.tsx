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
import { useAdminBookings } from "@/hooks/use-admin-data"
import { useAdminBookingsMutations } from "@/hooks/use-admin-mutations"
import type {
  BookingDto,
  BookingStatus,
  CreateBookingCommand,
  UpdateBookingCommand,
} from "@/lib/api-admin"

const currencyFormatter = new Intl.NumberFormat("vi-VN", {
  style: "currency",
  currency: "VND",
  maximumFractionDigits: 0,
})

const bookingStatuses: BookingStatus[] = ["Confirmed", "Reserved", "Cancelled"]

export default function AdminBookingsPage() {
  const {
    data: bookings,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminBookings()
  const { createBooking, updateBooking, deleteBooking } =
    useAdminBookingsMutations()

  const [isCreateOpen, setCreateOpen] = React.useState(false)
  const [editingBooking, setEditingBooking] = React.useState<BookingDto | null>(
    null
  )

  const createForm = useForm<CreateBookingCommand>({
    defaultValues: {
      userId: "",
      showId: undefined,
      seatRow: "A",
      seatNumber: 1,
      price: 70000,
      status: "Confirmed",
      bookingDateTime: new Date().toISOString().slice(0, 16),
    },
  })
  const updateForm = useForm<UpdateBookingCommand>({
    defaultValues: {
      id: 0,
      seatRow: "",
      seatNumber: undefined,
      status: "Confirmed",
    },
  })

  React.useEffect(() => {
    if (editingBooking) {
      updateForm.reset({
        id: editingBooking.id,
        seatRow: editingBooking.seatRow,
        seatNumber: editingBooking.seatNumber,
        status: editingBooking.status,
      })
    }
  }, [editingBooking, updateForm])

  const closeCreateDialog = () => {
    setCreateOpen(false)
    createForm.reset()
  }

  const closeEditDialog = () => {
    setEditingBooking(null)
    updateForm.reset()
  }

  const handleCreate = (values: CreateBookingCommand) => {
    createBooking.mutate(
      {
        ...values,
        showId: Number(values.showId),
        seatNumber: Number(values.seatNumber),
        price: Number(values.price),
      },
      { onSuccess: closeCreateDialog }
    )
  }

  const handleUpdate = (values: UpdateBookingCommand) => {
    updateBooking.mutate(
      {
        ...values,
        seatNumber: values.seatNumber ? Number(values.seatNumber) : undefined,
      },
      { onSuccess: closeEditDialog }
    )
  }

  const handleDelete = (booking: BookingDto) => {
    if (!window.confirm(`Xóa đơn đặt vé #${booking.id}?`)) return
    deleteBooking.mutate(booking.id)
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Quản Lý Đặt Vé</h2>
          <p className="text-muted-foreground">
            Theo dõi trạng thái ghế và doanh thu theo từng đơn đặt
          </p>
        </div>
        <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
          <DialogTrigger asChild>
            <Button>
              <IconPlus className="mr-2 h-4 w-4" />
              Thêm Đơn Đặt Vé
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Tạo đơn đặt vé</DialogTitle>
              <DialogDescription>
                Nhập chính xác mã người dùng và suất chiếu trước khi lưu.
              </DialogDescription>
            </DialogHeader>
            <form
              className="grid gap-4"
              onSubmit={createForm.handleSubmit(handleCreate)}
            >
              <div className="grid gap-2">
                <Label htmlFor="userId">User ID</Label>
                <Input
                  id="userId"
                  placeholder="GUID người dùng"
                  {...createForm.register("userId", { required: true })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="showId">Mã suất chiếu</Label>
                <Input
                  id="showId"
                  type="number"
                  min={1}
                  {...createForm.register("showId", { valueAsNumber: true })}
                />
              </div>
              <div className="grid grid-cols-2 gap-4">
                <div className="grid gap-2">
                  <Label htmlFor="seatRow">Hàng ghế</Label>
                  <Input
                    id="seatRow"
                    maxLength={2}
                    {...createForm.register("seatRow", { required: true })}
                  />
                </div>
                <div className="grid gap-2">
                  <Label htmlFor="seatNumber">Số ghế</Label>
                  <Input
                    id="seatNumber"
                    type="number"
                    min={1}
                    {...createForm.register("seatNumber", { valueAsNumber: true })}
                  />
                </div>
              </div>
              <div className="grid gap-2">
                <Label htmlFor="price">Giá vé</Label>
                <Input
                  id="price"
                  type="number"
                  min={0}
                  step="1000"
                  {...createForm.register("price", { valueAsNumber: true })}
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
                        field.onChange(value as BookingStatus)
                      }
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Chọn trạng thái" />
                      </SelectTrigger>
                      <SelectContent>
                        {bookingStatuses.map((status) => (
                          <SelectItem key={status} value={String(status)}>
                            {status}
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
                <Button type="submit" disabled={createBooking.isPending}>
                  {createBooking.isPending ? "Đang lưu..." : "Tạo đơn"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
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
                  <TableHead className="w-[150px] text-right">Hành động</TableHead>
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
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="outline"
                          size="icon"
                          onClick={() => setEditingBooking(booking)}
                        >
                          <IconPencil className="size-4" />
                          <span className="sr-only">Chỉnh sửa</span>
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="text-destructive hover:text-destructive"
                          onClick={() => handleDelete(booking)}
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
        open={Boolean(editingBooking)}
        onOpenChange={(value) => !value && closeEditDialog()}
      >
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Cập nhật đơn đặt vé</DialogTitle>
            <DialogDescription>
              Bạn có thể chỉnh ghế hoặc trạng thái của đơn đặt vé.
            </DialogDescription>
          </DialogHeader>
          <form
            className="grid gap-4"
            onSubmit={updateForm.handleSubmit(handleUpdate)}
          >
            <Input type="hidden" {...updateForm.register("id", { valueAsNumber: true })} />
            <div className="grid grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="edit-seat-row">Hàng ghế</Label>
                <Input
                  id="edit-seat-row"
                  maxLength={2}
                  {...updateForm.register("seatRow")}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="edit-seat-number">Số ghế</Label>
                <Input
                  id="edit-seat-number"
                  type="number"
                  min={1}
                  {...updateForm.register("seatNumber", { valueAsNumber: true })}
                />
              </div>
            </div>
            <div className="grid gap-2">
              <Label>Trạng thái</Label>
              <Controller
                control={updateForm.control}
                name="status"
                render={({ field }) => (
                  <Select
                    value={field.value?.toString()}
                    onValueChange={(value) =>
                      field.onChange(value as BookingStatus)
                    }
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Chọn trạng thái" />
                    </SelectTrigger>
                    <SelectContent>
                      {bookingStatuses.map((status) => (
                        <SelectItem key={status} value={String(status)}>
                          {status}
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
              <Button type="submit" disabled={updateBooking.isPending}>
                {updateBooking.isPending ? "Đang lưu..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}
