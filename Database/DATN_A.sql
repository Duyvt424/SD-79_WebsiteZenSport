IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'DATN_A')
BEGIN
    CREATE DATABASE [DATN_A];
END
GO

USE [DATN_A]
GO
/****** Object:  Table [dbo].[Address]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Address](
	[AddressID] [uniqueidentifier] NOT NULL,
	[Street] [nvarchar](200) NULL,
	[Commune] [nvarchar](200) NULL,
	[District] [nvarchar](200) NULL,
	[Province] [nvarchar](200) NULL,
	[IsDefaultAddress] [bit] NOT NULL,
	[ShippingCost] [decimal](18, 2) NOT NULL,
	[DistrictId] [int] NOT NULL,
	[WardCode] [int] NOT NULL,
	[ShippingMethodID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CumstomerID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Address] PRIMARY KEY CLUSTERED 
(
	[AddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[BillID] [uniqueidentifier] NOT NULL,
	[BillCode] [nvarchar](100) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ConfirmationDate] [datetime] NOT NULL,
	[DeliveryDate] [datetime] NOT NULL,
	[SuccessDate] [datetime] NOT NULL,
	[CancelDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[PaymentDay] [datetime] NOT NULL,
	[TotalPrice] [decimal](18, 2) NOT NULL,
	[ShippingCosts] [decimal](18, 2) NOT NULL,
	[TotalPriceAfterDiscount] [decimal](18, 2) NOT NULL,
	[Note] [nvarchar](500) NOT NULL,
	[IsPaid] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[VoucherID] [uniqueidentifier] NULL,
	[ShippingVoucherID] [uniqueidentifier] NULL,
	[EmployeeID] [uniqueidentifier] NULL,
	[PurchaseMethodID] [uniqueidentifier] NOT NULL,
	[AddressID] [uniqueidentifier] NULL,
	[TransactionType] [int] NOT NULL,
	[TotalRefundAmount] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Bill] PRIMARY KEY CLUSTERED 
(
	[BillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[ID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[ShoesDetails_SizeID] [uniqueidentifier] NOT NULL,
	[BillID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BillDetails] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillStatusHistory]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillStatusHistory](
	[BillStatusHistoryID] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ChangeDate] [datetime] NOT NULL,
	[Note] [nvarchar](1000) NOT NULL,
	[BillID] [uniqueidentifier] NOT NULL,
	[EmployeeID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_BillStatusHistory] PRIMARY KEY CLUSTERED 
(
	[BillStatusHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CumstomerID] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED 
(
	[CumstomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartDetails]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartDetails](
	[CartDetailsId] [uniqueidentifier] NOT NULL,
	[CumstomerID] [uniqueidentifier] NOT NULL,
	[ShoesDetails_SizeID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_CartDetails] PRIMARY KEY CLUSTERED 
(
	[CartDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Color]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Color](
	[ColorID] [uniqueidentifier] NOT NULL,
	[ColorCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED 
(
	[ColorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CumstomerID] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](100) NULL,
	[UserName] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Email] [nvarchar](300) NULL,
	[Sex] [int] NOT NULL,
	[ResetPassword] [nvarchar](60) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[RankID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CumstomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[EmployeeID] [uniqueidentifier] NOT NULL,
	[Image] [nvarchar](1000) NULL,
	[IdentificationCode] [nvarchar](50) NULL,
	[FullName] [nvarchar](100) NULL,
	[UserName] [nvarchar](100) NULL,
	[Password] [nvarchar](100) NULL,
	[Email] [nvarchar](300) NULL,
	[Sex] [int] NOT NULL,
	[Address] [nvarchar](1000) NULL,
	[ResetPassword] [nvarchar](60) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FavoriteShoes]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FavoriteShoes](
	[ShoesDetails_SizeId] [uniqueidentifier] NOT NULL,
	[CumstomerID] [uniqueidentifier] NOT NULL,
	[FavoriteShoesID] [uniqueidentifier] NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_FavoriteShoes] PRIMARY KEY CLUSTERED 
(
	[FavoriteShoesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Image]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Image](
	[ImageID] [uniqueidentifier] NOT NULL,
	[ImageCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Image1] [nvarchar](1000) NULL,
	[Image2] [nvarchar](1000) NULL,
	[Image3] [nvarchar](1000) NULL,
	[Image4] [nvarchar](1000) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[ShoesDetailsID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED 
(
	[ImageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Material]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Material](
	[MaterialId] [uniqueidentifier] NOT NULL,
	[MaterialCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[MaterialId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [uniqueidentifier] NOT NULL,
	[ProductCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[SupplierID] [uniqueidentifier] NOT NULL,
	[MaterialId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseMethod]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseMethod](
	[PurchaseMethodID] [uniqueidentifier] NOT NULL,
	[MethodName] [nvarchar](200) NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_PurchaseMethod] PRIMARY KEY CLUSTERED 
(
	[PurchaseMethodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rank]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rank](
	[RankID] [uniqueidentifier] NOT NULL,
	[RankCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Desciption] [nvarchar](500) NOT NULL,
	[ThresholdAmount] [decimal](18, 2) NOT NULL,
	[ReducedValue] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Rank] PRIMARY KEY CLUSTERED 
(
	[RankID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnedProducts]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnedProducts](
	[ID] [uniqueidentifier] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Note] [nvarchar](1000) NOT NULL,
	[QuantityReturned] [int] NOT NULL,
	[ReturnedPrice] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[BillId] [uniqueidentifier] NOT NULL,
	[ShoesDetails_SizeID] [uniqueidentifier] NULL,
	[NamePurChaseMethod] [nvarchar](1000) NOT NULL,
	[TransactionType] [int] NOT NULL,
	[InitialProductTotalPrice] [decimal](18, 2) NULL,
	[ShippingFeeReturned] [decimal](18, 2) NULL,
	[Image1] [nvarchar](1000) NULL,
	[Image2] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ReturnedProducts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleID] [uniqueidentifier] NOT NULL,
	[RoleCode] [nvarchar](50) NULL,
	[RoleName] [nvarchar](100) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sex]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sex](
	[SexID] [uniqueidentifier] NOT NULL,
	[SexName] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Sex] PRIMARY KEY CLUSTERED 
(
	[SexID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingVoucher]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingVoucher](
	[ShippingVoucherID] [uniqueidentifier] NOT NULL,
	[VoucherShipCode] [nvarchar](100) NULL,
	[MaxShippingDiscount] [decimal](18, 2) NULL,
	[ShippingDiscount] [decimal](18, 2) NULL,
	[QuantityShip] [int] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[IsShippingVoucher] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_ShippingVoucher] PRIMARY KEY CLUSTERED 
(
	[ShippingVoucherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoesDetails]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoesDetails](
	[ShoesDetailsId] [uniqueidentifier] NOT NULL,
	[ShoesDetailsCode] [nvarchar](100) NULL,
	[DateCreated] [datetime] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[ImportPrice] [decimal](18, 2) NOT NULL,
	[Description] [nvarchar](1000) NULL,
	[Status] [int] NOT NULL,
	[ColorID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[SoleID] [uniqueidentifier] NOT NULL,
	[StyleID] [uniqueidentifier] NOT NULL,
	[SexID] [uniqueidentifier] NOT NULL,
	[ImageUrl] [nvarchar](1000) NULL,
 CONSTRAINT [PK_ShoesDetails] PRIMARY KEY CLUSTERED 
(
	[ShoesDetailsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShoesDetails_Size]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShoesDetails_Size](
	[ID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ShoesDetailsId] [uniqueidentifier] NOT NULL,
	[SizeID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ShoesDetails_Size] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Size]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[SizeID] [uniqueidentifier] NOT NULL,
	[SizeCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Size] PRIMARY KEY CLUSTERED 
(
	[SizeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sole]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sole](
	[SoleID] [uniqueidentifier] NOT NULL,
	[SoleCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Height] [decimal](18, 2) NOT NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Sole] PRIMARY KEY CLUSTERED 
(
	[SoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Style]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Style](
	[StyleID] [uniqueidentifier] NOT NULL,
	[StyleCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Style] PRIMARY KEY CLUSTERED 
(
	[StyleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierID] [uniqueidentifier] NOT NULL,
	[SupplierCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Status] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Supplier] PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Voucher]    Script Date: 27/03/2025 14:14:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Voucher](
	[VoucherID] [uniqueidentifier] NOT NULL,
	[VoucherCode] [nvarchar](100) NULL,
	[VoucherValue] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Exclusiveright] [nvarchar](100) NULL,
	[MaxUsage] [int] NOT NULL,
	[Type] [int] NULL,
	[RemainingUsage] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[IsDel] [bit] NOT NULL,
	[UserNameCustomer] [nvarchar](100) NULL,
	[DateCreated] [datetime] NOT NULL,
 CONSTRAINT [PK_Voucher] PRIMARY KEY CLUSTERED 
(
	[VoucherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Address] ([AddressID], [Street], [Commune], [District], [Province], [IsDefaultAddress], [ShippingCost], [DistrictId], [WardCode], [ShippingMethodID], [Status], [DateCreated], [CumstomerID]) VALUES (N'23667781-9447-4e00-a99e-5e94cd7ab4ec', N'aa', N'Xã Định Hiệp', N'Huyện Dầu Tiếng', N'Bình Dương', 1, CAST(78999.00 AS Decimal(18, 2)), 1746, 440704, 53322, 0, CAST(N'2025-03-05T08:33:14.000' AS DateTime), N'fddd9e51-a776-4fc2-a643-1b29fb4d1973')
INSERT [dbo].[Address] ([AddressID], [Street], [Commune], [District], [Province], [IsDefaultAddress], [ShippingCost], [DistrictId], [WardCode], [ShippingMethodID], [Status], [DateCreated], [CumstomerID]) VALUES (N'6b2a0c10-9437-4d37-8632-76703bc395c2', N'aa', N'Phường Dĩ An', N'Thành phố Dĩ An', N'Bình Dương', 1, CAST(56499.00 AS Decimal(18, 2)), 1540, 440504, 53322, 0, CAST(N'2025-02-21T16:16:48.000' AS DateTime), N'8a03d765-a1ad-4c55-a304-7c0f13e562c8')
INSERT [dbo].[Address] ([AddressID], [Street], [Commune], [District], [Province], [IsDefaultAddress], [ShippingCost], [DistrictId], [WardCode], [ShippingMethodID], [Status], [DateCreated], [CumstomerID]) VALUES (N'a4166cb6-58a7-49a6-bfe6-ae8d3c700481', N'aa', N'Xã Hùng Xuyên', N'Huyện Đoan Hùng', N'Phú Thọ', 0, CAST(54001.00 AS Decimal(18, 2)), 1925, 800106, 53321, 0, CAST(N'2025-02-21T16:16:25.000' AS DateTime), N'8a03d765-a1ad-4c55-a304-7c0f13e562c8')
INSERT [dbo].[Address] ([AddressID], [Street], [Commune], [District], [Province], [IsDefaultAddress], [ShippingCost], [DistrictId], [WardCode], [ShippingMethodID], [Status], [DateCreated], [CumstomerID]) VALUES (N'eeb377c6-184c-466a-8c30-d9d6a07b6400', N'aa', N'Xã Đại Đồng', N'Huyện Tiên Du', N'Bắc Ninh', 0, CAST(49001.00 AS Decimal(18, 2)), 1729, 190403, 53321, 0, CAST(N'2024-12-11T09:32:49.000' AS DateTime), N'fddd9e51-a776-4fc2-a643-1b29fb4d1973')
GO
INSERT [dbo].[Bill] ([BillID], [BillCode], [CreateDate], [ConfirmationDate], [DeliveryDate], [SuccessDate], [CancelDate], [UpdateDate], [PaymentDay], [TotalPrice], [ShippingCosts], [TotalPriceAfterDiscount], [Note], [IsPaid], [Status], [CustomerID], [VoucherID], [ShippingVoucherID], [EmployeeID], [PurchaseMethodID], [AddressID], [TransactionType], [TotalRefundAmount]) VALUES (N'35f5c852-12b8-43e5-af43-1473162a254d', N'20250221161711018', CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(N'2025-02-25T00:00:00.000' AS DateTime), CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(N'2025-02-21T16:17:11.020' AS DateTime), CAST(2556499.00 AS Decimal(18, 2)), CAST(56499.00 AS Decimal(18, 2)), CAST(2556499.00 AS Decimal(18, 2)), N'', 0, 0, N'8a03d765-a1ad-4c55-a304-7c0f13e562c8', NULL, NULL, NULL, N'1947fa1a-b18e-4976-6566-08dbcafa51e6', N'6b2a0c10-9437-4d37-8632-76703bc395c2', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bill] ([BillID], [BillCode], [CreateDate], [ConfirmationDate], [DeliveryDate], [SuccessDate], [CancelDate], [UpdateDate], [PaymentDay], [TotalPrice], [ShippingCosts], [TotalPriceAfterDiscount], [Note], [IsPaid], [Status], [CustomerID], [VoucherID], [ShippingVoucherID], [EmployeeID], [PurchaseMethodID], [AddressID], [TransactionType], [TotalRefundAmount]) VALUES (N'fb23613b-d934-4893-a005-66e9e9d541c0', N'20241211093419562', CAST(N'2024-12-11T09:34:19.563' AS DateTime), CAST(N'2024-12-11T09:34:19.563' AS DateTime), CAST(N'2024-12-11T09:34:19.563' AS DateTime), CAST(N'2024-12-13T00:00:00.000' AS DateTime), CAST(N'2024-12-11T09:34:19.563' AS DateTime), CAST(N'2024-12-11T09:34:19.563' AS DateTime), CAST(N'2024-12-11T09:54:02.973' AS DateTime), CAST(2549001.00 AS Decimal(18, 2)), CAST(49001.00 AS Decimal(18, 2)), CAST(2549001.00 AS Decimal(18, 2)), N'', 1, 0, N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', NULL, NULL, NULL, N'e257a08e-b7dc-44b6-6567-08dbcafa51e6', N'eeb377c6-184c-466a-8c30-d9d6a07b6400', 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Bill] ([BillID], [BillCode], [CreateDate], [ConfirmationDate], [DeliveryDate], [SuccessDate], [CancelDate], [UpdateDate], [PaymentDay], [TotalPrice], [ShippingCosts], [TotalPriceAfterDiscount], [Note], [IsPaid], [Status], [CustomerID], [VoucherID], [ShippingVoucherID], [EmployeeID], [PurchaseMethodID], [AddressID], [TransactionType], [TotalRefundAmount]) VALUES (N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'20250305083211416', CAST(N'2025-03-05T08:32:11.417' AS DateTime), CAST(N'2025-03-05T08:33:29.053' AS DateTime), CAST(N'2025-03-05T08:33:39.233' AS DateTime), CAST(N'2025-03-05T08:33:56.130' AS DateTime), CAST(N'2025-03-05T08:32:11.417' AS DateTime), CAST(N'2025-03-05T08:32:51.243' AS DateTime), CAST(N'2025-03-05T08:33:47.153' AS DateTime), CAST(2578999.00 AS Decimal(18, 2)), CAST(78999.00 AS Decimal(18, 2)), CAST(2578999.00 AS Decimal(18, 2)), N'', 1, 6, N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', NULL, NULL, N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5', N'1947fa1a-b18e-4976-6566-08dbcafa51e6', N'23667781-9447-4e00-a99e-5e94cd7ab4ec', 0, CAST(2500000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[BillDetails] ([ID], [Quantity], [Price], [Status], [ShoesDetails_SizeID], [BillID]) VALUES (N'eecdce5a-b06b-4238-8a16-4a1922fec2cb', 1, CAST(2500000.00 AS Decimal(18, 2)), 0, N'2f403e4b-f6c4-44b6-8842-08dc028ff57a', N'013c969d-d7f2-45b8-8f29-c9f011b723ea')
INSERT [dbo].[BillDetails] ([ID], [Quantity], [Price], [Status], [ShoesDetails_SizeID], [BillID]) VALUES (N'badefe36-696c-41fc-a84c-8b43c2838a8a', 1, CAST(2500000.00 AS Decimal(18, 2)), 0, N'2f403e4b-f6c4-44b6-8842-08dc028ff57a', N'35f5c852-12b8-43e5-af43-1473162a254d')
INSERT [dbo].[BillDetails] ([ID], [Quantity], [Price], [Status], [ShoesDetails_SizeID], [BillID]) VALUES (N'1a3692ae-7559-40cd-aa1a-f133641695b3', 1, CAST(2500000.00 AS Decimal(18, 2)), 0, N'2f403e4b-f6c4-44b6-8842-08dc028ff57a', N'fb23613b-d934-4893-a005-66e9e9d541c0')
GO
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'506efe57-6ca7-4a48-a5e5-2164163edebb', 5, CAST(N'2025-03-05T08:32:51.243' AS DateTime), N'A sản phẩm [Nike SB Zoom Blazer Low Pro GT] - kích cỡ [37] với số lượng: [2]', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5')
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'f1154b00-9144-4c0a-a77e-3fbedd59a48a', 0, CAST(N'2025-02-21T16:17:11.053' AS DateTime), N'Người mua tạo đơn hàng', N'35f5c852-12b8-43e5-af43-1473162a254d', NULL)
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'eedd9227-5641-4d7d-9182-53bbead81f2b', 0, CAST(N'2025-03-05T08:32:11.460' AS DateTime), N'Người mua tạo đơn hàng', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', NULL)
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'3249988f-f34b-4918-b470-5a2c080738b8', 1, CAST(N'2025-03-05T08:33:29.053' AS DateTime), N'Ok', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5')
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'9340e4f3-b209-4589-9b84-77d3e05d62f2', 2, CAST(N'2025-03-05T08:33:39.233' AS DateTime), N'A', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5')
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'c99fbad3-dbef-4ae7-a400-b4c6ff5594e1', 3, CAST(N'2025-03-05T08:33:56.130' AS DateTime), N'ok', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5')
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'4cfbf257-a8b8-4b09-b6f7-f2210cb5194b', 0, CAST(N'2024-12-11T09:34:19.597' AS DateTime), N'Người mua tạo đơn hàng', N'fb23613b-d934-4893-a005-66e9e9d541c0', NULL)
INSERT [dbo].[BillStatusHistory] ([BillStatusHistoryID], [Status], [ChangeDate], [Note], [BillID], [EmployeeID]) VALUES (N'9bb0e30c-7b62-41c1-a384-fad23af375bf', 6, CAST(N'2025-03-05T08:40:59.327' AS DateTime), N'Đã hoàn trả [1] sản phẩm [Nike SB Zoom Blazer Low Pro GT] kích cỡ [37]. Lý do: [Sản phẩm không phù hợp với mô tả]', N'013c969d-d7f2-45b8-8f29-c9f011b723ea', NULL)
GO
INSERT [dbo].[Cart] ([CumstomerID], [Description]) VALUES (N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', N'Updated cart description')
INSERT [dbo].[Cart] ([CumstomerID], [Description]) VALUES (N'71b92f13-3ff0-4d10-a37c-3317ad0713e9', N'Cart for logged in user')
INSERT [dbo].[Cart] ([CumstomerID], [Description]) VALUES (N'8a03d765-a1ad-4c55-a304-7c0f13e562c8', N'Cart for logged in user')
GO
INSERT [dbo].[CartDetails] ([CartDetailsId], [CumstomerID], [ShoesDetails_SizeID], [Quantity]) VALUES (N'cc9de08d-27b8-4c79-b701-00e398393d59', N'71b92f13-3ff0-4d10-a37c-3317ad0713e9', N'ca4e156f-4545-488c-8840-08dc028ff57a', 1)
GO
INSERT [dbo].[Color] ([ColorID], [ColorCode], [Name], [Status], [DateCreated]) VALUES (N'9a4bbfa8-4ba1-45cf-b68b-30d50570abaa', N'CL001', N'Red', 0, CAST(N'2023-10-25T09:21:00.000' AS DateTime))
INSERT [dbo].[Color] ([ColorID], [ColorCode], [Name], [Status], [DateCreated]) VALUES (N'b449bf0c-15c0-435f-afda-404e470a8eac', N'CL002', N'Yellow', 0, CAST(N'2025-03-17T08:29:00.000' AS DateTime))
GO
INSERT [dbo].[Customer] ([CumstomerID], [FullName], [UserName], [Password], [Email], [Sex], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RankID]) VALUES (N'5c5bb7d7-bdd6-498a-825e-1346a1863590', N'Duy Vu', N'Duy', N'115601877290423916382', N'vuduy16102003@gmail.com', 2, N'', N'', 2, CAST(N'2024-12-25T08:29:46.943' AS DateTime), N'23eb6198-74c2-4c4c-92a5-13d2f80c5e97')
INSERT [dbo].[Customer] ([CumstomerID], [FullName], [UserName], [Password], [Email], [Sex], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RankID]) VALUES (N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', N'Vũ Ngân Hà', N'Vũ', N'111711223198212655710', N'duyvt424@gmail.com', 2, N'', N'0985887953', 2, CAST(N'2024-12-11T09:32:26.777' AS DateTime), N'23eb6198-74c2-4c4c-92a5-13d2f80c5e97')
INSERT [dbo].[Customer] ([CumstomerID], [FullName], [UserName], [Password], [Email], [Sex], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RankID]) VALUES (N'71b92f13-3ff0-4d10-a37c-3317ad0713e9', N'Duy Vũ Tường', N'Duy', N'110840378590600817270', N'vuduy10a7@gmail.com', 2, N'', N'', 2, CAST(N'2025-03-12T15:06:33.297' AS DateTime), N'23eb6198-74c2-4c4c-92a5-13d2f80c5e97')
INSERT [dbo].[Customer] ([CumstomerID], [FullName], [UserName], [Password], [Email], [Sex], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RankID]) VALUES (N'8a03d765-a1ad-4c55-a304-7c0f13e562c8', N'Vũ Ngân Hà', N'Vu Tuong Duy', N'110749865769935614589', N'duyvtph24890@fpt.edu.vn', 2, N'', N'0398759424', 2, CAST(N'2025-02-21T16:15:32.300' AS DateTime), N'23eb6198-74c2-4c4c-92a5-13d2f80c5e97')
GO
INSERT [dbo].[Employee] ([EmployeeID], [Image], [IdentificationCode], [FullName], [UserName], [Password], [Email], [Sex], [Address], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RoleID]) VALUES (N'ca416ca9-40f9-4c96-1867-08dbdf6c81c5', N'z5014761180071_f0f1df358c771a98700e2e33ce3dc554-fotor-bg-remover-2023122717349.png', N'0', N'a', N'aa', N'1', N'a', 0, N'a', N'66', N'0909', 0, CAST(N'2023-12-22T00:00:00.000' AS DateTime), N'1171fcae-d539-4cd7-9ab7-1c0b81082dca')
INSERT [dbo].[Employee] ([EmployeeID], [Image], [IdentificationCode], [FullName], [UserName], [Password], [Email], [Sex], [Address], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RoleID]) VALUES (N'ca416ca9-40f9-4c96-1867-08dbdf6c81c6', N'z5014761180071_f0f1df358c771a98700e2e33ce3dc554-fotor-bg-remover-2023122717349.png', N'0', N'a', N'duy', N'a', N'a', 0, N'a', N'66', N'0909', 0, CAST(N'2023-12-22T00:00:00.000' AS DateTime), N'46631fbb-e268-44f5-8225-7ed7c7e76490')
INSERT [dbo].[Employee] ([EmployeeID], [Image], [IdentificationCode], [FullName], [UserName], [Password], [Email], [Sex], [Address], [ResetPassword], [PhoneNumber], [Status], [DateCreated], [RoleID]) VALUES (N'afd889b2-7970-4889-99d9-89abbacbc4f2', N'hoanhok1.png', N'fdgfcvdfbg', N'AA', N'aa1', N'a', N'cs@vb.cn', 2, N'fdgfsdfghj', N'0', N'09876543678', 0, CAST(N'2024-01-15T14:28:48.000' AS DateTime), N'46631fbb-e268-44f5-8225-7ed7c7e76490')
GO
INSERT [dbo].[FavoriteShoes] ([ShoesDetails_SizeId], [CumstomerID], [FavoriteShoesID], [AddedDate], [Status]) VALUES (N'd5531dec-68cd-4390-8841-08dc028ff57a', N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', N'ee387cef-53c3-47de-bb2e-08f5c8e993f5', CAST(N'2024-12-30T00:00:00.000' AS DateTime), 1)
INSERT [dbo].[FavoriteShoes] ([ShoesDetails_SizeId], [CumstomerID], [FavoriteShoesID], [AddedDate], [Status]) VALUES (N'd5531dec-68cd-4390-8841-08dc028ff57a', N'fddd9e51-a776-4fc2-a643-1b29fb4d1973', N'78c6090e-2ce3-4185-b23a-b6eb6044fe86', CAST(N'2025-01-01T00:00:00.000' AS DateTime), 1)
GO
INSERT [dbo].[Image] ([ImageID], [ImageCode], [Name], [Image1], [Image2], [Image3], [Image4], [Status], [DateCreated], [ShoesDetailsID]) VALUES (N'59ad1b3a-eb5f-498c-a6ad-b2ff59090c9b', N'IG003', N'Image 3', N'shoes9.png', N'shoes10.png', N'shoes11.png', N'shoes12.png', 0, CAST(N'2023-12-29T14:48:00.000' AS DateTime), N'46631fbb-e268-44f5-8225-7ed7c7e76492')
INSERT [dbo].[Image] ([ImageID], [ImageCode], [Name], [Image1], [Image2], [Image3], [Image4], [Status], [DateCreated], [ShoesDetailsID]) VALUES (N'5f1313a9-1a9d-4956-b6fd-bac902c76569', N'IG001', N'Image 1', N'shoes5.png', N'shoes6.png', N'shoes7.png', N'shoes8.png', 0, CAST(N'2023-12-22T08:46:00.000' AS DateTime), N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc')
INSERT [dbo].[Image] ([ImageID], [ImageCode], [Name], [Image1], [Image2], [Image3], [Image4], [Status], [DateCreated], [ShoesDetailsID]) VALUES (N'41cbd8df-5e01-40cb-b7ac-cffd6ee9403a', N'IG002', N'Image 2', N'shoes17.png', N'shoes18.png', N'shoes19.png', N'shoes20.png', 0, CAST(N'2023-12-22T08:47:00.000' AS DateTime), N'46631fbb-e268-44f5-8225-7ed7c7e76491')
GO
INSERT [dbo].[Material] ([MaterialId], [MaterialCode], [Name], [Status], [DateCreated]) VALUES (N'b10982f4-e57d-4887-a637-0b00836cf2a1', N'MT002', N'Nhựa dẻo', 0, CAST(N'2023-10-10T17:11:00.000' AS DateTime))
INSERT [dbo].[Material] ([MaterialId], [MaterialCode], [Name], [Status], [DateCreated]) VALUES (N'8e3bb435-e13e-4206-9892-46fe6eac4492', N'MT003', N'Vải', 0, CAST(N'2023-10-10T17:12:00.000' AS DateTime))
INSERT [dbo].[Material] ([MaterialId], [MaterialCode], [Name], [Status], [DateCreated]) VALUES (N'b8210644-9250-413a-8a74-d18d65974638', N'MT001', N'Cao su', 0, CAST(N'2023-10-10T17:11:00.000' AS DateTime))
GO
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'589a7280-5358-4cf6-b4ba-1c22685f50c0', N'PD004', N'Nike Burrow', 0, CAST(N'2023-10-25T09:38:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'8e3bb435-e13e-4206-9892-46fe6eac4492')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'a9f85f43-c6f8-4c23-9255-2a2b18733f2c', N'PD006', N'Nike SB Zoom Blazer Low Pro GT', 0, CAST(N'2023-10-25T09:48:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'b10982f4-e57d-4887-a637-0b00836cf2a1')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'a58320dc-26eb-420a-803b-2cdb201c04dd', N'PD005', N'NikeCourt Air Zoom Vapor 9.5', 0, CAST(N'2023-10-25T09:44:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'b8210644-9250-413a-8a74-d18d65974638')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'f458ba7d-b29e-486f-a7f2-3ba45fc91925', N'PD001', N'Nike Waffle Debut', 0, CAST(N'2023-10-25T09:20:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'b10982f4-e57d-4887-a637-0b00836cf2a1')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'f7d96569-1798-46d3-abd1-4ed10fe3eeb0', N'PD002', N'Nike Air Force 1 ''07', 0, CAST(N'2023-10-25T09:27:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'b10982f4-e57d-4887-a637-0b00836cf2a1')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'f6c316bc-c9c7-477e-b120-675f658d00ad', N'PD008', N'Shoes Duy', 0, CAST(N'2023-10-25T09:55:00.000' AS DateTime), N'eefb8e43-4beb-403b-a470-9a47e5c36f27', N'b10982f4-e57d-4887-a637-0b00836cf2a1')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'960d2c89-496d-4fc4-87d9-b626c447a409', N'PD003', N'Nike Air Max Bliss', 0, CAST(N'2023-10-25T09:34:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'b10982f4-e57d-4887-a637-0b00836cf2a1')
INSERT [dbo].[Product] ([ProductID], [ProductCode], [Name], [Status], [DateCreated], [SupplierID], [MaterialId]) VALUES (N'b94b580e-1519-43a8-a12a-c9b0a76658e7', N'PD007', N'Nike Metcon 9', 0, CAST(N'2023-10-25T09:52:00.000' AS DateTime), N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'8e3bb435-e13e-4206-9892-46fe6eac4492')
GO
INSERT [dbo].[PurchaseMethod] ([PurchaseMethodID], [MethodName], [Status], [DateCreated]) VALUES (N'1947fa1a-b18e-4976-6566-08dbcafa51e6', N'Thanh toán khi nhận hàng', 0, CAST(N'2023-10-12T15:07:00.000' AS DateTime))
INSERT [dbo].[PurchaseMethod] ([PurchaseMethodID], [MethodName], [Status], [DateCreated]) VALUES (N'e257a08e-b7dc-44b6-6567-08dbcafa51e6', N'Thanh toán trực tuyến VNPay', 0, CAST(N'2023-10-12T15:07:00.000' AS DateTime))
GO
INSERT [dbo].[Rank] ([RankID], [RankCode], [Name], [Desciption], [ThresholdAmount], [ReducedValue], [Status], [DateCreated]) VALUES (N'23eb6198-74c2-4c4c-92a5-13d2f80c5e94', N'RK001', N'Hạng vàng', N'Khách hàng rank đồng', CAST(1500000.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), 0, CAST(N'2023-10-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Rank] ([RankID], [RankCode], [Name], [Desciption], [ThresholdAmount], [ReducedValue], [Status], [DateCreated]) VALUES (N'23eb6198-74c2-4c4c-92a5-13d2f80c5e95', N'RK002', N'Hạng bạc', N'A', CAST(3000000.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), 0, CAST(N'2023-12-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Rank] ([RankID], [RankCode], [Name], [Desciption], [ThresholdAmount], [ReducedValue], [Status], [DateCreated]) VALUES (N'23eb6198-74c2-4c4c-92a5-13d2f80c5e96', N'RK003', N'Hạng kim cương', N'A', CAST(5000000.00 AS Decimal(18, 2)), CAST(7.00 AS Decimal(18, 2)), 0, CAST(N'2023-12-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Rank] ([RankID], [RankCode], [Name], [Desciption], [ThresholdAmount], [ReducedValue], [Status], [DateCreated]) VALUES (N'23eb6198-74c2-4c4c-92a5-13d2f80c5e97', N'RK004', N'Không', N'0', CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, CAST(N'2023-12-18T00:00:00.000' AS DateTime))
GO
INSERT [dbo].[ReturnedProducts] ([ID], [CreateDate], [Note], [QuantityReturned], [ReturnedPrice], [Status], [BillId], [ShoesDetails_SizeID], [NamePurChaseMethod], [TransactionType], [InitialProductTotalPrice], [ShippingFeeReturned], [Image1], [Image2]) VALUES (N'b3db1384-cd54-4cb5-bf88-31d3b139707d', CAST(N'2025-03-05T08:42:13.653' AS DateTime), N'Hoàn tiền cho khách ', 1, CAST(2500000.00 AS Decimal(18, 2)), 0, N'013c969d-d7f2-45b8-8f29-c9f011b723ea', N'2f403e4b-f6c4-44b6-8842-08dc028ff57a', N'Tiền mặt', 1, CAST(5000000.00 AS Decimal(18, 2)), CAST(66499.00 AS Decimal(18, 2)), N'Screenshot 2025-03-01 090047.png', N'z6329862595134_53e65ebc3db736ceacba395aa104c36c.jpg')
INSERT [dbo].[ReturnedProducts] ([ID], [CreateDate], [Note], [QuantityReturned], [ReturnedPrice], [Status], [BillId], [ShoesDetails_SizeID], [NamePurChaseMethod], [TransactionType], [InitialProductTotalPrice], [ShippingFeeReturned], [Image1], [Image2]) VALUES (N'0febf645-e9d4-435b-8c29-fec7a43a44a2', CAST(N'2025-03-05T08:33:47.157' AS DateTime), N'ok', 0, CAST(5078999.00 AS Decimal(18, 2)), 0, N'013c969d-d7f2-45b8-8f29-c9f011b723ea', NULL, N'Thanh toán khi nhận hàng', 0, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Role] ([RoleID], [RoleCode], [RoleName], [Status], [DateCreated]) VALUES (N'1171fcae-d539-4cd7-9ab7-1c0b81082dca', N'RO002', N'Quản lý', 0, CAST(N'2023-12-10T15:08:00.000' AS DateTime))
INSERT [dbo].[Role] ([RoleID], [RoleCode], [RoleName], [Status], [DateCreated]) VALUES (N'46631fbb-e268-44f5-8225-7ed7c7e76490', N'RO001', N'Nhân viên', 0, CAST(N'2023-12-10T15:08:00.000' AS DateTime))
GO
INSERT [dbo].[Sex] ([SexID], [SexName]) VALUES (N'46631fbb-e268-44f5-8225-7ed7c7e76492', N'Nam')
INSERT [dbo].[Sex] ([SexID], [SexName]) VALUES (N'46631fbb-e268-44f5-8225-7ed7c7e76493', N'Nữ')
GO
INSERT [dbo].[ShoesDetails] ([ShoesDetailsId], [ShoesDetailsCode], [DateCreated], [Price], [ImportPrice], [Description], [Status], [ColorID], [ProductID], [SoleID], [StyleID], [SexID], [ImageUrl]) VALUES (N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'SDT001', CAST(N'2023-12-22T00:00:00.000' AS DateTime), CAST(2500000.00 AS Decimal(18, 2)), CAST(1900000.00 AS Decimal(18, 2)), N'Men''s shoes', 0, N'9a4bbfa8-4ba1-45cf-b68b-30d50570abaa', N'a9f85f43-c6f8-4c23-9255-2a2b18733f2c', N'05590ec8-fe6a-49f9-86f5-a9266078b220', N'd13546bc-5117-4342-b00e-2d2b3ebf90fe', N'46631fbb-e268-44f5-8225-7ed7c7e76492', NULL)
INSERT [dbo].[ShoesDetails] ([ShoesDetailsId], [ShoesDetailsCode], [DateCreated], [Price], [ImportPrice], [Description], [Status], [ColorID], [ProductID], [SoleID], [StyleID], [SexID], [ImageUrl]) VALUES (N'46631fbb-e268-44f5-8225-7ed7c7e76491', N'SDT002', CAST(N'2023-12-22T00:00:00.000' AS DateTime), CAST(3200000.00 AS Decimal(18, 2)), CAST(1900000.00 AS Decimal(18, 2)), N'Men''s shoes', 0, N'9a4bbfa8-4ba1-45cf-b68b-30d50570abaa', N'f458ba7d-b29e-486f-a7f2-3ba45fc91925', N'05590ec8-fe6a-49f9-86f5-a9266078b220', N'd13546bc-5117-4342-b00e-2d2b3ebf90fe', N'46631fbb-e268-44f5-8225-7ed7c7e76492', NULL)
INSERT [dbo].[ShoesDetails] ([ShoesDetailsId], [ShoesDetailsCode], [DateCreated], [Price], [ImportPrice], [Description], [Status], [ColorID], [ProductID], [SoleID], [StyleID], [SexID], [ImageUrl]) VALUES (N'46631fbb-e268-44f5-8225-7ed7c7e76492', N'SDT003', CAST(N'2023-12-29T00:00:00.000' AS DateTime), CAST(4700000.00 AS Decimal(18, 2)), CAST(3400000.00 AS Decimal(18, 2)), N'Men''s shoes', 0, N'9a4bbfa8-4ba1-45cf-b68b-30d50570abaa', N'a9f85f43-c6f8-4c23-9255-2a2b18733f2c', N'05590ec8-fe6a-49f9-86f5-a9266078b220', N'a69d5403-e98e-4c8b-8de1-6ed3e9099b70', N'46631fbb-e268-44f5-8225-7ed7c7e76492', NULL)
GO
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'0cc68ddb-a1b9-4d5e-883f-08dc028ff57a', 37, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'5a0afa61-7175-4525-b553-933d7fe70485')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'ca4e156f-4545-488c-8840-08dc028ff57a', 11, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'ce2f5a30-d79b-47ff-bd1f-15d2b545cb5a')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'd5531dec-68cd-4390-8841-08dc028ff57a', 55, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'1a83f348-9324-4e87-925a-947b5417d2dd')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'2f403e4b-f6c4-44b6-8842-08dc028ff57a', 26, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'e09655e8-a3c7-4357-a1e8-e6052f40f1bb')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'99b37081-7cc5-40b3-8843-08dc028ff57a', -5, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'0ef24690-fc05-43d3-a3ce-905602c3619f')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'd4766fb6-3684-4ec9-8844-08dc028ff57a', 1, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'53316389-4507-4211-9090-fb7e4c8ddd01')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'f1cd6457-59b5-4449-8845-08dc028ff57a', 11, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'7cf0e69d-ae75-4008-9625-b164a0bc5654')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'98f9f8d8-3078-47eb-8846-08dc028ff57a', 29, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'6a1d5552-f6b7-4970-aaeb-02cf17bb347c')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'55f0ab49-ec82-4bcb-8847-08dc028ff57a', 50, N'a2c0d461-401f-46ba-b3ca-2c9a80175bdc', N'd0048ba0-2292-4f50-9be2-683088b3432e')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'cd2ee1bb-6848-41a8-8848-08dc028ff57a', 1, N'46631fbb-e268-44f5-8225-7ed7c7e76491', N'5a0afa61-7175-4525-b553-933d7fe70485')
INSERT [dbo].[ShoesDetails_Size] ([ID], [Quantity], [ShoesDetailsId], [SizeID]) VALUES (N'39d4ca33-b8e1-49db-80c0-08dc178173fb', 18, N'46631fbb-e268-44f5-8225-7ed7c7e76491', N'7cf0e69d-ae75-4008-9625-b164a0bc5654')
GO
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'6a1d5552-f6b7-4970-aaeb-02cf17bb347c', N'SZ008', N'41', 0, CAST(N'2023-12-10T08:52:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'ce2f5a30-d79b-47ff-bd1f-15d2b545cb5a', N'SZ001', N'35', 0, CAST(N'2023-10-10T17:12:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'd0048ba0-2292-4f50-9be2-683088b3432e', N'SZ009', N'42', 0, CAST(N'2023-12-10T08:52:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'0ef24690-fc05-43d3-a3ce-905602c3619f', N'SZ002', N'38', 0, CAST(N'2023-10-10T17:12:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'5a0afa61-7175-4525-b553-933d7fe70485', N'SZ004', N'34', 0, CAST(N'2023-12-10T08:50:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'1a83f348-9324-4e87-925a-947b5417d2dd', N'SZ005', N'36', 0, CAST(N'2023-12-10T08:51:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'7cf0e69d-ae75-4008-9625-b164a0bc5654', N'SZ003', N'40', 0, CAST(N'2023-10-10T17:12:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'e09655e8-a3c7-4357-a1e8-e6052f40f1bb', N'SZ006', N'37', 0, CAST(N'2023-12-10T08:51:00.000' AS DateTime))
INSERT [dbo].[Size] ([SizeID], [SizeCode], [Name], [Status], [DateCreated]) VALUES (N'53316389-4507-4211-9090-fb7e4c8ddd01', N'SZ007', N'39', 0, CAST(N'2023-12-10T08:52:00.000' AS DateTime))
GO
INSERT [dbo].[Sole] ([SoleID], [SoleCode], [Name], [Height], [Status], [DateCreated]) VALUES (N'55f9c4cf-efde-4c13-a1be-0f110b55a9ea', N'SL001', N'XXX', CAST(15.00 AS Decimal(18, 2)), 0, CAST(N'2023-10-24T16:14:00.000' AS DateTime))
INSERT [dbo].[Sole] ([SoleID], [SoleCode], [Name], [Height], [Status], [DateCreated]) VALUES (N'd85161bb-634e-4c88-b7da-6543369ea4ea', N'SL002', N'Đế nhựa', CAST(1.00 AS Decimal(18, 2)), 0, CAST(N'2023-10-24T16:16:00.000' AS DateTime))
INSERT [dbo].[Sole] ([SoleID], [SoleCode], [Name], [Height], [Status], [DateCreated]) VALUES (N'05590ec8-fe6a-49f9-86f5-a9266078b220', N'SL003', N'Đế cao su', CAST(2.00 AS Decimal(18, 2)), 0, CAST(N'2023-10-24T16:34:00.000' AS DateTime))
GO
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'9d6d2f5e-df11-43b8-b31f-0ba0336aa659', N'ST004', N'Women''s Shoes', 0, CAST(N'2023-10-25T09:33:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'b43cffb2-4f56-4753-9835-29026405dcf5', N'ST003', N'Men''s shoes', 0, CAST(N'2023-10-25T09:22:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'd13546bc-5117-4342-b00e-2d2b3ebf90fe', N'ST005', N'Women''s Slippers', 0, CAST(N'2023-10-25T09:39:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'ba20e296-893d-4794-87d5-6d3423ed3f12', N'ST001', N'Unisex', 0, CAST(N'2023-10-10T17:13:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'a69d5403-e98e-4c8b-8de1-6ed3e9099b70', N'ST006', N'Skate Shoes', 0, CAST(N'2023-10-25T09:48:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'2dd9425c-2956-4e05-8858-b50c5b63b253', N'ST008', N'SDC', 0, CAST(N'2023-12-29T14:28:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'f7a4b663-22c2-4568-bf5a-ca505a9f01a3', N'ST002', N'Running', 0, CAST(N'2023-10-10T17:13:00.000' AS DateTime))
INSERT [dbo].[Style] ([StyleID], [StyleCode], [Name], [Status], [DateCreated]) VALUES (N'93f13352-0a6d-4401-a667-fd104ea1ecf5', N'ST007', N'Duy', 0, CAST(N'2023-12-16T16:31:00.000' AS DateTime))
GO
INSERT [dbo].[Supplier] ([SupplierID], [SupplierCode], [Name], [Status], [DateCreated]) VALUES (N'eefb8e43-4beb-403b-a470-9a47e5c36f27', N'SP003', N'Puma', 0, CAST(N'2023-10-10T17:11:00.000' AS DateTime))
INSERT [dbo].[Supplier] ([SupplierID], [SupplierCode], [Name], [Status], [DateCreated]) VALUES (N'4dc04515-14c7-40c5-a1c9-c8fdcae48132', N'SP001', N'Nike', 0, CAST(N'2023-10-10T17:11:00.000' AS DateTime))
INSERT [dbo].[Supplier] ([SupplierID], [SupplierCode], [Name], [Status], [DateCreated]) VALUES (N'b0a7f66d-d08c-40d2-b570-d22e6fee5376', N'SP002', N'Adidas', 0, CAST(N'2023-10-10T17:11:00.000' AS DateTime))
GO
INSERT [dbo].[Voucher] ([VoucherID], [VoucherCode], [VoucherValue], [Total], [Exclusiveright], [MaxUsage], [Type], [RemainingUsage], [CreateDate], [ExpirationDate], [Status], [IsDel], [UserNameCustomer], [DateCreated]) VALUES (N'b72ab68e-82c7-45dd-9f12-3957c4ad46f2', N'VC002', CAST(500.00 AS Decimal(18, 2)), CAST(100000.00 AS Decimal(18, 2)), N'Không', 4, 0, 20, CAST(N'2024-01-05T08:22:03.753' AS DateTime), CAST(N'2024-01-08T08:21:00.000' AS DateTime), 0, 0, NULL, CAST(N'2024-01-05T08:21:00.000' AS DateTime))
INSERT [dbo].[Voucher] ([VoucherID], [VoucherCode], [VoucherValue], [Total], [Exclusiveright], [MaxUsage], [Type], [RemainingUsage], [CreateDate], [ExpirationDate], [Status], [IsDel], [UserNameCustomer], [DateCreated]) VALUES (N'a2024d7e-7608-49a0-be2b-acf040d8f9e9', N'VC003', CAST(20000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), N'Không', 10, 0, 10, CAST(N'2024-01-13T16:45:14.490' AS DateTime), CAST(N'2024-01-18T16:45:00.000' AS DateTime), 0, 0, NULL, CAST(N'2024-01-13T16:45:00.000' AS DateTime))
INSERT [dbo].[Voucher] ([VoucherID], [VoucherCode], [VoucherValue], [Total], [Exclusiveright], [MaxUsage], [Type], [RemainingUsage], [CreateDate], [ExpirationDate], [Status], [IsDel], [UserNameCustomer], [DateCreated]) VALUES (N'7b4c277d-ce62-4418-b975-b813d3715b5d', N'VC001', CAST(10000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), N'Không', 10, 0, 10, CAST(N'2023-12-29T09:46:48.907' AS DateTime), CAST(N'2023-12-31T09:46:00.000' AS DateTime), 0, 0, NULL, CAST(N'2023-12-29T09:46:00.000' AS DateTime))
INSERT [dbo].[Voucher] ([VoucherID], [VoucherCode], [VoucherValue], [Total], [Exclusiveright], [MaxUsage], [Type], [RemainingUsage], [CreateDate], [ExpirationDate], [Status], [IsDel], [UserNameCustomer], [DateCreated]) VALUES (N'5addba43-d67b-4051-b37f-c47a8d290e9b', N'VC004', CAST(20000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), N'Không', 6, 0, 20, CAST(N'2024-01-16T08:02:27.413' AS DateTime), CAST(N'2024-01-19T08:02:00.000' AS DateTime), 0, 0, NULL, CAST(N'2024-01-16T08:02:00.000' AS DateTime))
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT ((0)) FOR [TransactionType]
GO
ALTER TABLE [dbo].[Bill] ADD  DEFAULT ((0.0)) FOR [TotalRefundAmount]
GO
ALTER TABLE [dbo].[FavoriteShoes] ADD  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [FavoriteShoesID]
GO
ALTER TABLE [dbo].[FavoriteShoes] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [AddedDate]
GO
ALTER TABLE [dbo].[FavoriteShoes] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[ReturnedProducts] ADD  DEFAULT (N'') FOR [NamePurChaseMethod]
GO
ALTER TABLE [dbo].[ReturnedProducts] ADD  DEFAULT ((0)) FOR [TransactionType]
GO
ALTER TABLE [dbo].[Address]  WITH CHECK ADD  CONSTRAINT [FK_Address_Customer_CumstomerID] FOREIGN KEY([CumstomerID])
REFERENCES [dbo].[Customer] ([CumstomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Address] CHECK CONSTRAINT [FK_Address_Customer_CumstomerID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Address_AddressID] FOREIGN KEY([AddressID])
REFERENCES [dbo].[Address] ([AddressID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Address_AddressID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Customer_CustomerID] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Customer] ([CumstomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Customer_CustomerID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Employee_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Employee_EmployeeID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_PurchaseMethod_PurchaseMethodID] FOREIGN KEY([PurchaseMethodID])
REFERENCES [dbo].[PurchaseMethod] ([PurchaseMethodID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_PurchaseMethod_PurchaseMethodID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_ShippingVoucher_ShippingVoucherID] FOREIGN KEY([ShippingVoucherID])
REFERENCES [dbo].[ShippingVoucher] ([ShippingVoucherID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_ShippingVoucher_ShippingVoucherID]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Voucher_VoucherID] FOREIGN KEY([VoucherID])
REFERENCES [dbo].[Voucher] ([VoucherID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Voucher_VoucherID]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_Bill_BillID] FOREIGN KEY([BillID])
REFERENCES [dbo].[Bill] ([BillID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_Bill_BillID]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_ShoesDetails_Size_ShoesDetails_SizeID] FOREIGN KEY([ShoesDetails_SizeID])
REFERENCES [dbo].[ShoesDetails_Size] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_ShoesDetails_Size_ShoesDetails_SizeID]
GO
ALTER TABLE [dbo].[BillStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_BillStatusHistory_Bill_BillID] FOREIGN KEY([BillID])
REFERENCES [dbo].[Bill] ([BillID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BillStatusHistory] CHECK CONSTRAINT [FK_BillStatusHistory_Bill_BillID]
GO
ALTER TABLE [dbo].[BillStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_BillStatusHistory_Employee_EmployeeID] FOREIGN KEY([EmployeeID])
REFERENCES [dbo].[Employee] ([EmployeeID])
GO
ALTER TABLE [dbo].[BillStatusHistory] CHECK CONSTRAINT [FK_BillStatusHistory_Employee_EmployeeID]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD  CONSTRAINT [FK_Cart_Customer_CumstomerID] FOREIGN KEY([CumstomerID])
REFERENCES [dbo].[Customer] ([CumstomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Cart] CHECK CONSTRAINT [FK_Cart_Customer_CumstomerID]
GO
ALTER TABLE [dbo].[CartDetails]  WITH CHECK ADD  CONSTRAINT [FK_CartDetails_Cart_CumstomerID] FOREIGN KEY([CumstomerID])
REFERENCES [dbo].[Cart] ([CumstomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartDetails] CHECK CONSTRAINT [FK_CartDetails_Cart_CumstomerID]
GO
ALTER TABLE [dbo].[CartDetails]  WITH CHECK ADD  CONSTRAINT [FK_CartDetails_ShoesDetails_Size_ShoesDetails_SizeID] FOREIGN KEY([ShoesDetails_SizeID])
REFERENCES [dbo].[ShoesDetails_Size] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CartDetails] CHECK CONSTRAINT [FK_CartDetails_ShoesDetails_Size_ShoesDetails_SizeID]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Rank_RankID] FOREIGN KEY([RankID])
REFERENCES [dbo].[Rank] ([RankID])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Rank_RankID]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK_Employee_Role_RoleID] FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([RoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK_Employee_Role_RoleID]
GO
ALTER TABLE [dbo].[FavoriteShoes]  WITH CHECK ADD  CONSTRAINT [FK_FavoriteShoes_Customer_CumstomerID] FOREIGN KEY([CumstomerID])
REFERENCES [dbo].[Customer] ([CumstomerID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FavoriteShoes] CHECK CONSTRAINT [FK_FavoriteShoes_Customer_CumstomerID]
GO
ALTER TABLE [dbo].[FavoriteShoes]  WITH CHECK ADD  CONSTRAINT [FK_FavoriteShoes_ShoesDetails_Size_ShoesDetails_SizeId] FOREIGN KEY([ShoesDetails_SizeId])
REFERENCES [dbo].[ShoesDetails_Size] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FavoriteShoes] CHECK CONSTRAINT [FK_FavoriteShoes_ShoesDetails_Size_ShoesDetails_SizeId]
GO
ALTER TABLE [dbo].[Image]  WITH CHECK ADD  CONSTRAINT [FK_Image_ShoesDetails_ShoesDetailsID] FOREIGN KEY([ShoesDetailsID])
REFERENCES [dbo].[ShoesDetails] ([ShoesDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Image] CHECK CONSTRAINT [FK_Image_ShoesDetails_ShoesDetailsID]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Material_MaterialId] FOREIGN KEY([MaterialId])
REFERENCES [dbo].[Material] ([MaterialId])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Material_MaterialId]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Supplier_SupplierID] FOREIGN KEY([SupplierID])
REFERENCES [dbo].[Supplier] ([SupplierID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Supplier_SupplierID]
GO
ALTER TABLE [dbo].[ReturnedProducts]  WITH CHECK ADD  CONSTRAINT [FK_ReturnedProducts_Bill_BillId] FOREIGN KEY([BillId])
REFERENCES [dbo].[Bill] ([BillID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ReturnedProducts] CHECK CONSTRAINT [FK_ReturnedProducts_Bill_BillId]
GO
ALTER TABLE [dbo].[ReturnedProducts]  WITH CHECK ADD  CONSTRAINT [FK_ReturnedProducts_ShoesDetails_Size_ShoesDetails_SizeID] FOREIGN KEY([ShoesDetails_SizeID])
REFERENCES [dbo].[ShoesDetails_Size] ([ID])
GO
ALTER TABLE [dbo].[ReturnedProducts] CHECK CONSTRAINT [FK_ReturnedProducts_ShoesDetails_Size_ShoesDetails_SizeID]
GO
ALTER TABLE [dbo].[ShoesDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Color_ColorID] FOREIGN KEY([ColorID])
REFERENCES [dbo].[Color] ([ColorID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails] CHECK CONSTRAINT [FK_ShoesDetails_Color_ColorID]
GO
ALTER TABLE [dbo].[ShoesDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Product_ProductID] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails] CHECK CONSTRAINT [FK_ShoesDetails_Product_ProductID]
GO
ALTER TABLE [dbo].[ShoesDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Sex_SexID] FOREIGN KEY([SexID])
REFERENCES [dbo].[Sex] ([SexID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails] CHECK CONSTRAINT [FK_ShoesDetails_Sex_SexID]
GO
ALTER TABLE [dbo].[ShoesDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Sole_SoleID] FOREIGN KEY([SoleID])
REFERENCES [dbo].[Sole] ([SoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails] CHECK CONSTRAINT [FK_ShoesDetails_Sole_SoleID]
GO
ALTER TABLE [dbo].[ShoesDetails]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Style_StyleID] FOREIGN KEY([StyleID])
REFERENCES [dbo].[Style] ([StyleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails] CHECK CONSTRAINT [FK_ShoesDetails_Style_StyleID]
GO
ALTER TABLE [dbo].[ShoesDetails_Size]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Size_ShoesDetails_ShoesDetailsId] FOREIGN KEY([ShoesDetailsId])
REFERENCES [dbo].[ShoesDetails] ([ShoesDetailsId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails_Size] CHECK CONSTRAINT [FK_ShoesDetails_Size_ShoesDetails_ShoesDetailsId]
GO
ALTER TABLE [dbo].[ShoesDetails_Size]  WITH CHECK ADD  CONSTRAINT [FK_ShoesDetails_Size_Size_SizeID] FOREIGN KEY([SizeID])
REFERENCES [dbo].[Size] ([SizeID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ShoesDetails_Size] CHECK CONSTRAINT [FK_ShoesDetails_Size_Size_SizeID]
GO
