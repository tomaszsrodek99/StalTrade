USE [StalTrade]
GO
/****** Object:  Schema [Identity]    Script Date: 14.03.2024 15:35:04 ******/
CREATE SCHEMA [Identity]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[CompanyID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
	[ShortName] [nvarchar](64) NOT NULL,
	[Address] [nvarchar](64) NOT NULL,
	[City] [nvarchar](64) NOT NULL,
	[PostalCode] [nvarchar](6) NOT NULL,
	[PostOffice] [nvarchar](64) NOT NULL,
	[NIP] [nvarchar](10) NOT NULL,
	[PaymentMethod] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Companies] PRIMARY KEY CLUSTERED 
(
	[CompanyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contacts](
	[ContactID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[Firstname] [nvarchar](32) NOT NULL,
	[Lastname] [nvarchar](32) NOT NULL,
	[Position] [nvarchar](64) NOT NULL,
	[Phone1] [nvarchar](9) NOT NULL,
	[Phone2] [nvarchar](9) NULL,
	[Email] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED 
(
	[ContactID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deposit]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deposit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Cash] [decimal](10, 2) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Deposit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Expenses]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Expenses](
	[ExpenseId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[Contractor] [nvarchar](64) NOT NULL,
	[InvoiceNumber] [nvarchar](64) NOT NULL,
	[Description] [nvarchar](64) NOT NULL,
	[Netto] [decimal](10, 2) NOT NULL,
	[Brutto] [decimal](10, 2) NOT NULL,
	[DateOfPayment] [datetime2](7) NOT NULL,
	[Paid] [bit] NOT NULL,
	[PaymentType] [nvarchar](64) NOT NULL,
	[EventType] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED 
(
	[ExpenseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceProducts]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceProducts](
	[InvoiceProductId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Netto] [decimal](10, 2) NOT NULL,
	[Brutto] [decimal](10, 2) NOT NULL,
	[ActualQuantity] [int] NOT NULL,
	[IsPurchase] [bit] NOT NULL,
 CONSTRAINT [PK_InvoiceProducts] PRIMARY KEY CLUSTERED 
(
	[InvoiceProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[InvoiceId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceDate] [datetime2](7) NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[InvoiceNumber] [nvarchar](64) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Netto] [decimal](10, 2) NOT NULL,
	[Brutto] [decimal](10, 2) NOT NULL,
	[IsPurchase] [bit] NOT NULL,
 CONSTRAINT [PK_Invoices] PRIMARY KEY CLUSTERED 
(
	[InvoiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentMethods]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentMethods](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](64) NOT NULL,
 CONSTRAINT [PK_PaymentMethods] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Prices]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Prices](
	[PriceId] [int] IDENTITY(1,1) NOT NULL,
	[Netto] [decimal](10, 2) NOT NULL,
	[ProductId] [int] NOT NULL,
	[CompanyId] [int] NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[IsPurchase] [bit] NOT NULL,
 CONSTRAINT [PK_Prices] PRIMARY KEY CLUSTERED 
(
	[PriceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyDrawingNumber] [nvarchar](max) NOT NULL,
	[CustomerDrawingNumber] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NOT NULL,
	[UnitOfMeasure] [nvarchar](max) NOT NULL,
	[PurchaseVat] [decimal](5, 2) NOT NULL,
	[SalesVat] [decimal](5, 2) NOT NULL,
	[ConsumptionStandard] [decimal](10, 2) NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[ChargeProfile] [nvarchar](max) NULL,
	[MaterialGrade] [nvarchar](max) NULL,
	[SubstituteGrade] [nvarchar](max) NULL,
	[StockStatusId] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockStatuses]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockStatuses](
	[StockStatusId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[PurchasedQuantity] [int] NOT NULL,
	[ActualQuantity] [int] NOT NULL,
	[SoldQuantity] [int] NOT NULL,
	[InStock] [int] NOT NULL,
	[PurchasedValue] [decimal](10, 2) NOT NULL,
	[SoldValue] [decimal](10, 2) NOT NULL,
	[MarginValue] [decimal](10, 2) NOT NULL,
	[Margin] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_StockStatuses] PRIMARY KEY CLUSTERED 
(
	[StockStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[Firstname] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Identity].[__EFMigrationsHistory]    Script Date: 14.03.2024 15:35:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Identity].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Companies] ON 

INSERT [dbo].[Companies] ([CompanyID], [Name], [ShortName], [Address], [City], [PostalCode], [PostOffice], [NIP], [PaymentMethod]) VALUES (1, N'Firma A', N'FA', N'Ulica 1', N'Rzeszów', N'31-555', N'Rzeszów', N'1231231231', N'Przelew 30 dni')
INSERT [dbo].[Companies] ([CompanyID], [Name], [ShortName], [Address], [City], [PostalCode], [PostOffice], [NIP], [PaymentMethod]) VALUES (10, N'Firma C', N'FC', N'Przemysłowa 43', N'Stalowa Wola', N'32-546', N'Stalowa Wola', N'4566544324', N'Kompensata')
INSERT [dbo].[Companies] ([CompanyID], [Name], [ShortName], [Address], [City], [PostalCode], [PostOffice], [NIP], [PaymentMethod]) VALUES (11, N'Firma B', N'FB', N'Radomyśl 144', N'Radomyśl nad Sanem', N'37-455', N'Radomyśl nad Sanem', N'3213213123', N'Przelew 7 dni')
SET IDENTITY_INSERT [dbo].[Companies] OFF
GO
SET IDENTITY_INSERT [dbo].[Contacts] ON 

INSERT [dbo].[Contacts] ([ContactID], [CompanyID], [Firstname], [Lastname], [Position], [Phone1], [Phone2], [Email]) VALUES (9, 10, N'Imię', N'Nazwisko', N'Stanowisko', N'546456546', NULL, N'onet@onet.pl')
SET IDENTITY_INSERT [dbo].[Contacts] OFF
GO
SET IDENTITY_INSERT [dbo].[Expenses] ON 

INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (11, CAST(N'2024-02-03T00:00:00.0000000' AS DateTime2), N'Play', N'F/10637626/01/19', N'Abonament telefoniczny', CAST(149.50 AS Decimal(10, 2)), CAST(183.89 AS Decimal(10, 2)), CAST(N'2024-02-17T00:00:00.0000000' AS DateTime2), 1, N'Konto', N'Działalność')
INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (12, CAST(N'2024-02-04T00:00:00.0000000' AS DateTime2), N'ZUS', N'02/2024', N'ZUS', CAST(1228.70 AS Decimal(10, 2)), CAST(1228.70 AS Decimal(10, 2)), CAST(N'2024-02-04T00:00:00.0000000' AS DateTime2), 1, N'Konto', N'ZUS')
INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (13, CAST(N'2024-02-11T00:00:00.0000000' AS DateTime2), N'Poczta Polska', N'paragon', N'Znaczki pocztowe', CAST(7.80 AS Decimal(10, 2)), CAST(7.80 AS Decimal(10, 2)), CAST(N'2024-02-11T00:00:00.0000000' AS DateTime2), 1, N'Kasa', N'Działalność')
INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (14, CAST(N'2024-02-13T00:00:00.0000000' AS DateTime2), N'RiA', N'99/19/SP1', N'Paliwo', CAST(76.39 AS Decimal(10, 2)), CAST(93.96 AS Decimal(10, 2)), CAST(N'2024-02-13T00:00:00.0000000' AS DateTime2), 1, N'Konto', N'LPG REJESTRACJA')
INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (15, CAST(N'2024-02-20T00:00:00.0000000' AS DateTime2), N'Warsztat', N'2567', N'Wymiana oleju', CAST(230.00 AS Decimal(10, 2)), CAST(230.00 AS Decimal(10, 2)), CAST(N'2024-02-24T00:00:00.0000000' AS DateTime2), 0, N'Kasa', N'Naprawa REJESTRACJA')
INSERT [dbo].[Expenses] ([ExpenseId], [Date], [Contractor], [InvoiceNumber], [Description], [Netto], [Brutto], [DateOfPayment], [Paid], [PaymentType], [EventType]) VALUES (16, CAST(N'2024-02-22T00:00:00.0000000' AS DateTime2), N'RiA', N'2568', N'Paliwo', CAST(121.99 AS Decimal(10, 2)), CAST(150.05 AS Decimal(10, 2)), CAST(N'2024-02-22T00:00:00.0000000' AS DateTime2), 1, N'Kasa', N'LPG + BENZYNA REJESTRACJA')
SET IDENTITY_INSERT [dbo].[Expenses] OFF
GO
SET IDENTITY_INSERT [dbo].[InvoiceProducts] ON 

INSERT [dbo].[InvoiceProducts] ([InvoiceProductId], [InvoiceId], [ProductId], [Quantity], [Netto], [Brutto], [ActualQuantity], [IsPurchase]) VALUES (30, 20, 7, 131, CAST(2342.28 AS Decimal(10, 2)), CAST(2342.28 AS Decimal(10, 2)), 131, 0)
INSERT [dbo].[InvoiceProducts] ([InvoiceProductId], [InvoiceId], [ProductId], [Quantity], [Netto], [Brutto], [ActualQuantity], [IsPurchase]) VALUES (31, 21, 7, 131, CAST(3078.50 AS Decimal(10, 2)), CAST(3786.55 AS Decimal(10, 2)), 0, 0)
INSERT [dbo].[InvoiceProducts] ([InvoiceProductId], [InvoiceId], [ProductId], [Quantity], [Netto], [Brutto], [ActualQuantity], [IsPurchase]) VALUES (32, 22, 10, 50, CAST(1148.00 AS Decimal(10, 2)), CAST(1148.00 AS Decimal(10, 2)), 50, 0)
INSERT [dbo].[InvoiceProducts] ([InvoiceProductId], [InvoiceId], [ProductId], [Quantity], [Netto], [Brutto], [ActualQuantity], [IsPurchase]) VALUES (33, 22, 11, 50, CAST(950.00 AS Decimal(10, 2)), CAST(950.00 AS Decimal(10, 2)), 50, 0)
SET IDENTITY_INSERT [dbo].[InvoiceProducts] OFF
GO
SET IDENTITY_INSERT [dbo].[Invoices] ON 

INSERT [dbo].[Invoices] ([InvoiceId], [InvoiceDate], [PaymentDate], [InvoiceNumber], [CompanyId], [Netto], [Brutto], [IsPurchase]) VALUES (20, CAST(N'2024-02-26T00:00:00.0000000' AS DateTime2), CAST(N'2024-03-04T00:00:00.0000000' AS DateTime2), N'1Z.24/02/2024', 11, CAST(2342.28 AS Decimal(10, 2)), CAST(2342.28 AS Decimal(10, 2)), 1)
INSERT [dbo].[Invoices] ([InvoiceId], [InvoiceDate], [PaymentDate], [InvoiceNumber], [CompanyId], [Netto], [Brutto], [IsPurchase]) VALUES (21, CAST(N'2024-02-26T00:00:00.0000000' AS DateTime2), CAST(N'2024-02-26T00:00:00.0000000' AS DateTime2), N'1Z.26/02/2024', 10, CAST(3078.50 AS Decimal(10, 2)), CAST(3786.55 AS Decimal(10, 2)), 0)
INSERT [dbo].[Invoices] ([InvoiceId], [InvoiceDate], [PaymentDate], [InvoiceNumber], [CompanyId], [Netto], [Brutto], [IsPurchase]) VALUES (22, CAST(N'2024-02-26T00:00:00.0000000' AS DateTime2), CAST(N'2024-03-27T00:00:00.0000000' AS DateTime2), N'2Z.26/02/2024', 1, CAST(2098.00 AS Decimal(10, 2)), CAST(2098.00 AS Decimal(10, 2)), 1)
SET IDENTITY_INSERT [dbo].[Invoices] OFF
GO
SET IDENTITY_INSERT [dbo].[PaymentMethods] ON 

INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (1, N'Przelew 7 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (2, N'Przelew 14 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (3, N'Przelew 21 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (4, N'Przelew 30 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (5, N'Przelew 45 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (6, N'Przelew 60 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (7, N'Przelew 90 dni')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (8, N'Przedpłata')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (9, N'Kompensata')
INSERT [dbo].[PaymentMethods] ([Id], [Name]) VALUES (10, N'Gotówka')
SET IDENTITY_INSERT [dbo].[PaymentMethods] OFF
GO
SET IDENTITY_INSERT [dbo].[Prices] ON 

INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (2, CAST(0.19 AS Decimal(10, 2)), 8, 1, CAST(N'2024-02-18T13:50:14.8058144' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (3, CAST(1.18 AS Decimal(10, 2)), 7, 1, CAST(N'2024-02-18T16:05:52.7532185' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (8, CAST(1.52 AS Decimal(10, 2)), 7, 1, CAST(N'2024-02-22T12:21:35.8104627' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (9, CAST(64.19 AS Decimal(10, 2)), 7, 10, CAST(N'2024-02-22T13:25:14.0367152' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (10, CAST(0.51 AS Decimal(10, 2)), 8, 1, CAST(N'2024-02-22T13:25:41.0594006' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (11, CAST(5.65 AS Decimal(10, 2)), 8, 1, CAST(N'2024-02-22T19:48:53.1395355' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (12, CAST(22.50 AS Decimal(10, 2)), 7, 1, CAST(N'2024-02-22T19:49:22.1472765' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (13, CAST(23.50 AS Decimal(10, 2)), 7, 10, CAST(N'2024-02-22T19:49:30.6646511' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (14, CAST(6.90 AS Decimal(10, 2)), 9, 1, CAST(N'2024-02-22T19:49:49.2140064' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (15, CAST(68.00 AS Decimal(10, 2)), 10, 1, CAST(N'2024-02-22T19:50:04.2247225' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (16, CAST(24.40 AS Decimal(10, 2)), 11, 1, CAST(N'2024-02-22T19:50:17.1603882' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (17, CAST(2.74 AS Decimal(10, 2)), 12, 1, CAST(N'2024-02-22T19:50:45.7695517' AS DateTime2), 0)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (18, CAST(4.53 AS Decimal(10, 2)), 8, 1, CAST(N'2024-02-22T19:51:19.3439122' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (19, CAST(4.34 AS Decimal(10, 2)), 8, 10, CAST(N'2024-02-22T19:51:30.2881619' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (20, CAST(4.43 AS Decimal(10, 2)), 8, 11, CAST(N'2024-02-22T19:51:39.1837304' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (21, CAST(17.95 AS Decimal(10, 2)), 7, 1, CAST(N'2024-02-22T19:52:13.8672494' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (22, CAST(18.00 AS Decimal(10, 2)), 7, 10, CAST(N'2024-02-22T19:52:19.8315703' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (23, CAST(17.88 AS Decimal(10, 2)), 7, 11, CAST(N'2024-02-22T19:54:35.5859381' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (24, CAST(5.20 AS Decimal(10, 2)), 9, 1, CAST(N'2024-02-22T19:54:50.6497414' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (25, CAST(5.11 AS Decimal(10, 2)), 9, 10, CAST(N'2024-02-22T19:54:57.7274670' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (26, CAST(5.18 AS Decimal(10, 2)), 9, 11, CAST(N'2024-02-22T19:55:08.1370378' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (27, CAST(22.96 AS Decimal(10, 2)), 10, 1, CAST(N'2024-02-22T19:55:27.6199004' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (28, CAST(22.95 AS Decimal(10, 2)), 10, 10, CAST(N'2024-02-22T19:55:33.3286703' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (29, CAST(22.90 AS Decimal(10, 2)), 10, 11, CAST(N'2024-02-22T19:55:41.9759058' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (30, CAST(19.00 AS Decimal(10, 2)), 11, 1, CAST(N'2024-02-22T19:57:47.7945139' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (31, CAST(18.50 AS Decimal(10, 2)), 11, 10, CAST(N'2024-02-22T19:57:58.7736215' AS DateTime2), 1)
INSERT [dbo].[Prices] ([PriceId], [Netto], [ProductId], [CompanyId], [Date], [IsPurchase]) VALUES (32, CAST(18.80 AS Decimal(10, 2)), 11, 11, CAST(N'2024-02-22T19:58:10.9612004' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Prices] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (7, N'01-011-042', N'2026-080-138.00', N'Koło zębate', N'szt', CAST(0.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), NULL, CAST(3.55 AS Decimal(10, 2)), NULL, N'16HG', NULL, 0)
INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (8, N'01-011-041', N'2026-070-117.00', N'Chwytacz sznurka', N'szt', CAST(0.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), NULL, CAST(3.88 AS Decimal(10, 2)), NULL, N'16HG', NULL, 0)
INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (9, N'01-018-098', N'324-990-000012', N'Ucho cylindra', N'szt', CAST(0.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), CAST(1.13 AS Decimal(10, 2)), CAST(0.98 AS Decimal(10, 2)), NULL, N'St3s', N'20,35', 3)
INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (10, N'01-026-146', N'50.42.543.0', N'Koło', N'szt', CAST(0.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), NULL, CAST(11.90 AS Decimal(10, 2)), NULL, N'16HG', N'18XGT', 4)
INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (11, N'01-027-204', N'CT-200 B4486/1', N'Tarcza', N'szt', CAST(0.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), NULL, CAST(3.10 AS Decimal(10, 2)), NULL, N'16HG', N'18XGT Rosja', 5)
INSERT [dbo].[Products] ([ProductId], [CompanyDrawingNumber], [CustomerDrawingNumber], [Name], [UnitOfMeasure], [PurchaseVat], [SalesVat], [ConsumptionStandard], [Weight], [ChargeProfile], [MaterialGrade], [SubstituteGrade], [StockStatusId]) VALUES (12, N'fi.60 42CrMo4', NULL, N'Pręt walcowany', N'kg', CAST(23.00 AS Decimal(5, 2)), CAST(23.00 AS Decimal(5, 2)), NULL, CAST(10.00 AS Decimal(10, 2)), N'fi.60', N'42CrMo4', NULL, 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[StockStatuses] ON 

INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (1, 7, 131, 131, 131, 131, CAST(2342.28 AS Decimal(10, 2)), CAST(3786.55 AS Decimal(10, 2)), CAST(1444.27 AS Decimal(10, 2)), CAST(61.66 AS Decimal(10, 2)))
INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (2, 8, 0, 0, 0, 0, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (3, 9, 0, 0, 0, 0, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (4, 10, 50, 50, 0, 50, CAST(1148.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(-1148.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (5, 11, 50, 50, 0, 50, CAST(950.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(-950.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)))
INSERT [dbo].[StockStatuses] ([StockStatusId], [ProductId], [PurchasedQuantity], [ActualQuantity], [SoldQuantity], [InStock], [PurchasedValue], [SoldValue], [MarginValue], [Margin]) VALUES (6, 12, 0, 0, 0, 0, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[StockStatuses] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [Email], [PasswordHash], [PasswordSalt], [Firstname]) VALUES (2, N'user@example.com', 0xFBE1573CAD6859283FC2834BEA9BA399E3807DFC588889C19961E45FF7CAAB1AEEC449615918FFBDD366CC73C8D2613B5CD771D055AFFDBA039674372444113F, 0x2D1AE0C5C747280483DBC9DF41D36EA27EEF4DD45F2B549970601FACADC268A710AE8E9651F28405BD7B7E4C316A17D948E3F19D4FD67A9A5E51ADBBBACF8AA88694D7C67B37437E52E2F2351A346BD25EADC9A6C2EDECE5CE9D61D0865E819509C6D06F3AE0E038B152E94A8A531768137D015A22B869FBF12933C0499502D3, N'User')
INSERT [dbo].[Users] ([Id], [Email], [PasswordHash], [PasswordSalt], [Firstname]) VALUES (4, N'user@user.com', 0x2D732A6FD14C448BE6D50D781FB3049D41D7D65EA6B74C2AAE488059E36DC72FE911522AD4E15A3F82714EAFB53A6B9FB78AA747983D73A3ACF7D96F39237A29, 0xA5BF3994C3B3EA33B5780C716442E519BAD3F824210EAAB0499D546F968BF58F0F7580155063B1E0A3531FA2484384CDD089E8E63174DBFF460C72E5BF0068D1D49004DA5564F1215D629E5E37E78B6877FCFDD556C21A36C28A3F35D8C23E70A64E089C978370B999D4E2B9D1D13897D281E3233613BC4F10EAAA4107A4AE00, N'string')
INSERT [dbo].[Users] ([Id], [Email], [PasswordHash], [PasswordSalt], [Firstname]) VALUES (5, N'admin@example.com', 0xECD41A5F636A1241F1A094FEBD441C53FD6B488D7855EEF597983800ED091FD3FEB761B8779C011A5DE0E99A8590B491AD32B4F8D1F9808E584E470EED330AB0, 0xD3E35A1C84BC1D4D23E7499F5C9E991D3D2796DE6DD2144CE76F3C4B563F383720A79380E55916C7917AC5E9D66F281BDE037677B871E099BA2D56FA02C9B6F2B32CEAC7E118E6BE12A6462480E3212F89FCCF5DA89EEECE72A2BC4DE67297831A74FADEDD457A45B69C7EC875835CDD33F4F2B9582523A4DEF3524FE5B0267B, N'Administrator')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231016165457_Init', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231020190308_UpdateUserModel', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231106143948_CompanyWithContacts', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231113130247_UpdateCompanyRelationsWithContact', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231120105419_Contact-Phone2-field-update', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231127125003_AddProductWithPriceHistory', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231127125423_AddProductWithPriceHistory', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20231204140836_ExpensesTable', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240126135025_CreatePaymentMethod', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240205153943_update_date_columns', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240205160804_update_date_columns2', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240205172327_update_date_columns_repair', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240205172600_update_date_columns_repair', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240205173347_update_date_columns_repair', N'6.0.23')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240209134718_add-deposit', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240214150320_edit_deposit', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240215102026_replace-decimal-column', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240215114014_revert', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240216201725_create-warehouse', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240218114637_Update-price', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240220150528_Create-Invoices', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240222200037_Update-decimal-columns', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240226102809_Add-IsPurchaseColumn-in-InvoiceProduct', N'6.0.26')
INSERT [Identity].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240302111953_Remove-column-from-user', N'6.0.26')
GO
ALTER TABLE [dbo].[Deposit] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Date]
GO
ALTER TABLE [dbo].[InvoiceProducts] ADD  DEFAULT ((0)) FOR [ActualQuantity]
GO
ALTER TABLE [dbo].[InvoiceProducts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPurchase]
GO
ALTER TABLE [dbo].[Prices] ADD  DEFAULT ('0001-01-01T00:00:00.0000000') FOR [Date]
GO
ALTER TABLE [dbo].[Prices] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsPurchase]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0.0)) FOR [SalesVat]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [StockStatusId]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (N'') FOR [Firstname]
GO
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD  CONSTRAINT [FK_Contacts_Companies_CompanyID] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Companies] ([CompanyID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Contacts] CHECK CONSTRAINT [FK_Contacts_Companies_CompanyID]
GO
ALTER TABLE [dbo].[InvoiceProducts]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceProducts_Invoices_InvoiceId] FOREIGN KEY([InvoiceId])
REFERENCES [dbo].[Invoices] ([InvoiceId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceProducts] CHECK CONSTRAINT [FK_InvoiceProducts_Invoices_InvoiceId]
GO
ALTER TABLE [dbo].[InvoiceProducts]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceProducts_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[InvoiceProducts] CHECK CONSTRAINT [FK_InvoiceProducts_Products_ProductId]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([CompanyID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prices] CHECK CONSTRAINT [FK_Prices_Companies_CompanyId]
GO
ALTER TABLE [dbo].[Prices]  WITH CHECK ADD  CONSTRAINT [FK_Prices_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Prices] CHECK CONSTRAINT [FK_Prices_Products_ProductId]
GO
ALTER TABLE [dbo].[StockStatuses]  WITH CHECK ADD  CONSTRAINT [FK_StockStatuses_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StockStatuses] CHECK CONSTRAINT [FK_StockStatuses_Products_ProductId]
GO
