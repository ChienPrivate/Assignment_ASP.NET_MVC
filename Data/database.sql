/* 
	Assignment : NET_104 Lập Trình C#4 
	Giảng viên hướng dẫn: Huỳnh Khắc Duy
	Người thực hiện : Nguyễn Ngọc Chiến - Mã sinh viên: PS27765
	Lơp : SD18317
	Ngày Thực hiện: 9/10/2024
*/

-- 1. Câu lệnh Tạo CSDL
USE master
GO

CREATE DATABASE MVC_Samsung_Database
GO

-- 2. Câu lệnh trỏ đến CSDL đã tạo
USE MVC_Samsung_Database
GO

-- 3. Câu lệnh tạo bảng

-- 3.1 Câu lệnh tạo bảng Category (Danh mục)
CREATE TABLE Category(
	CategoryId VARCHAR(50) NOT NULL,
	CategoryName NVARCHAR(100) NOT NULL,
	DisplayOrder INT,
	CONSTRAINT PK_Category PRIMARY KEY (CategoryId)
)
GO

-- 3.2 Câu lệnh tạo bảng Product (Sản Phẩm)
CREATE TABLE Product(
	ProductId VARCHAR(50) NOT NULL,
	ProductName NVARCHAR(50) NOT NULL,
	ImageUrl NVARCHAR(MAX),
	Price FLOAT NOT NULL,
	ProductQuantity INT NOT NULL,
	DescriptionText NVARCHAR(MAX),
	CategoryId VARCHAR(50),
	Discount FLOAT,
	Info NVARCHAR(MAX),
	RealeaseDate DATE
	CONSTRAINT PK_Product PRIMARY KEY (ProductId),
	CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
)
GO



-- 3.3 Câu lệnh tạo bảng EUser (Người dùng)
CREATE TABLE EUser(
	EUserId VARCHAR(50) NOT NULL,
	Email VARCHAR(100) NOT NULL,
	Pwd VARCHAR(100) NOT NULL,
	EUserName NVARCHAR(100) NOT NULL,
	EUserAddress NVARCHAR(100),
	BirthDay DATE NOT NULL,
	EUserRole VARCHAR(100) NOT NULL,
	IsConfirm BIT NOT NULL,
	CONSTRAINT PK_EUser PRIMARY KEY (EUserId),
)
GO

-- 3.4 Câu lệnh tạo bảng Coupon (Phiếu giảm giá)
CREATE TABLE Coupon(
	CouponCode VARCHAR(50) NOT NULL,
	Discount FLOAT,
	MinAmount INT,
	CONSTRAINT PK_Coupon PRIMARY KEY (CouponCode)
)

-- 3.5 Câu lệnh tạo bảng Cart (Giỏ hàng)
CREATE TABLE Cart(
	CartId VARCHAR(50) NOT NULL,
	EUserId VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Cart PRIMARY KEY (CartId),
	CONSTRAINT FK_Cart_Product FOREIGN KEY (EUserId) REFERENCES EUser(EUserId)
)
GO

--3.6 Câu lệnh tạo bảng CartDetails(Chi tiết giỏ hàng)
CREATE TABLE CartDetails(
	CartId VARCHAR(50) NOT NULL,
	ProductId VARCHAR(50) NOT NULL,
	Quantity INT,
	CONSTRAINT PK_CartDetails PRIMARY KEY (CartId, ProductId),
	CONSTRAINT FK_CartDetails_Cart FOREIGN KEY (CartId) REFERENCES Cart(CartId),
	CONSTRAINT FK_CartDetails_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
)
GO

-- 4. Câu lệnh Insert Dữ liệu Cho các bảng

--4.1 Câu lệnh Insert dữ liệu cho bảng Category
INSERT INTO Category (CategoryId,CategoryName,DisplayOrder)
VALUES
	('C001',N'Di động',1),
	('C002',N'TV & AV',2),
	('C003',N'Màn hình hiển thị',3),
	('C004',N'Phụ kiện',4),
	('C005',N'Gia dụng',5)
GO

--4.2 Câu lệnh Insert dữ liệu cho bảng Product
INSERT INTO Product (ProductId,ProductName,ImageUrl,Price,ProductQuantity,DescriptionText,CategoryId,Discount,RealeaseDate)
VALUES
	('P001',N'Samsung Galaxy S24 AI','https://cdn2.cellphones.com.vn/x/media/catalog/product/s/a/samsung-galaxy-s24-ultra_5_.png',26990000,100,N'Samsung S24 Ultra là siêu phẩm smartphone đỉnh cao mở đầu năm 2024 đến từ nhà Samsung với chip Snapdragon 8 Gen 3 For Galaxy mạnh mẽ','C001',22,'2023-05-01'),
	('P002',N'Samsung Galaxy Z Flip 5','https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/s/a/samsung-galaxy-z-flip-5-256gb_1_5.png',21990000,100,N'Samsung S24 Ultra là siêu phẩm smartphone đỉnh cao mở đầu năm 2024 đến từ nhà Samsung với chip Snapdragon 8 Gen 3 For Galaxy mạnh mẽ','C001',10,'2023-01-01'),
	('P003',N'Samsung Galaxy S23 Ultra','https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/s/a/samsung-galaxy-s23-ultra.png',24490000,100,N'Samsung S24 Ultra là siêu phẩm smartphone đỉnh cao mở đầu năm 2024 đến từ nhà Samsung với chip Snapdragon 8 Gen 3 For Galaxy mạnh mẽ','C001',1,'2024-01-01'),
	('P004',N'Samsung Galaxy Z Fold 5','https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/s/a/samsung-galaxy-z-fold-5-256gb_1.png',36990000,100,N'Samsung S24 Ultra là siêu phẩm smartphone đỉnh cao mở đầu năm 2024 đến từ nhà Samsung với chip Snapdragon 8 Gen 3 For Galaxy mạnh mẽ','C001',10,'2020-01-01'),
	('P005',N'Samsung Galaxy S21 FE','https://cdn2.cellphones.com.vn/insecure/rs:fill:358:358/q:90/plain/https://cellphones.com.vn/media/catalog/product/s/m/sm-g990_s21fe_backfront_lv.png',9590000,100,N'Samsung S21 FE 8GB 128GB sở hữu chipset Exynos 2100 mạnh mẽ có thể chơi mượt mà, RAM 8GB và ROM 128GB cho khả năng đa nhiệm và lưu trữ ổn định. Thêm vào đó cụm camera chất lượng, cho hình ảnh sắc nét: 12MP+12MP+8MP và camera selfie 32MP. Không chỉ vậy, các phiên bản màu sắc thanh lịch, thời thượng giúp sản phẩm nổi bật hơn giữa hàng loạt các thương hiệu khác.','C001',5,'2021-01-01'),
	('P006',N'50 inch QLED QE1C 4K','https://images.samsung.com/is/image/samsung/p6pim/vn/qa50qe1cakxxv/gallery/vn-qled-q60c-473027-qa50qe1cakxxv-thumb-537625245?$172_172_PNG$',14605000,100,N'Công nghệ Motion Xcelerator cho khung hình rõ nét, Tái hiện sắc màu chân thực với công nghệ PurColor, Thưởng thức nội dung 4K chuẩn điện ảnh','C002',8,'2022-01-01'),
	('P007',N'75 inch UHD 4K AU7000','https://images.samsung.com/is/image/samsung/p6pim/vn/ua75au7000kxxv/gallery/vn-uhd-au7000-ua75au7000kxxv-thumb-408365676?$172_172_PNG$',19900000,100,N'Hiển thị 1 tỷ sắc màu với Dynamic Crystal Color, Bộ xử lý hình ảnh Crystal 4K, Cá nhân hóa kho ứng dụng giải trí','C002',19,'2022-01-01'),
	('P008',N'85 inch Crystal UHD 4K BU8000','https://images.samsung.com/is/image/samsung/p6pim/vn/qa98qn990ckxxv/gallery/vn-qled-98qn990c-qa98qn990ckxxv-thumb-538021544?$252_252_PNG$',40900000,100,N'Công nghệ Quantum Dot hiển thị 100% dải sắc màu, Công nghệ Quantum HDR, Thiết kế AirSlim','C002',20,'2022-01-01'),
	('P009',N'Màn hình viền mỏng LF24T450','https://images.samsung.com/is/image/samsung/p6pim/vn/lf24t450fqexxv/gallery/vn-led-f24t450fqe-lf24t450fqexxv-thumb-538550926?$172_172_PNG$',3990000,100,N'Chân đế chuyên dụng nâng tầm hiệu suất, Thiết kế tràn viền 3 cạnh mở rộng góc nhìn tối đa','C003',22,'2022-01-01'),
	('P010',N'Màn Hình ViewFinity S8 UHD Dòng 27 inch S80PB','https://images.samsung.com/is/image/samsung/p6pim/vn/ls27b800pxexxv/gallery/vn-s80pb-s8-27-ls27b800pxexxv-thumb-533364855?$172_172_PNG$',8989000,100,N'Tái hiện màu sắc chuyên nghiệp với dài màu mở rộng DCI-P3 98% và HDR 400, Màu sắc được cân chỉnh từ nhà máy và hỗ trợ lên đến 1 tỷ màu','C003',15,'2022-01-01'),
	('P011',N'Màn hình cong LC27R500FHE','https://images.samsung.com/is/image/samsung/p6pim/vn/lc27r500fhexxv/gallery/vn-curved-c27r500fhe-lc27r500fhexxv-thumb-535985137?$172_172_PNG$',3139000,100,N'Nâng tầm trải nghiệm với độ cong 1800R, Thiết kế tinh giản thu hút mọi ánh nhìn, Chế Độ Game tối ưu từng chi tiết hình ảnh','C003',16,'2022-01-01'),
	('P012',N'Galaxy Buds FE','https://images.samsung.com/is/image/samsung/p6pim/vn/sm-r400nzaaxxv/gallery/vn-galaxy-buds-fe-sm-r400nzaaxxv-thumb-538394933',2450000,100,N'Tai nghe với nhiều tầng âm sắc, biên độ bass phù hợp cho mọi trải nghiệm','C004',17,'2022-01-01'),
	('P013',N'Galaxy Buds2 Pro','https://images.samsung.com/is/image/samsung/p6pim/vn/2208/gallery/vn-galaxy-buds2-pro-r510-sm-r510nlvaxxv-thumb-533199212',1890000,100,N'Thiết kế công thái học giúp đeo thoải mái, Âm thanh 360 bao quanh bạn sống động như thật','C004',18,'2022-01-01'),
	('P014',N'Galaxy Buds Live','https://images.samsung.com/is/image/samsung/p6pim/vn/sm-r180nznaxxv/gallery/vn-galaxy-buds-live-r180-sm-r180nznaxxv-thumb-508264340?$172_172_PNG$',4490200,100,N'Thiết kế gọn nhẹ, Công nghệ Active Noise Cancellation (Chống ồn chủ động) cho loại mở, Âm thanh sống động và sâu lắng','C004',19,'2022-01-01'),
	('P015',N'Digital Inverter™, 243L','https://images.samsung.com/is/image/samsung/p6pim/vn/ww85t4040cx-sv/gallery/vn-front-loading-washer-ww70t4020cheo-386161-ww85t4040cx-sv-thumb-536830345?$172_172_PNG$',6490000,100,N'Công nghệ Digital Inverter™ bền bỉ, tiết kiệm điện, Bộ lọc khử mùi than hoạt tính','C005',20,'2022-01-01'),
	('P016',N'Optimal Fresh+, 305L','https://images.samsung.com/is/image/samsung/p6pim/vn/dv90t7240bb-sv/gallery/vn-dryer-dv90t7240bhs3-dv90t7240bb-sv-thumb-536831752?$172_172_PNG$',9990000,100,N'Ngăn đông mềm linh hoạt 4 chế độ Optimal Fresh+, Tăng 20L dung tích với công nghệ SpaceMax™','C005',21,'2022-01-01'),
	('P017',N'WW85T4040CX/SV, 8.5kg','https://images.samsung.com/is/image/samsung/p6pim/vn/ww90t634dle-sv/gallery/vn-front-loading-washer-ww10t634dles3-ww90t634dle-sv-thumb-536820770?$172_172_PNG$',8576304,100,N'Hygiene Steam diệt khuẩn 99,9%, ngừa dị ứng, Digital Inverter bền bỉ, êm ái 23 năm','C005',22,'2022-01-01')
GO

--4.4 Câu lệnh tạo Store Procedure mã hóa mật khẩu
CREATE OR ALTER PROC HashPwd
	@Pwd VARCHAR(100),
	@HasedPassword VARCHAR(64) OUTPUT
AS
BEGIN
	SET @HasedPassword = CONVERT(VARCHAR(64), HASHBYTES('MD5',@Pwd),2)
END
GO

--4.5 Câu lệnh Insert dữ liệu cho bảng EUser

DECLARE @HashedPwd VARCHAR(64)
EXEC HashPwd 'abc123', @HashedPwd OUTPUT
INSERT INTO EUser (EUserId,Email,Pwd,EUserName,EUserAddress,BirthDay,EUserRole,IsConfirm)
VALUES
	('EU001','chienprivate@gmail.com',@HashedPwd,N'Nguyễn Ngọc Chiến',N'50/7 Trần Quan Diệu, Quận 8, TP.HCM','2002-01-01','Admin',1),
	('EU002','chiennnps27765@gmail.com',@HashedPwd,N'Vũ Vãn Tường',N'100 Tô Hiến Thành, Quận 10, TP.HCM','2002-02-01','Customer',0),
	('EU003','anhlahb@gmail.com',@HashedPwd,N'Mai Gia Đào',N'27/6/1 Hoàng Diệu, Quận 4, TP.HCM','2002-02-01','Customer',0)
GO

--4.6 Câu lệnh Insert dữ liệu cho bảng Cart
INSERT INTO Cart(CartId,EUserId)
VALUES
	('C001','EU001'),
	('EU002','EU002')
GO

--4.7 Câu lệnh Insert dữ liệu cho bang CartDetails
INSERT INTO CartDetails(CartId,ProductId,Quantity)
VALUES
	('C001','P001',35),
	('C001','P002',35),
	('C001','P003',35),
	('C001','P004',35),
	('C001','P005',35)