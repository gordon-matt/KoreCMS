using System.Data.Entity;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.RevolutionSlider.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RevolutionSlider
{
    public class RevolutionSliderPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
            var dbContext = EngineContext.Current.Resolve<DbContext>();

            if (!CheckIfTableExists(dbContext, Constants.Tables.Sliders))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Sliders]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Sliders]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	CONSTRAINT [PK_Kore_Plugins_RevolutionSlider_Sliders] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Sliders]
            }

            if (!CheckIfTableExists(dbContext, Constants.Tables.Slides))
            {
                #region CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Slides]

                dbContext.Database.ExecuteSqlCommand(
@"CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Slides]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SliderId] [int] NOT NULL,
	[Order] [smallint] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Link] [nvarchar](255) NULL,
	[Target] [tinyint] NULL,
	[Transition] [tinyint] NULL,
	[RandomTransition] [bit] NOT NULL,
	[SlotAmount] [tinyint] NULL,
	[MasterSpeed] [smallint] NULL,
	[Delay] [smallint] NULL,
	[SlideIndex] [tinyint] NULL,
	[Thumb] [nvarchar](255) NULL,
	[ImageUrl] [nvarchar](255) NOT NULL,
	[LazyLoad] [bit] NOT NULL,
	[BackgroundRepeat] [tinyint] NULL,
	[BackgroundFit] [tinyint] NULL,
	[BackgroundFitCustomValue] [smallint] NULL,
	[BackgroundFitEnd] [smallint] NULL,
	[BackgroundPosition] [tinyint] NULL,
	[BackgroundPositionEnd] [tinyint] NULL,
	[KenBurnsEffect] [bit] NOT NULL,
	[Duration] [smallint] NULL,
	[Easing] [tinyint] NULL,
	CONSTRAINT [PK_Kore_Plugins_RevolutionSlider_Slides] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_RevolutionSlider_Slides] WITH CHECK
ADD CONSTRAINT [FK_Kore_Plugins_RevolutionSlider_Slides_Kore_Plugins_RevolutionSlider_Sliders]
FOREIGN KEY([SliderId])
REFERENCES [dbo].[Kore_Plugins_RevolutionSlider_Sliders] ([Id])");

                dbContext.Database.ExecuteSqlCommand(
@"ALTER TABLE [dbo].[Kore_Plugins_RevolutionSlider_Slides]
CHECK CONSTRAINT [FK_Kore_Plugins_RevolutionSlider_Slides_Kore_Plugins_RevolutionSlider_Sliders]");

                #endregion CREATE TABLE [dbo].[Kore_Plugins_RevolutionSlider_Slides]
            }
        }

        public override void Uninstall()
        {
            base.Uninstall();

            var dbContext = EngineContext.Current.Resolve<DbContext>();
            DropTable(dbContext, Constants.Tables.Sliders);
            DropTable(dbContext, Constants.Tables.Slides);

            UninstallLanguagePack<LanguagePackInvariant>();
        }
    }
}