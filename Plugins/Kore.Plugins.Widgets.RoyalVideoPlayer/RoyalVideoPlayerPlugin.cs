using System.Data.Entity;
using Kore.EntityFramework.Data.EntityFramework;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Infrastructure;
using Kore.Web.Plugins;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public class RoyalVideoPlayerPlugin : BasePlugin
    {
        public override void Install()
        {
            base.Install();
            InstallLanguagePack<LanguagePackInvariant>();
//            var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
//            var dbContext = dbContextFactory.GetContext();

//            if (!CheckIfTableExists(dbContext, Constants.Tables.Playlists))
//            {
//                #region CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Playlists]

//                dbContext.Database.ExecuteSqlCommand(
//@"CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Playlists]
//(
//	[Id] [int] IDENTITY(1,1) NOT NULL,
//	[Name] [nvarchar](255) NOT NULL,
//	PRIMARY KEY CLUSTERED
//	(
//		[Id] ASC
//	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
//) ON [PRIMARY]");

//                #endregion CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Playlists]
//            }

//            if (!CheckIfTableExists(dbContext, Constants.Tables.Videos))
//            {
//                #region CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Videos]

//                dbContext.Database.ExecuteSqlCommand(
//@"CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Videos]
//(
//	[Id] [int] IDENTITY(1,1) NOT NULL,
//	[Title] [nvarchar](255) NOT NULL,
//	[ThumbnailUrl] [nvarchar](255) NOT NULL,
//	[VideoUrl] [nvarchar](255) NOT NULL,
//	[MobileVideoUrl] [nvarchar](255) NULL,
//	[PosterUrl] [nvarchar](255) NOT NULL,
//	[MobilePosterUrl] [nvarchar](255) NULL,
//	[IsDownloadable] [bit] NOT NULL,
//	[PopoverHtml] [nvarchar](max) NULL,
//	PRIMARY KEY CLUSTERED
//	(
//		[Id] ASC
//	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
//) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]");

//                #endregion CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_Videos]
//            }

//            if (!CheckIfTableExists(dbContext, Constants.Tables.PlaylistVideos))
//            {
//                #region CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos]

//                dbContext.Database.ExecuteSqlCommand(
//@"CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos]
//(
//	[PlaylistId] [int] NOT NULL,
//	[VideoId] [int] NOT NULL,
//	PRIMARY KEY CLUSTERED
//	(
//		[PlaylistId] ASC,
//		[VideoId] ASC
//	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
//) ON [PRIMARY]");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos] WITH CHECK
//ADD CONSTRAINT [Playlist_Videos_Source] FOREIGN KEY([PlaylistId])
//REFERENCES [dbo].[Kore_Plugins_RoyalVideoPlayer_Playlists] ([Id])
//ON DELETE CASCADE");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos]
//CHECK CONSTRAINT [Playlist_Videos_Source]");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos] WITH CHECK
//ADD CONSTRAINT [Playlist_Videos_Target] FOREIGN KEY([VideoId])
//REFERENCES [dbo].[Kore_Plugins_RoyalVideoPlayer_Videos] ([Id])
//ON DELETE CASCADE");

//                dbContext.Database.ExecuteSqlCommand(
//@"ALTER TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos]
//CHECK CONSTRAINT [Playlist_Videos_Target]");

//                #endregion CREATE TABLE [dbo].[Kore_Plugins_RoyalVideoPlayer_PlaylistVideos]
//            }
        }

        public override void Uninstall()
        {
            UninstallLanguagePack<LanguagePackInvariant>();

            var dbContextFactory = EngineContext.Current.Resolve<IDbContextFactory>();
            var dbContext = dbContextFactory.GetContext();
            DropTable(dbContext, Constants.Tables.PlaylistVideos);
            DropTable(dbContext, Constants.Tables.Videos);
            DropTable(dbContext, Constants.Tables.Playlists);

            base.Uninstall();
        }
    }
}