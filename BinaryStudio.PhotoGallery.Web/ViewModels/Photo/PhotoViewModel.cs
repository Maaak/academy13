﻿using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Web.ViewModels.Photo
{
    public class PhotoViewModel : BaseViewModel
    {
        public string PhotoSource { get; set; }
        public string PhotoThumbSource { get; set; }
        public string PhotoViewPageUrl { get; set; }
        public int PhotoId { get; set; }
        public int AlbumId { get; set; }

        public static PhotoModel ToModel(int albumId, int userId, string realFileFormat)
        {
            var model = new PhotoModel
            {
                OwnerId = userId,
                AlbumId = albumId,
                Format = realFileFormat
            };

            return model;
        }
    }
}