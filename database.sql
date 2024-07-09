CREATE DATABASE QuanlyBanThuoc
GO 
USE QuanlyBanThuoc
GO 
-- Bảng tbl_vaitro
CREATE TABLE [dbo].[tbl_vaitro](
    [vt_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [vt_ten] [nvarchar](50) NOT NULL,
    [vt_mota] [nvarchar](max) NULL,
    [vt_ngaytao] [datetime] NULL
);
GO 
-- Bảng tbl_nguoidung
CREATE TABLE [dbo].[tbl_nguoidung](
    [nd_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [nd_tendangnhap] [nvarchar](10) NOT NULL,
    [nd_matkhau] [nvarchar](12) NOT NULL,
    [nd_ten] [nvarchar](40) NOT NULL,
    [nd_diachi] [nvarchar](70) NULL,
    [nd_sodienthoai] [nvarchar](10) NULL,
    [nd_email] [nvarchar](30) NULL,
	[nd_duongdananh] [nvarchar](max) NULL,
    [vt_ma] [uniqueidentifier] NULL,
    [nd_ngaytao] [datetime] NULL,
    FOREIGN KEY ([vt_ma]) REFERENCES [dbo].[tbl_vaitro]([vt_ma])
);
GO 
-- Bảng tbl_danhmuc
CREATE TABLE [dbo].[tbl_danhmuc](
    [dm_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [dm_ten] [nvarchar](20) NOT NULL,
    [dm_dinhdang] [nvarchar](MAX) NULL,
    [dm_ngaytao] [datetime] NULL
);
GO 
-- Bảng tbl_sanpham
CREATE TABLE [dbo].[tbl_sanpham](
    [sp_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [sp_ten] [nvarchar](30) NOT NULL,
    [sp_gia] [decimal](38, 2) NULL CHECK (sp_gia >= 0),
	[sp_donvi] [nvarchar](7) NULL,
    [sp_mota] [nvarchar](max) NULL,
	[sp_duongdananh] [nvarchar](max) NULL,
    [dm_ma] [uniqueidentifier] NULL,
    [sp_ngaytao] [datetime] NULL,
    FOREIGN KEY ([dm_ma]) REFERENCES [dbo].[tbl_danhmuc]([dm_ma])
);
GO 
-- Bảng tbl_nhasanxuat
CREATE TABLE [dbo].[tbl_nhasanxuat](
    [nsx_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [nsx_ten] [nvarchar](50) NOT NULL,
    [nsx_diachi] [nvarchar](50) NULL,
    [nsx_dienthoai] [nvarchar](10) NULL,
    [nsx_ngaytao] [datetime] NULL
);
GO 
-- Bảng tbl_khachhang
CREATE TABLE [dbo].[tbl_khachhang](
    [kh_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [kh_ten] [nvarchar](40) NOT NULL,
    [kh_diachi] [nvarchar](50) NULL,
    [kh_sodienthoai] [nvarchar](10) NULL,
    [kh_email] [nvarchar](30) NULL,
    [kh_ngaytao] [datetime] NULL
);
GO 
-- Bảng tbl_donhang
CREATE TABLE [dbo].[tbl_donhang](
    [dh_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [dh_ngaylap] [datetime] NULL,
    [dh_tongtien] [decimal](38, 2) NULL CHECK (dh_tongtien >= 0),
    [nd_ma] [uniqueidentifier] NULL,
    [kh_ma] [uniqueidentifier] NULL,
    FOREIGN KEY ([nd_ma]) REFERENCES [dbo].[tbl_nguoidung]([nd_ma]),
    FOREIGN KEY ([kh_ma]) REFERENCES [dbo].[tbl_khachhang]([kh_ma])
);
GO 
-- Bảng tbl_chitietdonhang
CREATE TABLE [dbo].[tbl_chitietdonhang](
    [ctdh_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [ctdh_soluong] [int] NULL CHECK (ctdh_soluong > 0),
    [ctdh_gia] [decimal](38, 2) NULL CHECK (ctdh_gia >= 0),
    [dh_ma] [uniqueidentifier] NULL,
    [sp_ma] [uniqueidentifier] NULL,
    [ctdh_ngaytao] [datetime] NULL,
	[ctdh_hansudung] [datetime] NULL,
	[ctdh_tttonkho] [nvarchar](7) NULL,
    FOREIGN KEY ([dh_ma]) REFERENCES [dbo].[tbl_donhang]([dh_ma]),
    FOREIGN KEY ([sp_ma]) REFERENCES [dbo].[tbl_sanpham]([sp_ma])
);
GO 
-- Bảng tbl_donhangnhap
CREATE TABLE [dbo].[tbl_donhangnhap](
    [dhn_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [dhn_ngaylap] [datetime] NULL,
    [dhn_tongtien] [decimal](38, 2) NULL CHECK (dhn_tongtien >= 0),
    [nsx_ma] [uniqueidentifier] NULL,
    FOREIGN KEY ([nsx_ma]) REFERENCES [dbo].[tbl_nhasanxuat]([nsx_ma])
);
GO 
-- Bảng tbl_chitietdonhangnhap
CREATE TABLE [dbo].[tbl_chitietdonhangnhap](
    [ctdhn_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [ctdhn_soluong] [int] NULL CHECK (ctdhn_soluong > 0),
    [ctdhn_gia] [decimal](38, 2) NULL CHECK (ctdhn_gia >= 0),
    [dhn_ma] [uniqueidentifier] NULL,
    [sp_ma] [uniqueidentifier] NULL,
    [ctdhn_ngaytao] [datetime] NULL,
	[ctdhn_hansudung] [datetime] NULL,
	[ctdhn_tttonkho] [nvarchar](7) NULL,
    FOREIGN KEY ([dhn_ma]) REFERENCES [dbo].[tbl_donhangnhap]([dhn_ma]),
    FOREIGN KEY ([sp_ma]) REFERENCES [dbo].[tbl_sanpham]([sp_ma])
);
GO 
-- Bảng tbl_phieuthu
CREATE TABLE [dbo].[tbl_phieuthu](
    [pt_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [pt_ngaylap] [datetime] NULL,
    [pt_sotien] [decimal](38, 2) NULL CHECK (pt_sotien >= 0),
    [pt_noidung] [nvarchar](max) NULL,
    [nd_ma] [uniqueidentifier] NULL,
    [dh_ma] [uniqueidentifier] NULL,
    FOREIGN KEY ([nd_ma]) REFERENCES [dbo].[tbl_nguoidung]([nd_ma]),
    FOREIGN KEY ([dh_ma]) REFERENCES [dbo].[tbl_donhang]([dh_ma])
);
GO 
-- Bảng tbl_phieuchi
CREATE TABLE [dbo].[tbl_phieuchi](
    [pc_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [pc_ngaylap] [datetime] NULL,
    [pc_sotien] [decimal](38, 2) NULL CHECK (pc_sotien >= 0),
    [pc_noidung] [nvarchar](max) NULL,
    [nd_ma] [uniqueidentifier] NULL,
    [nsx_ma] [uniqueidentifier] NULL,
    FOREIGN KEY ([nd_ma]) REFERENCES [dbo].[tbl_nguoidung]([nd_ma]),
    FOREIGN KEY ([nsx_ma]) REFERENCES [dbo].[tbl_nhasanxuat]([nsx_ma])
);
GO 
-- Bảng tbl_lichsugiaodich
CREATE TABLE [dbo].[tbl_lichsugiaodich](
    [ls_ma] [uniqueidentifier] NOT NULL PRIMARY KEY,
    [ls_loai] [nvarchar](50) NOT NULL,
    [ls_mota] [nvarchar](max) NULL,
    [ls_tongtien] [decimal](38, 2) NULL CHECK (ls_tongtien >= 0),
    [ls_thoigian] [datetime] NOT NULL DEFAULT GETDATE(),
    [nd_ma] [uniqueidentifier] NULL,
    [kh_ma] [uniqueidentifier] NULL,
    [dh_ma] [uniqueidentifier] NULL,
    [dhn_ma] [uniqueidentifier] NULL,
    [pt_ma] [uniqueidentifier] NULL,
    [pc_ma] [uniqueidentifier] NULL,
    FOREIGN KEY ([nd_ma]) REFERENCES [dbo].[tbl_nguoidung]([nd_ma]),
    FOREIGN KEY ([kh_ma]) REFERENCES [dbo].[tbl_khachhang]([kh_ma]),
    FOREIGN KEY ([dh_ma]) REFERENCES [dbo].[tbl_donhang]([dh_ma]),
    FOREIGN KEY ([dhn_ma]) REFERENCES [dbo].[tbl_donhangnhap]([dhn_ma]),
    FOREIGN KEY ([pt_ma]) REFERENCES [dbo].[tbl_phieuthu]([pt_ma]),
    FOREIGN KEY ([pc_ma]) REFERENCES [dbo].[tbl_phieuchi]([pc_ma])
);
GO 
