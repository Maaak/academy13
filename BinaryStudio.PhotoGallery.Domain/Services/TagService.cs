﻿using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class TagService : DbService, ITagService
    {
        public TagService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public IEnumerable<AlbumTagModel> GetAlbumTags(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(albumId, unitOfWork);

                return album.Tags.ToList();
            }
        }

        public void SetAlbumTags(int albumId, ICollection<AlbumTagModel> tags)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(albumId, unitOfWork);

                album.Tags = tags;

                unitOfWork.Albums.Update(album);
                unitOfWork.SaveChanges();
            }
        }
    }
}