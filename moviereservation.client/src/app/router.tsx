import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { useMemo } from 'react';
import { createBrowserRouter } from 'react-router';
import { RouterProvider } from 'react-router/dom';

import { paths } from '@/config/paths';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const convert = (queryClient: QueryClient) => (m: any) => {
  const { clientLoader, clientAction, default: Component, ErrorBoundary, ...rest } = m;
  return {
    ...rest,
    loader: clientLoader?.(queryClient),
    action: clientAction?.(queryClient),
    Component,
    ErrorBoundary,
  };
};
// eslint-disable-next-line react-refresh/only-export-components
export const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: paths.home.path,
      lazy: () => import('./routes/app/layout').then(convert(queryClient)),
    },
    {
      path: paths.auth.register.path,
      lazy: () => import('./routes/auth/register/page').then(convert(queryClient)),
    },
    {
      path: paths.auth.login.path,
      lazy: () => import('./routes/auth/login/page').then(convert(queryClient)),
    },
    {
      path: paths.auth.otp.path,
      lazy: () => import('./routes/auth/otp/page').then(convert(queryClient)),
    },
    {
      path: paths.admin.root.path,
      lazy: () => import('./routes/admin/layout').then(convert(queryClient)),
      children: [
        {
          index: true,
          lazy: () => import('./routes/admin/dashboard/page').then(convert(queryClient)),
        },
        {
          path: 'dashboard',
          lazy: () => import('./routes/admin/dashboard/page').then(convert(queryClient)),
        },
        {
          path: 'movies',
          lazy: () => import('./routes/admin/movies/page').then(convert(queryClient)),
        },
        {
          path: 'genres',
          lazy: () => import('./routes/admin/genres/page').then(convert(queryClient)),
        },
        {
          path: 'cinemas',
          lazy: () => import('./routes/admin/theaters/page').then(convert(queryClient)),
        },
        {
          path: 'showtimes',
          lazy: () => import('./routes/admin/showtimes/page').then(convert(queryClient)),
        },
        {
          path: 'bookings',
          lazy: () => import('./routes/admin/bookings/page').then(convert(queryClient)),
        },
        {
          path: 'users',
          lazy: () => import('./routes/admin/users/page').then(convert(queryClient)),
        },
        {
          path: 'settings',
          lazy: () => import('./routes/admin/settings/layout').then(convert(queryClient)),
          children: [
            {
              index: true,
              lazy: () => import('./routes/admin/settings/page').then(convert(queryClient)),
            },
            {
              path: 'users',
              lazy: () => import('./routes/admin/settings/users/page').then(convert(queryClient)),
            },
            {
              path: 'users/:userId/permissions',
              lazy: () => import('./routes/admin/settings/users/[userId]/permissions/page').then(convert(queryClient)),
            },
            {
              path: 'user-groups',
              lazy: () => import('./routes/admin/settings/user-groups/page').then(convert(queryClient)),
            },
            {
              path: 'user-groups/:roleId/permissions',
              lazy: () => import('./routes/admin/settings/user-groups/[roleId]/permissions/page').then(convert(queryClient)),
            },
            {
              path: 'roles',
              lazy: () => import('./routes/admin/settings/roles/page').then(convert(queryClient)),
            },
          ],
        },
      ],
    },
    {
      path: paths.home.path,
      lazy: () => import('./routes/app/layout').then(convert(queryClient)),
      children: [
        {
          index: true,
          lazy: () => import('./routes/app/root/page').then(convert(queryClient)),
        }
      ]
    },
    {
      path: '*',
      lazy: () => import('./routes/errors/404/page').then(convert(queryClient)),
    }
  ]);
export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};