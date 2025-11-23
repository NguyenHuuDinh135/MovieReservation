import { SiteHeader } from '@/components/site-header-v1';

import { SiteFooter } from '@/components/site-footer';
import { Outlet } from 'react-router';

export const ErrorBoundary = () => {
  return <div>Something went wrong!</div>;
};

// eslint-disable-next-line @typescript-eslint/no-unused-vars
const AppRoot = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="bg-background relative z-10 flex min-h-svh flex-col">
      <SiteHeader />
      <main className="flex flex-1 flex-col"><Outlet /></main>
      <SiteFooter />
    </div>
  );
};

export default AppRoot;