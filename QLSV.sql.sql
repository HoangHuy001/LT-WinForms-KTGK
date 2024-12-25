CREATE DATABASE QuanLySV
go
USE QuanLySV
go

CREATE TABLE Lop (
    MaLop CHAR(3) PRIMARY KEY,
    TenLop NVARCHAR(30) NOT NULL
);


CREATE TABLE Sinhvien (
    MaSV CHAR(6) NOT NULL PRIMARY KEY,
    HoTenSV NVARCHAR(40),
	NgaySinh DateTime,
    MaLop CHAR(3),
    CONSTRAINT FK_Sinhvien_Lop FOREIGN KEY (MaLop) REFERENCES Lop(MaLop)
);


INSERT INTO Lop (MaLop, TenLop)
VALUES 
('L01', N'Công nghệ thông tin'),
('L02', N'Kế toán khóa 1'),
('L03', N'Kế toán khóa 1');


INSERT INTO Sinhvien (MaSV, HoTenSV,NgaySinh, MaLop)
VALUES
('SV0001', N'Trần Văn Nam','1985-08-20', 'L01'),
('SV0002', N'Nguyễn Thị Tuyết','1986-08-25', 'L02'),
('SV0003', N'Nguyễn Kim Tuyến','1984-03-21', 'L02');


-- Xem dữ liệu trong các bảng
SELECT * FROM Lop;
SELECT * FROM Sinhvien;
