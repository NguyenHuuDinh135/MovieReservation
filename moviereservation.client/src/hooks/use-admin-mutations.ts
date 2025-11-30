import { useMutation, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"

import apiAdmin, {
  type CreateBookingCommand,
  type UpdateBookingCommand,
  type CreateGenreCommand,
  type UpdateGenreCommand,
  type CreateMovieCommand,
  type UpdateMovieCommand,
  type CreateShowCommand,
  type UpdateShowCommand,
  type CreateTheaterCommand,
  type UpdateTheaterCommand,
} from "@/lib/api-admin"

type ToastCopy = {
  success: string
  error: string
}

const getErrorMessage = (error: unknown) => {
  if (
    error &&
    typeof error === "object" &&
    "response" in error &&
    (error as any)?.response
  ) {
    const response = (error as any).response
    return (
      response?.data?.message ||
      response?.data?.error ||
      response?.statusText ||
      "Đã có lỗi xảy ra."
    )
  }

  if (error instanceof Error) {
    return error.message
  }

  return "Đã có lỗi xảy ra."
}

const buildMutationHandlers = <TPayload,>(
  queryClient: ReturnType<typeof useQueryClient>,
  mutationFn: (payload: TPayload) => Promise<unknown>,
  invalidateKey: (string | number)[],
  copy: ToastCopy
) =>
  useMutation({
    mutationFn,
    onSuccess: () => {
      toast.success(copy.success)
      queryClient.invalidateQueries({ queryKey: invalidateKey })
    },
    onError: (error) => {
      toast.error(copy.error, {
        description: getErrorMessage(error),
      })
    },
  })

export const useAdminMoviesMutations = () => {
  const queryClient = useQueryClient()

  const createMovie = buildMutationHandlers<CreateMovieCommand>(
    queryClient,
    (payload) => apiAdmin.movies.create(payload),
    ["admin", "movies"],
    { success: "Đã tạo phim mới", error: "Không thể tạo phim" }
  )

  const updateMovie = buildMutationHandlers<UpdateMovieCommand>(
    queryClient,
    (payload) => apiAdmin.movies.update(payload),
    ["admin", "movies"],
    { success: "Đã cập nhật trạng thái phim", error: "Không thể cập nhật phim" }
  )

  const deleteMovie = buildMutationHandlers<number>(
    queryClient,
    (id) => apiAdmin.movies.delete(id),
    ["admin", "movies"],
    { success: "Đã xóa phim", error: "Không thể xóa phim" }
  )

  return { createMovie, updateMovie, deleteMovie }
}

export const useAdminShowsMutations = () => {
  const queryClient = useQueryClient()

  const createShow = buildMutationHandlers<CreateShowCommand>(
    queryClient,
    (payload) => apiAdmin.shows.create(payload),
    ["admin", "shows"],
    { success: "Đã tạo suất chiếu", error: "Không thể tạo suất chiếu" }
  )

  const updateShow = buildMutationHandlers<UpdateShowCommand>(
    queryClient,
    (payload) => apiAdmin.shows.update(payload),
    ["admin", "shows"],
    { success: "Đã cập nhật suất chiếu", error: "Không thể cập nhật suất chiếu" }
  )

  const deleteShow = buildMutationHandlers<number>(
    queryClient,
    (id) => apiAdmin.shows.delete(id),
    ["admin", "shows"],
    { success: "Đã xóa suất chiếu", error: "Không thể xóa suất chiếu" }
  )

  return { createShow, updateShow, deleteShow }
}

export const useAdminTheatersMutations = () => {
  const queryClient = useQueryClient()

  const createTheater = buildMutationHandlers<CreateTheaterCommand>(
    queryClient,
    (payload) => apiAdmin.theaters.create(payload),
    ["admin", "theaters"],
    { success: "Đã tạo rạp chiếu", error: "Không thể tạo rạp chiếu" }
  )

  const updateTheater = buildMutationHandlers<UpdateTheaterCommand>(
    queryClient,
    (payload) => apiAdmin.theaters.update(payload),
    ["admin", "theaters"],
    { success: "Đã cập nhật rạp chiếu", error: "Không thể cập nhật rạp chiếu" }
  )

  const deleteTheater = buildMutationHandlers<number>(
    queryClient,
    (id) => apiAdmin.theaters.delete(id),
    ["admin", "theaters"],
    { success: "Đã xóa rạp chiếu", error: "Không thể xóa rạp chiếu" }
  )

  return { createTheater, updateTheater, deleteTheater }
}

export const useAdminGenresMutations = () => {
  const queryClient = useQueryClient()

  const createGenre = buildMutationHandlers<CreateGenreCommand>(
    queryClient,
    (payload) => apiAdmin.genres.create(payload),
    ["admin", "genres"],
    { success: "Đã tạo thể loại", error: "Không thể tạo thể loại" }
  )

  const updateGenre = buildMutationHandlers<UpdateGenreCommand>(
    queryClient,
    (payload) => apiAdmin.genres.update(payload),
    ["admin", "genres"],
    { success: "Đã cập nhật thể loại", error: "Không thể cập nhật thể loại" }
  )

  const deleteGenre = buildMutationHandlers<number>(
    queryClient,
    (id) => apiAdmin.genres.delete(id),
    ["admin", "genres"],
    { success: "Đã xóa thể loại", error: "Không thể xóa thể loại" }
  )

  return { createGenre, updateGenre, deleteGenre }
}

export const useAdminBookingsMutations = () => {
  const queryClient = useQueryClient()

  const createBooking = buildMutationHandlers<CreateBookingCommand>(
    queryClient,
    (payload) => apiAdmin.bookings.create(payload),
    ["admin", "bookings"],
    { success: "Đã tạo đơn đặt vé", error: "Không thể tạo đơn đặt vé" }
  )

  const updateBooking = buildMutationHandlers<UpdateBookingCommand>(
    queryClient,
    (payload) => apiAdmin.bookings.update(payload),
    ["admin", "bookings"],
    { success: "Đã cập nhật đơn đặt vé", error: "Không thể cập nhật đơn đặt vé" }
  )

  const deleteBooking = buildMutationHandlers<number>(
    queryClient,
    (id) => apiAdmin.bookings.delete(id),
    ["admin", "bookings"],
    { success: "Đã xóa đơn đặt vé", error: "Không thể xóa đơn đặt vé" }
  )

  return { createBooking, updateBooking, deleteBooking }
}

