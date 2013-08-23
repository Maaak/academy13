﻿using BinaryStudio.PhotoGallery.Core.PhotoUtils;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    public interface IPathUtil
    {
        /// <returns>Pattern: ~data\photos</returns>
        string BuildPhotoDirectoryPath();

        /// <returns>Pattern: ~data\photos\userId\albumId</returns>
        string BuildAlbumPath(int userId, int albumId);

        /// <returns>Pattern: ~data\photos\userId\albumId\photoId.format</returns>
        string BuildOriginalPhotoPath(int userId, int albumId, int photoId, string format);

        /// <returns>Pattern: ~data\photos\userId\albumId\thumbnails</returns>
        string BuildThumbnailsPath(int userId, int albumId);

        /// <returns>Pattern: ~data\photos\userId\avatar.jpg</returns>
        string BuildAvatarPath(int userId);

        // todo: change signature
        /// <summary>
        ///     Deprecated
        /// </summary>
        string BuildThumbnailPath(int userId, int albumId, int photoId, string format);

        string BuildThumbnailPathSized(int userId, int albumId, int photoId, string format, int size);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryDirectoryPath(int userId);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryAlbumPath(int userId, int albumId);

        string BuildAbsoluteUserDirPath(int userId);

        string BuildAbsoluteAvatarPath(int userId, ImageSize size = ImageSize.Original);

        string BuildAbsoluteAlbumPath(int userId, int albumId);

        string BuildAbsoluteThumbnailsDirPath(int userId, int albumId, int thumbnailsSize);

        string BuildAbsoluteAlbumCollagesDirPath(int userId, int albumId);

        string GetEndUserReference(string absolutePath);

        string MakeFileName(string name, string ext);

        string BuildPathToOriginalFileOnServer(int userId, int albumId, int photoId, string format);

        string BuildPathToThumbnailFileOnServer(int userId, int albumId, int photoId, string format, int thumbnailsSize);

        string CustomAvatarPath { get; }

        string CreatePathToCollage(int userId, int albumId);
    }
}