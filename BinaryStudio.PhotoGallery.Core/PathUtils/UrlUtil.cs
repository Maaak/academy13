﻿using System.Text;

namespace BinaryStudio.PhotoGallery.Core.PathUtils
{
    internal class UrlUtil : IUrlUtil
    {
        public string BuildPhotoViewUrl(int photoId)
        {
            const string PHOTO_PART = "photo";

            return BuildPathByPattern(PHOTO_PART, photoId);
        }

        public string BuildAlbumViewUrl(int albumId)
        {
            const string ALBUM_PART = "album";

            return BuildPathByPattern(ALBUM_PART, albumId);
        }

        public string BuildUserViewUrl(int userId)
        {
            const string USER_PART = "user";

            return BuildPathByPattern(USER_PART, userId);
        }

        public string BuildCommentUrl(int photoId, int commentId)
        {
            const string COMMENT_DELIMITER = "#";

            var url = new StringBuilder(BuildPhotoViewUrl(photoId));

            url.Append(COMMENT_DELIMITER)
                .Append(commentId);

            return url.ToString();
        }

        private string BuildPathByPattern(string pathPart, int id)
        {
            const string DELIMITER = "/";

            var url = new StringBuilder();

            url.Append(DELIMITER)
               .Append(pathPart)
               .Append(DELIMITER)
               .Append(id);

            return url.ToString();
        }
    }
}