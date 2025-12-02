import { SiteHeader } from '@/components/site-header-v1';
import { SiteFooter } from '@/components/site-footer';
import { Outlet, useRouteError, isRouteErrorResponse } from 'react-router';
import { GenericError } from '@/components/errors/generic';

export const ErrorBoundary = () => {
  const error = useRouteError();

  if (isRouteErrorResponse(error)) {
    if (error.status === 404) {
      return <GenericError title="404 - Không tìm thấy trang" description="Trang bạn đang tìm kiếm không tồn tại." />;
    }
    if (error.status === 500) {
      return <GenericError title="500 - Lỗi máy chủ" description="Đã xảy ra lỗi từ phía máy chủ. Vui lòng thử lại sau." />;
    }
  }

  const errorMessage = error instanceof Error ? error.message : 'Đã xảy ra lỗi không mong muốn';
  return <GenericError error={error instanceof Error ? error : undefined} title="Lỗi" description={errorMessage} />;
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