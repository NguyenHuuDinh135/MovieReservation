# 🎬 MovieReservation

🎥 MovieReservation là hệ thống đặt vé xem phim trực tuyến, cung cấp giải pháp toàn diện cho người dùng và quản trị viên trong việc quản lý, đặt vé, thanh toán, và theo dõi các suất chiếu phim tại rạp.

---

## 📚 Mục lục

- [Giới thiệu](#giới-thiệu)
- [Kiến trúc hệ thống](#kiến-trúc-hệ-thống)
- [Tính năng chính](#tính-năng-chính)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Cách cài đặt & chạy dự án](#cách-cài-đặt--chạy-dự-án)
- [Cấu trúc thư mục](#cấu-trúc-thư-mục)

---

## 👋 Giới thiệu

MovieReservation giúp người dùng:

- Đăng ký, đăng nhập, xác thực OTP qua email.
- Xem danh sách phim, suất chiếu, rạp chiếu.
- Đặt vé, chọn ghế, thanh toán trực tuyến.
- Quản lý thông tin cá nhân, lịch sử đặt vé.

Quản trị viên có thể:

- Quản lý phim, suất chiếu, rạp, vai trò, thể loại.
- Theo dõi doanh thu, thống kê đặt vé.

---

## 🏗️ Kiến trúc hệ thống

- **Frontend**: React + TypeScript, giao diện hiện đại, responsive, sử dụng [Vite](https://vitejs.dev/) để build và phát triển.
- **Backend**: ASP.NET Core Web API (.NET 8), sử dụng Entity Framework Core, xác thực JWT, quản lý người dùng với Identity.
- **Database**: SQL Server, lưu trữ dữ liệu phim, suất chiếu, người dùng, vé, thanh toán.
- **Caching**: Redis, tăng hiệu năng truy vấn và xác thực OTP.

---

## ✨ Tính năng chính

### Người dùng

- Đăng ký, đăng nhập, xác thực OTP qua email.
- Xem phim, suất chiếu, rạp.
- Đặt vé, chọn ghế, thanh toán (thẻ, ví điện tử).
- Quản lý thông tin cá nhân, lịch sử đặt vé.

### Quản trị viên

- Quản lý phim, suất chiếu, rạp, vai trò, thể loại.
- Quản lý người dùng, phân quyền.
- Thống kê doanh thu, số lượng vé bán ra.

---

## 🛠️ Công nghệ sử dụng

- **Frontend**: React, TypeScript, TailwindCSS, Radix UI, React Query.
- **Backend**: ASP.NET Core, Entity Framework Core, MediatR, FluentValidation, StackExchange.Redis.
- **Database**: SQL Server.
- **Khác**: Swagger, Sonner (toast), Lucide Icons.

---

## 🚀 Cách cài đặt & chạy dự án

### Yêu cầu

- Node.js >= 18
- .NET SDK >= 8.0
- SQL Server

### Các bước thực hiện

1. **Clone dự án**

   ```sh
   git clone https://github.com/your-org/MovieReservation.git
   cd MovieReservation
   ```

2. **Cài đặt frontend**

   ```sh
   cd moviereservation.client
   npm install
   npm run dev
   ```

3. **Cài đặt backend**

   ```sh
   cd ../MovieReservation.Server
   dotnet restore
   dotnet ef database update
   dotnet run
   ```

4. **Truy cập hệ thống**
   - Frontend: [https://localhost:62111](https://localhost:62111)
   - Backend API: [https://localhost:7292/swagger](https://localhost:7292/swagger)

---

## 📁 Cấu trúc thư mục

```
MovieReservation/
├── moviereservation.client/      # Frontend React + Vite
│   ├── src/                     # Source code giao diện
│   ├── public/                  # Tài nguyên tĩnh
│   └── ...                      # Cấu hình, tài liệu
├── MovieReservation.Server/      # Backend ASP.NET Core
│   ├── Application/             # Business logic (CQRS, MediatR)
│   ├── Domain/                  # Entity, Enum
│   ├── Infrastructure/          # Database, Identity, Redis
│   ├── Data/                    # Migration, Seed
│   └── ...                      # Cấu hình, tài liệu
├── README.md                    # Tài liệu dự án
└── ...
```

---
