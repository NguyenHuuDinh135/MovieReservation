import { QueryClient, useQueryClient } from '@tanstack/react-query';
import { useMemo } from 'react';
import { createBrowserRouter } from 'react-router';
import { RouterProvider } from 'react-router/dom';

import {
  default as AppRoot,
  ErrorBoundary as AppRootErrorBoundary
} from './routes/app/root';
  
import { paths } from '@/config/paths';

// eslint-disable-next-line @typescript-eslint/no-explicit-any
const convert = (queryClient: QueryClient) => (m: any) => {
  const { clientLoader, clientAction, default: Component, ...rest } = m;
  return {
    ...rest,
    loader: clientLoader?.(queryClient),
    action: clientAction?.(queryClient),
    Component,
  };
};
// eslint-disable-next-line react-refresh/only-export-components
export const createAppRouter = (queryClient: QueryClient) =>
  createBrowserRouter([
    {
      path: paths.home.path,
      lazy: () => import('./routes/app/root').then(convert(queryClient)),
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
      path: paths.app.root.path,
      element: (
        <AppRoot />
      ),
      ErrorBoundary: AppRootErrorBoundary,
      children: [
        {
          index: true,
          lazy: () => import('./').then(convert(queryClient)),
        }
      ]
    }
  ]);
export const AppRouter = () => {
  const queryClient = useQueryClient();

  const router = useMemo(() => createAppRouter(queryClient), [queryClient]);

  return <RouterProvider router={router} />;
};