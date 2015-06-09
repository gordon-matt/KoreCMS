using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Ecommerce.Simple.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class SimpleCommercePlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();

            InstallLanguagePack<LanguagePackInvariant>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (!CheckIfTableExists(dbContext, Constants.Tables.Categories))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Categories]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Categories]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ParentId] [int] NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Slug] [nvarchar](255) NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
	[Order] [int] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[MetaKeywords] [nvarchar](255) NULL,
	[MetaDescription] [nvarchar](255) NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_Categories] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Categories] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Categories_dbo.Kore_Plugins_SimpleCommerce_Categories_ParentId]
FOREIGN KEY([ParentId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Categories] ([Id])");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Categories]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Categories_dbo.Kore_Plugins_SimpleCommerce_Categories_ParentId]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Categories]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.Products))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Products]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Products]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Slug] [nvarchar](255) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Price] [real] NOT NULL,
	[Tax] [real] NOT NULL,
	[ShippingCost] [real] NOT NULL,
	[MainImageUrl] [nvarchar](255) NULL,
	[ShortDescription] [nvarchar](max) NOT NULL,
	[FullDescription] [nvarchar](max) NOT NULL,
	[MetaKeywords] [nvarchar](255) NULL,
	[MetaDescription] [nvarchar](255) NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_Products] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Products] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Products_dbo.Kore_Plugins_SimpleCommerce_Categories_CategoryId]
FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Categories] ([Id])
ON DELETE CASCADE");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Products]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Products_dbo.Kore_Plugins_SimpleCommerce_Categories_CategoryId]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Products]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.Addresses))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Addresses]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Addresses]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[FamilyName] [nvarchar](128) NOT NULL,
	[GivenNames] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[AddressLine1] [nvarchar](128) NOT NULL,
	[AddressLine2] [nvarchar](128) NULL,
	[AddressLine3] [nvarchar](128) NULL,
	[City] [nvarchar](128) NOT NULL,
	[PostalCode] [nvarchar](10) NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](25) NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_Addresses] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Addresses]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.Orders))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[BillingAddressId] [int] NOT NULL,
	[ShippingAddressId] [int] NOT NULL,
	[ShippingTotal] [real] NOT NULL,
	[TaxTotal] [real] NOT NULL,
	[OrderTotal] [real] NOT NULL,
	[IPAddress] [nvarchar](max) NULL,
	[OrderDateUtc] [datetime] NOT NULL,
	[Status] [tinyint] NOT NULL,
	[PaymentStatus] [tinyint] NOT NULL,
	[AuthorizationTransactionId] [nvarchar](255) NULL,
	[DatePaidUtc] [datetime] NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_Orders] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Orders_dbo.Kore_Plugins_SimpleCommerce_Addresses_BillingAddressId]
FOREIGN KEY([BillingAddressId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Addresses] ([Id])");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Orders_dbo.Kore_Plugins_SimpleCommerce_Addresses_BillingAddressId]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Orders_dbo.Kore_Plugins_SimpleCommerce_Addresses_ShippingAddressId]
FOREIGN KEY([ShippingAddressId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Addresses] ([Id])");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_Orders_dbo.Kore_Plugins_SimpleCommerce_Addresses_ShippingAddressId]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.OrderLines))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitPrice] [real] NOT NULL,
	[Quantity] [smallint] NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_OrderLines] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderLines_dbo.Kore_Plugins_SimpleCommerce_Orders_OrderId]
FOREIGN KEY([OrderId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Orders] ([Id])
ON DELETE CASCADE");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderLines_dbo.Kore_Plugins_SimpleCommerce_Orders_OrderId]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderLines_dbo.Kore_Plugins_SimpleCommerce_Products_ProductId]
FOREIGN KEY([ProductId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Products] ([Id])
ON DELETE CASCADE");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderLines_dbo.Kore_Plugins_SimpleCommerce_Products_ProductId]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderLines]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.OrderNotes))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderNotes]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderNotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[DisplayToCustomer] [bit] NOT NULL,
	[DateCreatedUtc] [datetime] NOT NULL,
	CONSTRAINT [PK_dbo.Kore_Plugins_SimpleCommerce_OrderNotes] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderNotes] WITH CHECK
ADD CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderNotes_dbo.Kore_Plugins_SimpleCommerce_Orders_OrderId]
FOREIGN KEY([OrderId])
REFERENCES [dbo].[Kore_Plugins_SimpleCommerce_Orders] ([Id])
ON DELETE CASCADE");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_SimpleCommerce_OrderNotes]
CHECK CONSTRAINT [FK_dbo.Kore_Plugins_SimpleCommerce_OrderNotes_dbo.Kore_Plugins_SimpleCommerce_Orders_OrderId]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_SimpleCommerce_Orders]
            }
        }

        public override void Uninstall()
        {
            UninstallLanguagePack<LanguagePackInvariant>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.OrderNotes);
            DropTable(dbContext, Constants.Tables.OrderLines);
            DropTable(dbContext, Constants.Tables.Orders);
            DropTable(dbContext, Constants.Tables.Addresses);
            DropTable(dbContext, Constants.Tables.Products);
            DropTable(dbContext, Constants.Tables.Categories);

            base.Uninstall();
        }
    }
}