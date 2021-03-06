﻿using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.Photo;

namespace BinaryStudio.PhotoGallery.Web.ViewModels
{
    public class AlbumViewModel
    {
        public int OwnerId { get; set; }
        public int Id { get; set; }

        // todo
        public string CollageSource { get; set; }

        public string AlbumName { get; set; }
        public string Description { get; set; }
        public DateTime DateOfCreation { get; set; }
        public IEnumerable<AlbumTagModel> Tags { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
    }
}