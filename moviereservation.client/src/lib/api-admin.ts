import api from './api'

/**
 * Admin API surface (protected endpoints).
 * DTOs mirror MovieReservation.Server.* controllers & application layer.
 */

type EnumValue<T extends string> = T | Lowercase<T> | number
type DateString = string
type TimeString = string

export type MovieType = EnumValue<'ComingSoon' | 'NowShowing' | 'Removed'>
export type TheaterType = EnumValue<'Normal' | 'Royal'>
export type ShowStatus = EnumValue<'Free' | 'AlmostFull' | 'Full'>
export type ShowType = EnumValue<'ThreeD' | 'TwoD'>
export type BookingStatus = EnumValue<'Confirmed' | 'Reserved' | 'Cancelled'>
export type PaymentMethod = EnumValue<'Cash' | 'Card' | 'Cod'>

// --- Movies DTOs/commands ---
export interface GenreDto {
  id: number
  name: string
}

export interface MovieDto {
  id: number
  title: string
  summary: string
  year: number
  rating?: number | null
  trailerUrl: string
  posterUrl: string
  movieType: MovieType
  genres: GenreDto[]
}

export type FilteredMovieDto = Omit<MovieDto, 'genres'>

export interface MovieRoleDto {
  id: number
  fullName: string
  age: number
  pictureUrl: string
}

export interface RolesForMovieDto {
  id: number
  title: string
  roles: MovieRoleDto[]
}

export interface FilteredMoviesQuery {
  /**
   * Backend expects `movieType` query param (enum as string or number).
   */
  movieType: MovieType
}

export interface CreateMovieCommand {
  title: string
  year: number
  summary: string
  trailerUrl: string
  posterUrl: string
  movieType: MovieType
}

export interface UpdateMovieCommand {
  id: number
  movieType?: MovieType
}

// --- Shows DTOs/commands ---
export interface ShowDto {
  id: number
  startTime: TimeString
  endTime: TimeString
  date: DateString
  movieId: number
  theaterId: number
  status: ShowStatus
  type: ShowType
}

export interface CreateShowCommand {
  startTime: TimeString
  endTime: TimeString
  date: DateString
  movieId: number
  theaterId: number
  status: ShowStatus
  type: ShowType
}

export interface UpdateShowCommand {
  id: number
  theaterId: number
  startTime: TimeString
  endTime: TimeString
  date: DateString
}

export interface ShowsFilterQuery {
  date: DateString | Date
}

// --- Theaters DTOs/commands ---
export interface TheaterSeatDto {
  seatRow: string
  seatNumber: number
}

export interface TheaterDto {
  id?: number
  name: string
  numOfRows: number
  seatsPerRow: number
  type: TheaterType
  missing: TheaterSeatDto[]
  blocked: TheaterSeatDto[]
}

export interface CreateTheaterCommand {
  name: string
  numOfRows: number
  seatsPerRow: number
  type: TheaterType
}

export interface UpdateTheaterCommand {
  id: number
  theaterType: TheaterType
}

// --- Bookings DTOs/commands ---
export interface BookingDto {
  id: number
  userId: string
  showId: number
  seatRow: string
  seatNumber: number
  price: number
  status: BookingStatus
  bookingDateTime: DateString
}

export type BookingByIdDto = BookingDto

export interface BookingInfoDto {
  bookingId: number
  price: number
  seatRow: string
  seatNumber: number
  bookingStatus: BookingStatus
  bookingDateTime: DateString
}

export interface ShowInfoDto {
  showId: number
  showType: ShowType | string
  showDatetime: DateString
}

export interface BookingsByUserDto {
  title: string
  posterUrl: string
  show: ShowInfoDto
  bookings: BookingInfoDto[]
}

export interface BookingsByShowDto {
  seatRow: string
  seatNumber: number
}

export interface CreateBookingCommand {
  userId: string
  showId: number
  seatRow: string
  seatNumber: number
  price: number
  status?: BookingStatus
  bookingDateTime?: DateString
}

export interface UpdateBookingCommand {
  id: number
  seatRow?: string
  seatNumber?: number
  status?: BookingStatus
}

// --- Payments DTOs/commands ---
export interface PaymentEnvelope<TBody> {
  headers: {
    success: number
    message: string
  }
  body: TBody
}

export interface PaymentListItem {
  payment_id: number
  amount: number
  payment_datetime: string
  payment_method: string
  user_id: string
  show_id: number
}

export interface PaymentDetailBody {
  payment_id: number
  amount: number
  payment_datetime: string
  payment_method: string
  movie?: {
    title: string
    poster_url: string
  } | null
}

export type PaymentListResponse = PaymentEnvelope<PaymentListItem[]>
export type PaymentDetailResponse = PaymentEnvelope<PaymentDetailBody>
export type PaymentsByUserResponse = PaymentEnvelope<PaymentDetailBody[]>

export interface CreatePaymentCommand {
  amount: number
  paymentDateTime?: DateString
  method?: PaymentMethod
  userId: string
  showId: number
}

export interface UpdatePaymentCommand extends Required<CreatePaymentCommand> {
  id: number
}

// --- Genres DTOs/commands ---
export interface GenreSummaryDto {
  id: number
  name: string
}

export type GenreByIdDto = GenreSummaryDto
export type GenresByMovieDto = GenreSummaryDto

export interface CreateGenreCommand {
  name: string
}

export interface UpdateGenreCommand {
  id: number
  name: string
}

// --- Users DTOs ---
export interface UserDto {
  id: string
  userName: string
  email: string
  phoneNumber?: string | null
  address: string
  contact: string
  emailConfirmed: boolean
  createdAt: string
  updatedAt?: string | null
  roles: string[]
}

// --- Roles DTOs (for roles, not movie roles) ---
export interface IAMRoleDto {
  id: string
  name: string
  normalizedName: string
}

// --- Movies endpoints ---
export const moviesAdminApi = {
  getAll: () => api.get<MovieDto[]>('/movies/all'),
  getById: (id: number) => api.get<MovieDto>(`/movies/id/${id}`),
  getFiltered: (params: FilteredMoviesQuery) => api.get<FilteredMovieDto[]>('/movies/filtered', { params }),
  getRoles: (id: number) => api.get<RolesForMovieDto>(`/movies/id/${id}/roles`),
  create: (payload: CreateMovieCommand) => api.post<number>('/movies/create', payload),
  update: (payload: UpdateMovieCommand) => api.put<void>('/movies/update', payload),
  delete: (id: number) => api.delete<void>(`/movies/delete/id/${id}`),
}

// --- Shows endpoints ---
export const showsAdminApi = {
  getAll: () => api.get<ShowDto[]>('/shows/all'),
  getById: (id: number) => api.get<ShowDto>(`/shows/id/${id}`),
  getFiltered: (params: ShowsFilterQuery) => api.get<ShowDto[]>('/shows/filters', { params }),
  create: (payload: CreateShowCommand) => api.post<number>('/shows/create', payload),
  update: (payload: UpdateShowCommand) => api.put<void>('/shows/update', payload),
  delete: (id: number) => api.delete<void>(`/shows/id/${id}`),
}

// --- Theaters endpoints ---
export const theatersAdminApi = {
  getAll: () => api.get<TheaterDto[]>('/theaters'),
  getById: (id: number) => api.get<TheaterDto>(`/theaters/id/${id}`),
  create: (payload: CreateTheaterCommand) => api.post<number>('/theaters', payload),
  update: (payload: UpdateTheaterCommand) => api.put<void>('/theaters', payload),
  delete: (id: number) => api.delete<void>(`/theaters/id/${id}`),
}

// --- Bookings endpoints ---
export const bookingsAdminApi = {
  getAll: () => api.get<BookingDto[]>('/bookings/all'),
  getById: (id: number) => api.get<BookingByIdDto>(`/bookings/id/${id}`),
  getByUser: (userId: string) => api.get<BookingsByUserDto[]>(`/bookings/users/${userId}`),
  getByShow: (showId: number) => api.get<BookingsByShowDto[]>(`/bookings/shows/${showId}`),
  create: (payload: CreateBookingCommand) => api.post<number>('/bookings/create', payload),
  update: (payload: UpdateBookingCommand) => api.put<void>('/bookings/update', payload),
  delete: (id: number) => api.delete<void>(`/bookings/delete/${id}`),
}

// --- Payments endpoints ---
export const paymentsAdminApi = {
  getAll: () => api.get<PaymentListResponse>('/payments/all'),
  getById: (id: number) => api.get<PaymentDetailResponse>(`/payments/id/${id}`),
  getByUser: (userId: string) => api.get<PaymentsByUserResponse>(`/payments/users/${userId}`),
  create: (payload: CreatePaymentCommand) => api.post<number>('/payments/create', payload),
  update: (payload: UpdatePaymentCommand) => api.put<void>('/payments/update', payload),
  delete: (id: number) => api.delete<void>(`/payments/delete/${id}`),
}

// --- Genres endpoints ---
export const genresAdminApi = {
  getAll: () => api.get<GenreSummaryDto[]>('/genre/all'),
  getById: (id: number) => api.get<GenreByIdDto>(`/genre/id/${id}`),
  getByMovie: (movieId: number) => api.get<GenresByMovieDto[]>(`/genre/movies/${movieId}`),
  create: (payload: CreateGenreCommand) => api.post<number>('/genre/create', payload),
  update: (payload: UpdateGenreCommand) => api.put<void>('/genre/update', payload),
  delete: (id: number) => api.delete<void>(`/genre/delete/${id}`),
}

// --- Users endpoints ---
export const usersAdminApi = {
  getAll: () => api.get<UserDto[]>('/users/all'),
  getById: (id: string) => api.get<UserDto>(`/users/id/${id}`),
}

const apiAdmin = {
  movies: moviesAdminApi,
  shows: showsAdminApi,
  theaters: theatersAdminApi,
  bookings: bookingsAdminApi,
  payments: paymentsAdminApi,
  genres: genresAdminApi,
  users: usersAdminApi,
}

export default apiAdmin