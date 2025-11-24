export const paths = {
  home: {
    path: '/',
    getHref: () => '/',
  },

  auth: {
    register: {
      path: '/register',
      getHref: (redirectTo?: string | null | undefined) =>
        `/register${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
    },
    login: {
      path: '/login',
      getHref: (redirectTo?: string | null | undefined) =>
        `/login${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
    },
    otp: {
      path: '/verify-otp',
      getHref: (redirectTo?: string | null | undefined) =>
        `/verify-otp${redirectTo ? `?redirectTo=${encodeURIComponent(redirectTo)}` : ''}`,
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