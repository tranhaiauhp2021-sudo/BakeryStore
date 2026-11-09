# 12 – BAKERY STORE 🍰

Website bán bánh lấy cảm hứng từ bố cục và phong cách thanh lịch của Banno Bakery, **tự viết** với ASP.NET MVC5 + C# + Entity Framework 6 + SQL Server.

## Chạy trên Visual Studio 2022 / 2019 (Windows)

1. Cài Visual Studio có workload **ASP.NET and web development**, .NET Framework 4.8 Developer Pack, SQL Server Express hoặc LocalDB/Developer.
2. Mở `Database/01_Create_MyStore.sql` bằng SQL Server Management Studio, chạy toàn bộ script. Nếu máy đã tạo MyStore bằng `database2024.txt`, script sẽ bổ sung dữ liệu mẫu mà không xóa các bảng, và mở rộng cột mật khẩu thành `NVARCHAR(255)`.
3. Mở `BakeryStore.sln` trong Visual Studio, cho phép **Restore NuGet Packages** (hoặc Tools → NuGet Package Manager → Restore).
4. Sửa `BakeryStore/Web.config`: `Data Source=.` thành tên SQL Server máy bạn, chẳng hạn `.\\SQLEXPRESS` hoặc `(localdb)\\MSSQLLocalDB`. Dữ liệu mặc định `Initial Catalog=MyStore;Integrated Security=True`.
5. Run bằng IIS Express (F5). Mở `/` xem trang khách hàng; `/Admin` xem trang quản trị.

**Admin demo:** user `admin` / password `Admin@123456` (đổi ngay nếu triển khai ngoài máy học tập).

## Chức năng

- Customer: Home, danh mục, tìm kiếm, sắp xếp, phân trang, chi tiết bánh, bánh liên quan, giỏ hàng Session, đăng ký / đăng nhập / đăng xuất, sửa thông tin cá nhân, COD / đặt hàng, xác nhận, lịch sử mua.
- Admin: dashboard, CRUD danh mục / sản phẩm (có upload ảnh), xem khách hàng và đơn hàng, CRUD tài khoản admin.
- Responsive: mobile menu, theme kem / nâu chocolate, Font Awesome 5, CSS riêng.
- `DemoFrontend/index.html`: **bản chạy xem giao diện ngay** không cần Visual Studio; có tìm kiếm, bộ lọc và giỏ hàng mô phỏng với localStorage. Đây là demo FE, không lưu vào database.

## Ghi chú về bài học

Slide của cô sử dụng **EF6 Database First EDMX**. Project có sẵn này dùng **EF6 ánh xạ vào CSDL MyStore đã có** qua các class và `[Table]`, giúp chạy ngay mà không cần tạo EDMX trên máy biên soạn. Nếu giáo viên chấm bắt buộc có file `.edmx`, mở Visual Studio → Models → Add → ADO.NET Entity Data Model → EF Designer from Database → kết nối MyStore và cập nhật Controller sang model được sinh. Không mix cùng lúc hai bộ model trùng tên.

Giữ nguyên tên cột `ProductDecription` (thiếu chữ s) trong SQL do file database2024 của môn học đặt như vậy.

**Lưu ý:** Chỉ tích hợp COD / xác nhận đơn hàng demo; chưa có cổng thanh toán thực, email xác thực, chống spam, logging và hệ thống phân quyền production. Mật khẩu PBKDF2 với salt; script demo chứa mật khẩu tài khoản mẫu, không dùng tài khoản đó trên web public. Mở bằng Windows + Visual Studio; không thể chạy .NET Framework 4.8 MVC5 trên Linux mặc định.
