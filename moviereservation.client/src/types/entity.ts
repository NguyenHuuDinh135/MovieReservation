// export type User = {
//   id: string;
//   userName?: string;
//   email?: string;
//   phoneNumber?: string;
//   address: string;
//   contact: string;
//   bookings?: Booking[];
//   payments?: Payment[];
// };

// export type AuthResponse = {
//   jwt: string;
//   user: User;
// };

// export type Booking = {
//   id: number;
//   userId: string;
//   showId: number;
//   seatRow: string;
//   seatNumber: number;
//   price: number;
//   status: 'Confirmed' | 'Reserved'|'Cancelled';
//   bookingDateTime: string; // ISO date string
//   user?: User;
//   show?: Show;
// };

// export type Payment = {
//   id: number;
//   amount: number;
//   paymentDateTime: string; // ISO date string
//   method: 'Cash' | 'Card' | 'Cod';
//   userId: string;
//   showId: number;
//   user?: User;
//   show?: Show;
// };

// export type Movie = {
//   id: number;
//   title: string;
//   summary: string;
//   year: number;
//   rating?: number;
//   trailerUrl: string;
//   posterUrl: string;
//   movieType: 'ComingSoon' | 'NowShowing' | 'Removed';
//   shows?: Show[];
//   movieGenres?: MovieGenre[];
//   movieRoles?: MovieRole[];
// };

// export type Genre = {
//   id: number;
//   name: string;
//   movieGenres?: MovieGenre[];
// };

// export type MovieGenre = {
//   movieId: number;
//   genreId: number;
//   movie?: Movie;
//   genre?: Genre;
// };

// export type Role = {
//   id: number;
//   fullName: string;
//   age: number;
//   pictureUrl: string;
//   movieRoles?: MovieRole[];
// };

// export type MovieRole = {
//   movieId: number;
//   roleId: number;
//   roleType: 'Cast' | 'Director' | 'Producer';
//   movie?: Movie;
//   role?: Role;
// };

// export type Show = {
//   id: number;
//   startTime: string; // HH:mm:ss
//   endTime: string;   // HH:mm:ss
//   date: string;      // ISO date string
//   movieId: number;
//   theaterId: number;
//   status: 'Free' | 'AlmostFull' | 'Full';
//   type: 'ThreeD' | 'TwoD';
//   movie?: Movie;
//   theater?: Theater;
//   bookings?: Booking[];
//   payments?: Payment[];
// };

// export type Theater = {
//   id: number;
//   name: string;
//   numOfRows: number;
//   seatsPerRow: number;
//   type: 'Normal' | 'Royal';
//   shows?: Show[];
//   theaterSeats?: TheaterSeat[];
// };

// export type TheaterSeat = {
//   seatRow: string;
//   seatNumber: number;
//   theaterId: number;
//   type: 'Missing' | 'Blocked';
//   theater?: Theater;
// };