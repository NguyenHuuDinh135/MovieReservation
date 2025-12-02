import { GenericError } from './generic';

export const MainErrorFallback = () => {
  return (
    <GenericError
      title="Lỗi ứng dụng"
      description="Đã xảy ra lỗi trong ứng dụng. Vui lòng thử lại."
    />
  );
};