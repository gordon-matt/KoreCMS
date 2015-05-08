using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Ecommerce.Simple.Infrastructure;
using Kore.Web.Plugins;
using System.Linq;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class SimpleCommercePlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();

            InstallLocalizableStrings<DefaultLocalizableStringsProvider>();

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

                #endregion
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
	[MainImageUrl] [nvarchar](255) NULL,
	[ShortDescription] [nvarchar](max) NOT NULL,
	[FullDescription] [nvarchar](max) NOT NULL,
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

                #endregion
            }
        }

        public override void Uninstall()
        {
            UninstallLocalizableStrings<DefaultLocalizableStringsProvider>();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.Categories);
            DropTable(dbContext, Constants.Tables.Products);

            base.Uninstall();
        }
    }
}