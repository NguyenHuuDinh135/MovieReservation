import { SiteHeader } from '@/components/site-header';



export const ErrorBoundary = () => {
  return <div>Something went wrong!</div>;
};

const AppRoot = ({ children }: { children: React.ReactNode }) => {
  return (
    <div className="bg-background relative z-10 flex min-h-svh flex-col">
      <SiteHeader />
      <main className="flex flex-1 flex-col">{children}</main>
      {/* <SiteFooter /> */}
    </div>
  );
};

export default AppRoot;