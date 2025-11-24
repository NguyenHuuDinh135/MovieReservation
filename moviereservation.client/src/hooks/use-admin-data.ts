import { useQuery } from "@tanstack/react-query"

import apiAdmin, {
  type BookingDto,
  type BookingsByUserDto,
  type GenreByIdDto,
  type GenreSummaryDto,
  type MovieDto,
  type PaymentDetailBody,
  type PaymentListItem,
  type RolesForMovieDto,
  type ShowDto,
  type TheaterDto,
} from "@/lib/api-admin"

const extractData = async <T>(promise: Promise<{ data: T }>) => {
  const res = await promise
  return res.data
}

const fetchMovies = () => extractData<MovieDto[]>(apiAdmin.movies.getAll())
const fetchShows = () => extractData<ShowDto[]>(apiAdmin.shows.getAll())
const fetchTheaters = () =>
  extractData<TheaterDto[]>(apiAdmin.theaters.getAll())
const fetchBookings = () =>
  extractData<BookingDto[]>(apiAdmin.bookings.getAll())
const fetchGenres = () =>
  extractData<GenreSummaryDto[]>(apiAdmin.genres.getAll())
const fetchPayments = async () => {
  const res = await apiAdmin.payments.getAll()
  return res.data.body
}

export const useAdminMovies = () =>
  useQuery({
    queryKey: ["admin", "movies"],
    queryFn: fetchMovies,
  })

export const useAdminShows = () =>
  useQuery({
    queryKey: ["admin", "shows"],
    queryFn: fetchShows,
  })

export const useAdminTheaters = () =>
  useQuery({
    queryKey: ["admin", "theaters"],
    queryFn: fetchTheaters,
  })

export const useAdminBookings = () =>
  useQuery({
    queryKey: ["admin", "bookings"],
    queryFn: fetchBookings,
  })

export const useAdminGenres = () =>
  useQuery({
    queryKey: ["admin", "genres"],
    queryFn: fetchGenres,
  })

export const useAdminPayments = () =>
  useQuery({
    queryKey: ["admin", "payments"],
    queryFn: fetchPayments,
  })

export const useAdminMovieRoles = (movieId?: number) =>
  useQuery({
    queryKey: ["admin", "movie-roles", movieId],
    queryFn: async () => {
      if (!movieId) return null
      const res = await apiAdmin.movies.getRoles(movieId)
      return res.data
    },
    enabled: Boolean(movieId),
  })

export const useAdminBookingsByUser = (userId?: string) =>
  useQuery({
    queryKey: ["admin", "bookings", "user", userId],
    queryFn: async () => {
      if (!userId) return [] as BookingsByUserDto[]
      const res = await apiAdmin.bookings.getByUser(userId)
      return res.data
    },
    enabled: Boolean(userId),
  })

export const useAdminMoviesOptions = () =>
  useQuery({
    queryKey: ["admin", "movies", "options"],
    queryFn: fetchMovies,
  })

export type AdminPaymentList = PaymentListItem[]
export type AdminPaymentDetail = PaymentDetailBody
export type AdminBooking = BookingDto
export type AdminMovie = MovieDto
export type AdminShow = ShowDto
export type AdminTheater = TheaterDto
export type AdminGenre = GenreByIdDto | GenreSummaryDto
export type AdminMovieRoles = RolesForMovieDto | null
export type AdminBookingsByUser = BookingsByUserDto[]


