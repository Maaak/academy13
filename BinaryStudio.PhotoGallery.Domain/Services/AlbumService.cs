﻿using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class AlbumService : DbService, IAlbumService
    {
        public AlbumService(IUnitOfWorkFactory workFactory) : base(workFactory)
        {
        }

        public void CreateAlbum(string userEmail, AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                user.Albums.Add(album);

                unitOfWork.SaveChanges();
            }
        }

        public void CreateAlbum(string userEmail, string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                var album = new AlbumModel
                {
                    AlbumName = albumName,
                    OwnerId = user.Id
                };

                user.Albums.Add(album);

                unitOfWork.SaveChanges();
            }
        }

        public void DeleteAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumId, unitOfWork);

                album.IsDeleted = true;

                unitOfWork.SaveChanges();
            }
        }

        public AlbumModel GetAlbum(int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Albums.Find(albumId);
            }
        }

        public IEnumerable<AlbumModel> GetAllAlbums(string userEmail)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                return user.Albums.Where(model => !model.IsDeleted).ToList();
            }
        }

        public AlbumModel GetAlbum(string userEmail, int albumId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                AlbumModel album = GetAlbum(albumId);

                if (album.OwnerId == user.Id)
                {
                    return album;
                }
                
                throw new AlbumNotFoundException();
            }
        }

        public int GetAlbumId(string albumName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var foundAlbum = unitOfWork.Albums.Find(album => album.AlbumName == albumName);

                if (foundAlbum == null)
                {
                    throw new AlbumNotFoundException();
                }

                return foundAlbum.Id;
            }
        }

        public bool IsExist(string albumName)
        {
            try
            {
                this.GetAlbumId(albumName);
            }
            catch (AlbumNotFoundException)
            {
                return false;
            }

            return true;
        }
    }
}