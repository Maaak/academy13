﻿namespace BinaryStudio.PhotoGallery.Core.PathUtils
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
        string BuildUserAvatarPath(int userId);

        // todo: change signature
        /// <summary>
        /// Deprecated
        /// </summary>
        string BuildThumbnailPath(int userId, int albumId, int photoId, string format);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryDirectoryPath(int userId);

        /// <returns>Path in format "C:\\ololo\\ololo"</returns>
        string BuildAbsoluteTemporaryAlbumPath(int userId, int albumId);

        string BuildPathToUsersFolderOnServer();

        string BuildPathToUserFolderOnServer(int userId);

        string BuildPathToUserAvatarOnServer(int userId);

        string BuildPathToUserAlbumFolderOnServer(int userId, int albumId);

        string BuildPathToUserAlbumThumbnailsFolderOnServer(int userId, int albumId, int thumbnailsSize);

        string BuildPathToThumbnailFileOnServer(int userId, int albumId, int thumbnailsSize, string originalPath);

        string BuildPathToOriginalFileOnServer(int userId, int albumId,string originalName);

        string BuildPathToUserAlbumCollagesFolderOnServer(int userId, int albumId);

        string GetEndUserReference(string absolutePath);

        string NoAvatar();

        string MakeFileNameWithExtension(string name);

        string MakePathToCollage(int userId,int albumId,int lenght);
    }
}
