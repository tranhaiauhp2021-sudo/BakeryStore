-- BAKERY STORE / MyStore - dung voi ASP.NET MVC5 + Entity Framework 6
-- Chay bang SSMS (SQL Server), co the chay lai ma khong xoa data cu.
IF DB_ID(N'MyStore') IS NULL CREATE DATABASE [MyStore];
GO
USE [MyStore];
GO
IF OBJECT_ID(N'dbo.[User]',N'U') IS NULL
CREATE TABLE dbo.[User] (Username NVARCHAR(255) NOT NULL PRIMARY KEY, [Password] NVARCHAR(255) NOT NULL, UserRole NCHAR(1) NOT NULL);
GO
-- Database2024.txt goc dung NCHAR(50) se khong du cho hash PBKDF2.
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id=OBJECT_ID(N'dbo.[User]') AND name='Password' AND max_length<510)
BEGIN
 ALTER TABLE dbo.[User] ALTER COLUMN [Password] NVARCHAR(255) NOT NULL;
END;
GO
IF OBJECT_ID(N'dbo.Customer',N'U') IS NULL
CREATE TABLE dbo.Customer (CustomerID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,CustomerName NVARCHAR(MAX) NOT NULL,CustomerPhone NVARCHAR(15) NOT NULL,CustomerEmail NVARCHAR(MAX) NULL,CustomerAddress NVARCHAR(MAX) NULL,Username NVARCHAR(255) NOT NULL,CONSTRAINT FK_User_Customer FOREIGN KEY(Username) REFERENCES dbo.[User](Username));
GO
IF OBJECT_ID(N'dbo.Category',N'U') IS NULL
CREATE TABLE dbo.Category (CategoryID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,CategoryName NVARCHAR(MAX) NOT NULL);
GO
IF OBJECT_ID(N'dbo.Product',N'U') IS NULL
CREATE TABLE dbo.Product (ProductID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,CategoryID INT NOT NULL,ProductName NVARCHAR(MAX) NOT NULL,ProductDecription NVARCHAR(MAX) NOT NULL,ProductPrice DECIMAL(18,2) NOT NULL,ProductImage NVARCHAR(MAX) NULL,CONSTRAINT FK_Pro_Category FOREIGN KEY(CategoryID) REFERENCES dbo.Category(CategoryID));
GO
IF OBJECT_ID(N'dbo.[Order]',N'U') IS NULL
CREATE TABLE dbo.[Order](OrderID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,CustomerID INT NOT NULL,OrderDate DATE NOT NULL,TotalAmount DECIMAL(18,2) NOT NULL,PaymentStatus NVARCHAR(MAX) NULL,AddressDelivery NVARCHAR(MAX) NOT NULL,FOREIGN KEY(CustomerID) REFERENCES dbo.Customer(CustomerID));
GO
IF OBJECT_ID(N'dbo.OrderDetail',N'U') IS NULL
CREATE TABLE dbo.OrderDetail(ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,ProductID INT NOT NULL,OrderID INT NOT NULL,Quantity INT NOT NULL,UnitPrice DECIMAL(18,2) NOT NULL,FOREIGN KEY(ProductID) REFERENCES dbo.Product(ProductID),FOREIGN KEY(OrderID) REFERENCES dbo.[Order](OrderID));
GO
-- Du lieu mau
IF NOT EXISTS (SELECT 1 FROM dbo.Category WHERE CategoryName=N'Bánh kem') INSERT INTO dbo.Category(CategoryName) VALUES(N'Bánh kem');
IF NOT EXISTS (SELECT 1 FROM dbo.Category WHERE CategoryName=N'Bánh mì & Croissant') INSERT INTO dbo.Category(CategoryName) VALUES(N'Bánh mì & Croissant');
IF NOT EXISTS (SELECT 1 FROM dbo.Category WHERE CategoryName=N'Cookies') INSERT INTO dbo.Category(CategoryName) VALUES(N'Cookies');
IF NOT EXISTS (SELECT 1 FROM dbo.Category WHERE CategoryName=N'Cupcake') INSERT INTO dbo.Category(CategoryName) VALUES(N'Cupcake');
IF NOT EXISTS (SELECT 1 FROM dbo.Category WHERE CategoryName=N'Tart & Pastry') INSERT INTO dbo.Category(CategoryName) VALUES(N'Tart & Pastry');
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Strawberry Dream Cake')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Strawberry Dream Cake',N'Bánh kem dâu tây mềm mịn với lớp whipping cream tươi và dâu mọng.',320000,N'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh kem';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Butter Croissant')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Butter Croissant',N'Croissant bơ Pháp thơm lừng, vỏ giòn nhiều lớp và ruột mềm xốp.',49000,N'https://images.unsplash.com/photo-1555507036-ab1f4038808a?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh mì & Croissant';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Chocolate Chip Cookies')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Chocolate Chip Cookies',N'Bánh quy chocolate chip thơm bơ, giòn cạnh và mềm bên trong.',45000,N'https://images.unsplash.com/photo-1499636136210-6f4ee915583e?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Cookies';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Vanilla Cloud Cupcake')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Vanilla Cloud Cupcake',N'Cupcake vani với lớp kem ngọt dịu và trang trí dễ thương.',58000,N'https://images.unsplash.com/photo-1559622214-f8a9850965bb?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Cupcake';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Artisan Sourdough')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Artisan Sourdough',N'Bánh mì sourdough lên men tự nhiên, vỏ giòn ruột dẻo.',85000,N'https://images.unsplash.com/photo-1509440159596-0249088772ff?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh mì & Croissant';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Tiramisu Classic')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Tiramisu Classic',N'Tiramisu chuẩn vị với mascarpone, cà phê và chút bột cacao.',79000,N'https://images.unsplash.com/photo-1488477181946-6428a0291777?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh kem';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Red Velvet Cupcake')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Red Velvet Cupcake',N'Cupcake red velvet mềm mượt cùng kem phô mai thơm béo.',65000,N'https://images.unsplash.com/photo-1603532648955-039310d9ed75?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Cupcake';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Lemon Tart')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Lemon Tart',N'Tart chanh vàng chua ngọt hài hòa trên đế bánh giòn.',69000,N'https://images.unsplash.com/photo-1464305795204-6f5bbfc7fb81?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Tart & Pastry';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Berry Cheesecake')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Berry Cheesecake',N'Cheesecake phô mai tươi với sốt berries chua ngọt.',89000,N'https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh kem';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Chocolate Fudge Cake')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Chocolate Fudge Cake',N'Bánh chocolate đậm đà với lớp ganache sánh mịn.',115000,N'https://images.unsplash.com/photo-1587668178277-295251f900ce?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Bánh kem';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Mini Fruit Tart')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Mini Fruit Tart',N'Bánh tart mini trái cây tươi đủ sắc màu.',62000,N'https://images.unsplash.com/photo-1623334044303-241021148842?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Tart & Pastry';
IF NOT EXISTS (SELECT 1 FROM dbo.Product WHERE ProductName=N'Almond Butter Cookies')
INSERT INTO dbo.Product(CategoryID,ProductName,ProductDecription,ProductPrice,ProductImage)
SELECT TOP(1) CategoryID,N'Almond Butter Cookies',N'Bánh quy bơ hạnh nhân giòn tan, thơm dịu và vừa miệng.',52000,N'https://images.unsplash.com/photo-1519869325930-281384150729?w=850&q=80' FROM dbo.Category WHERE CategoryName=N'Cookies';

IF NOT EXISTS (SELECT 1 FROM dbo.[User] WHERE Username=N'admin') INSERT INTO dbo.[User](Username,[Password],UserRole) VALUES(N'admin',N'100000.3ya/VwaAHu8dcHLKyGM4gQ==.ES5goFRxauJXJEfRprDDcRnPV/4kA8vkfE4/7hXMtX4=',N'A');
GO
