import * as React from "react"
import { useForm } from "react-hook-form"
import { IconPencil, IconPlus, IconTrash } from "@tabler/icons-react"

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
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"
import { useAdminGenres } from "@/hooks/use-admin-data"
import { useAdminGenresMutations } from "@/hooks/use-admin-mutations"
import type {
  CreateGenreCommand,
  GenreSummaryDto,
  UpdateGenreCommand,
} from "@/lib/api-admin"

export default function AdminGenresPage() {
  const {
    data: genres,
    isLoading,
    isError,
    error,
    refetch,
  } = useAdminGenres()
  const { createGenre, updateGenre, deleteGenre } = useAdminGenresMutations()

  const [isCreateOpen, setCreateOpen] = React.useState(false)
  const [editingGenre, setEditingGenre] = React.useState<GenreSummaryDto | null>(
    null
  )

  const createForm = useForm<CreateGenreCommand>({
    defaultValues: { name: "" },
  })
  const updateForm = useForm<UpdateGenreCommand>({
    defaultValues: { id: 0, name: "" },
  })

  React.useEffect(() => {
    if (editingGenre) {
      updateForm.reset({ id: editingGenre.id, name: editingGenre.name })
    }
  }, [editingGenre, updateForm])

  const closeCreateDialog = () => {
    setCreateOpen(false)
    createForm.reset()
  }

  const closeEditDialog = () => {
    setEditingGenre(null)
    updateForm.reset()
  }

  const handleCreate = (values: CreateGenreCommand) =>
    createGenre.mutate(values, { onSuccess: closeCreateDialog })

  const handleUpdate = (values: UpdateGenreCommand) =>
    updateGenre.mutate(values, { onSuccess: closeEditDialog })

  const handleDelete = (genre: GenreSummaryDto) => {
    if (!window.confirm(`Xóa thể loại "${genre.name}"?`)) return
    deleteGenre.mutate(genre.id)
  }

  return (
    <div className="px-4 lg:px-6">
      <div className="mb-6 flex items-center justify-between">
        <div>
          <h2 className="text-2xl font-bold tracking-tight">Thể Loại Phim</h2>
          <p className="text-muted-foreground">
            Quản lý danh sách thể loại để sử dụng cho phim
          </p>
        </div>
        <Dialog open={isCreateOpen} onOpenChange={setCreateOpen}>
          <DialogTrigger asChild>
            <Button>
              <IconPlus className="mr-2 h-4 w-4" />
              Thêm Thể Loại
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>Thêm thể loại mới</DialogTitle>
              <DialogDescription>
                Đặt tên ngắn gọn và dễ nhớ cho thể loại.
              </DialogDescription>
            </DialogHeader>
            <form
              className="grid gap-4"
              onSubmit={createForm.handleSubmit(handleCreate)}
            >
              <div className="grid gap-2">
                <Label htmlFor="genre-name">Tên thể loại</Label>
                <Input
                  id="genre-name"
                  placeholder="Drama, Romance..."
                  {...createForm.register("name", { required: true })}
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
                <Button type="submit" disabled={createGenre.isPending}>
                  {createGenre.isPending ? "Đang lưu..." : "Tạo thể loại"}
                </Button>
              </DialogFooter>
            </form>
          </DialogContent>
        </Dialog>
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
                  <TableHead className="w-[140px] text-right">Hành động</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {genres?.map((genre) => (
                  <TableRow key={genre.id}>
                    <TableCell className="font-medium">{genre.id}</TableCell>
                    <TableCell>{genre.name}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex items-center justify-end gap-2">
                        <Button
                          variant="outline"
                          size="icon"
                          onClick={() => setEditingGenre(genre)}
                        >
                          <IconPencil className="size-4" />
                          <span className="sr-only">Chỉnh sửa</span>
                        </Button>
                        <Button
                          variant="ghost"
                          size="icon"
                          className="text-destructive hover:text-destructive"
                          onClick={() => handleDelete(genre)}
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
        open={Boolean(editingGenre)}
        onOpenChange={(value) => !value && closeEditDialog()}
      >
        <DialogContent>
          <DialogHeader>
            <DialogTitle>Cập nhật thể loại</DialogTitle>
            <DialogDescription>
              Chỉnh sửa tên thể loại để đồng bộ toàn hệ thống.
            </DialogDescription>
          </DialogHeader>
          <form
            className="grid gap-4"
            onSubmit={updateForm.handleSubmit(handleUpdate)}
          >
            <Input type="hidden" {...updateForm.register("id", { valueAsNumber: true })} />
            <div className="grid gap-2">
              <Label htmlFor="edit-genre-name">Tên thể loại</Label>
              <Input
                id="edit-genre-name"
                {...updateForm.register("name", { required: true })}
              />
            </div>
            <DialogFooter>
              <Button type="button" variant="outline" onClick={closeEditDialog}>
                Hủy
              </Button>
              <Button type="submit" disabled={updateGenre.isPending}>
                {updateGenre.isPending ? "Đang lưu..." : "Cập nhật"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </div>
  )
}