﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"\";

        private const string THUMBNAIL_DIRECTORY_NAME = "thumbnail";

        private readonly string dataVirtualRoot;

        public PathUtil()
        {
            dataVirtualRoot = ConfigurationManager.AppSettings["DataDirectory"];
        }

        public string BuildPhotoDirectoryPath()
        {
            const string PHOTOS_DIRECTORY_NAME = "photos";

            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                   .Append(DELIMITER)
                   .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                   .Append(albumId);

            return builder.ToString();
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);
            return HostingEnvironment.MapPath(builder.ToString());
        }

        public string BuildUserPath(int userId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());
            builder.Append(DELIMITER)
                   .Append(userId);
            return builder.ToString();
        }

        public string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string photoFormat)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(photoId)
                   .Append(photoFormat);

            return builder.ToString();
        }

        public string BuildThumbnailsPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                   .Append(THUMBNAIL_DIRECTORY_NAME);

            return builder.ToString();
        }

        public string BuildUserAvatarPath(int userId)
        {
            const string AVATAR_FILE_NAME = "avatar.jpg";

            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(AVATAR_FILE_NAME);

            return builder.ToString();
        }

        public IEnumerable<string> BuildTemporaryDirectoriesPaths()
        {
            string photoDirectoryPath = BuildPhotoDirectoryPath();

            IEnumerable<string> usersDirectories = Directory.EnumerateDirectories(photoDirectoryPath);

            var temporaryPhotosDirectories = new Collection<string>();

            foreach (string temporaryPhotosDirectory in usersDirectories.Select(BuildTemporaryDirectoryPath))
            {
                temporaryPhotosDirectories.Add(temporaryPhotosDirectory);
            }

            return temporaryPhotosDirectories;
        }

        public string BuildAbsoluteTemporaryDirectoryPath(int userId)
        {
            var userPath = BuildUserPath(userId);

            var userTempPath = BuildTemporaryDirectoryPath(userPath);

            return HostingEnvironment.MapPath(userTempPath);
        }

        private string GetDataDirectory()
        {
            return VirtualPathUtility.ToAbsolute(dataVirtualRoot); 
        }

        private string BuildTemporaryDirectoryPath(string userDirectoryPath)
        {
            const string TEMPORARY_DIRECTORY_NAME = "temporary";

            return Path.Combine(userDirectoryPath, TEMPORARY_DIRECTORY_NAME);
        }
    }
}