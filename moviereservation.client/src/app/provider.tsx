import { Icons } from "@/components/icons";
import * as React from "react";
import { ErrorBoundary } from 'react-error-boundary';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import { queryConfig } from '@/lib/react-query';
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
// import { cn } from "@/lib/utils";
// import { META_THEME_COLORS } from "@/config/site"
import { Toaster } from "sonner";
import { ThemeProvider } from "@/components/theme-provider";
import { LayoutProvider } from "@/hooks/use-layout";
import { ActiveThemeProvider } from "@/components/active-theme";
import { TailwindIndicator } from "@/components/tailwind-indicator";
type AppProviderProps = {
  children: React.ReactNode;
};
export const AppProvider = ({ children }: AppProviderProps) =>{
  const [queryClient] = React.useState(
    () =>
      new QueryClient({
        defaultOptions: queryConfig,
      }),
  );
  
  return (
        <React.Suspense fallback={
            <div className="flex h-screen w-screen items-center justify-center">
              <Icons.spinner size="xl" />
            </div>
        }>
          <ErrorBoundary
            FallbackComponent={({ error, resetErrorBoundary }) => (
              <div role="alert" className="p-4">
                <p>Something went wrong:</p>
                <pre>{error.message}</pre>
                <button onClick={resetErrorBoundary}>Try again</button>
              </div>
            )}
          >
            <QueryClientProvider client={queryClient}>
              {/* {import.meta.env.DEV && <ReactQueryDevtools />} */}
              <ThemeProvider>
                <LayoutProvider>
                  <ActiveThemeProvider>
                    {children}
                    <TailwindIndicator />
                    <Toaster position="top-center" />
                  </ActiveThemeProvider>
                </LayoutProvider>
              </ThemeProvider>
            </QueryClientProvider>

          </ErrorBoundary>
        </React.Suspense>
      
    
  );
}