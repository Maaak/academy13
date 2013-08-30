﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class PathUtil : IPathUtil
    {
        private const string DELIMITER = @"/";

        private const string DATA_DIRECTORY_NAME = @"~/data";

        private const string PHOTOS_DIRECTORY_NAME = "photos";

        private const string COLLAGE_FILE_NAME = "collage";
        private const string COLLAGE_FILE_FORMAT = "jpg";
        private const string COLLAGES_DIRECTORY_NAME = "collages";

        private const string CUSTOM_AVATAR_PATH = @"~/Content/images/no_avatar.png";
        private const string CUSTOM_COLLAGE_PATH = @"~/Content/images/no_collage.png";
        private const string SUFFIX_AVATAR_FILENAME = "Avatar";
        private const string AVATAR_FILE_FORMAT = "jpg";
        private readonly IEnumerable<string> photoFormats= new[] { "*.bmp", "*.ico", "*.gif", "*.jpeg", "*.jpg", "*.jfif", "*.png", "*.tif", "*.tiff", "*.wmf", "*.emf"};
        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId
        /// </summary>
        public string BuildAlbumPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));
            builder.Append(DELIMITER)
                .Append(albumId);
            
            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\photoId.format
        /// </summary>
        public string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));
            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\imageSize\photoId.format
        /// </summary>
        public string BuildThumbnailPath(int userId, int albumId, int photoId, string format, ImageSize size)
        {
            var builder = new StringBuilder(BuildThumbnailsDirPath(userId, albumId, size));

            builder.Append(DELIMITER)
                .Append(photoId)
                .Append(MakeExtension(format));

            return builder.ToString();
        }

        private string BuildRealCollagePath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                    .Append(COLLAGES_DIRECTORY_NAME)
                    .Append(DELIMITER)
                    .Append(COLLAGE_FILE_NAME)
                    .Append(MakeExtension(COLLAGE_FILE_FORMAT));

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\[Small|Medium|Big]avatar.jpg or custom
        /// </summary>
        public string BuildAvatarPath(int userId, ImageSize imageSize)
        {
            string result;

            if (File.Exists(BuildAbsoluteOriginalAvatarPath(userId)))
            {
                var builder = new StringBuilder(BuildUserPath(userId));

                builder.Append(DELIMITER)
                    .Append(imageSize)
                    .Append(SUFFIX_AVATAR_FILENAME)
                    .Append(MakeExtension(AVATAR_FILE_FORMAT));

                result = builder.ToString();
            }
            else
            {
                result = VirtualPathUtility.ToAbsolute(CUSTOM_AVATAR_PATH);
            }

            return result;
        }

        public string BuildAbsoluteAvatarPath(int userId, ImageSize imageSize)
        {
            return HostingEnvironment.MapPath(BuildAvatarPath(userId, imageSize));
        }

        public string BuildAbsoluteAlbumPath(int userId, int albumId)
        {
            return HostingEnvironment.MapPath(BuildAlbumPath(userId, albumId));
        }

        public string BuildAbsoluteThumbailPath(int userId, int albumId, int photoId, string format, ImageSize size)
        {
            return HostingEnvironment.MapPath(BuildThumbnailPath(userId, albumId, photoId, format, size));
        }

        private bool modelExists(IEnumerable<PhotoModel> models, int photoId)
        {
            return models.Any(model => model.Id == photoId);
        }

        
        private IEnumerable<string> GetOnlyImages(string path)
        {
            var photoPaths = new List<string>();

            foreach (var photoFormat in photoFormats)
                photoPaths.AddRange(Directory.GetFiles(path, photoFormat));

            return photoPaths;
        }

        public IEnumerable<string> BuildAbsoluteThumbnailsPaths(int userId, int albumId,IEnumerable<PhotoModel> models,ImageSize size)
        {
            string thumbnailsDirectoryPath = BuildAbsoluteThumbnailsDirPath(userId, albumId, size);
            return Randomizer.GetEnumerator(GetOnlyImages(thumbnailsDirectoryPath).Where(s =>
                {
                    int photoId;
                    if (int.TryParse(Path.GetFileNameWithoutExtension(s), out photoId))
                        return modelExists(models, photoId);

                    return false;
                })).ToList();
        }

        public string BuildAbsoluteCollagePath(int userId, int albumId)
        {
            return HostingEnvironment.MapPath(BuildRealCollagePath(userId, albumId));
        }

        public string BuildAbsoluteCollagesDirPath(int userId, int albumId)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append(COLLAGES_DIRECTORY_NAME);

            string virtualCollagesDirectoryPath = builder.ToString();

            return HostingEnvironment.MapPath(virtualCollagesDirectoryPath);
        }

        public string CreateCollagePath(int userId, int albumId)
        {
            return Path.Combine(BuildAbsoluteAlbumPath(userId, albumId), COLLAGES_DIRECTORY_NAME,
                                Randomizer.GetString(20) + MakeExtension(COLLAGE_FILE_FORMAT));
        }

        public string GetCollage(int userId, int albumId)
        {
            string collagesDirName = Path.Combine(BuildAbsoluteAlbumPath(userId, albumId), COLLAGES_DIRECTORY_NAME);
            if (File.Exists(collagesDirName))
            {
                string path =
                    GetOnlyImages(collagesDirName)
                        .FirstOrDefault();
                if (path != null)
                    return GetUserReference(path);
            }
            return VirtualPathUtility.ToAbsolute(CUSTOM_COLLAGE_PATH);
        }

        public string GetUserReference(string absolutePath)
        {
            return absolutePath.Remove(0, absolutePath.IndexOf("data") - 1).Replace(@"\", "/");
        }

        public string BuildAbsoluteOriginalPhotoPath(int userId, int albumId, int photoId, string format)
        {
            return HostingEnvironment.MapPath(BuildOriginalPhotoPath(userId, albumId, photoId, format));
        }

        private string BuildAbsoluteOriginalAvatarPath(int userId)
        {
            var builder = new StringBuilder(BuildUserPath(userId));

            builder.Append(DELIMITER)
                .Append(ImageSize.Original)
                .Append(SUFFIX_AVATAR_FILENAME)
                .Append(MakeExtension(AVATAR_FILE_FORMAT));

            string virtualOriginalAvatarPath = builder.ToString();

            return HostingEnvironment.MapPath(virtualOriginalAvatarPath);
        }

        private string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, ImageSize size)
        {
            return HostingEnvironment.MapPath(BuildThumbnailsDirPath(userId, albumId, size));
        }

        /// <summary>
        ///     Pattern: ~data\photos
        /// </summary>
        private string BuildPhotoDirectoryPath()
        {
            var builder = new StringBuilder();
            builder.Append(GetDataDirectory())
                .Append(DELIMITER)
                .Append(PHOTOS_DIRECTORY_NAME);

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId\albumId\size
        /// </summary>
        private string BuildThumbnailsDirPath(int userId, int albumId, ImageSize size)
        {
            var builder = new StringBuilder(BuildAlbumPath(userId, albumId));

            builder.Append(DELIMITER)
                .Append((int) size);

            return builder.ToString();
        }

        /// <summary>
        ///     Pattern: ~data\photos\userId
        /// </summary>
        private string BuildUserPath(int userId)
        {
            var builder = new StringBuilder(BuildPhotoDirectoryPath());

            builder.Append(DELIMITER)
                .Append(userId);
            return builder.ToString();
        }

        public string BuildAbsolutePhotoDirectoryPath()
        {
            return HostingEnvironment.MapPath(BuildPhotoDirectoryPath());
        }

        private string MakeExtension(string format)
        {
            return "." + format;
        }

        private string GetDataDirectory()
        {
            return VirtualPathUtility.ToAbsolute(DATA_DIRECTORY_NAME);
        }
    }
}