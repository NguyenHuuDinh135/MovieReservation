export const paths = {
  home: {
    path: '/',
    getHref: () => '/',
  },

  auth: {
    register: {
      path: '/auth/register',
      getHref: (redirectTo?: string | null | undefined) =>
        `/auth/register${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
    },
    login: {
      path: '/auth/login',
      getHref: (redirectTo?: string | null | undefined) =>
        `/auth/login${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
    },
    otp: {
      path: '/auth/verify-otp',
      getHref: (redirectTo?: string | null | undefined) =>
        `/auth/verify-otp${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
    }
  },

  app: {
    root: {
      path: '/app',
      getHref: () => '/app',
    },
    dashboard: {
      path: '',
      getHref: () => '/app',
    },
  },

  admin: {
    root: {
      path: '/admin',
      getHref: () => '/admin',
    },
    dashboard: {
      path: '/admin/dashboard',
      getHref: () => '/admin/dashboard',
    },
    movies: {
      path: '/admin/movies',
      getHref: () => '/admin/movies',
    },
    cinemas: {
      path: '/admin/cinemas',
      getHref: () => '/admin/cinemas',
    },
    showtimes: {
      path: '/admin/showtimes',
      getHref: () => '/admin/showtimes',
    },
    bookings: {
      path: '/admin/bookings',
      getHref: () => '/admin/bookings',
    },
    users: {
      path: '/admin/users',
      getHref: () => '/admin/users',
    },
    settings: {
      path: '/admin/settings',
      getHref: () => '/admin/settings',
    },
  }
} as const;