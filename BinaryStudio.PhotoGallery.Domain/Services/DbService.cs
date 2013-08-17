﻿using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal abstract class DbService
    {
        protected readonly IUnitOfWorkFactory WorkFactory;

        protected DbService(IUnitOfWorkFactory workFactory)
        {
            WorkFactory = workFactory;
        }

        protected AlbumModel GetAlbum(int albumId, IUnitOfWork unitOfWork)
        {
            AlbumModel foundAlbum = unitOfWork.Albums.Find(album => album.Id == albumId);

            if (foundAlbum == null)
            {
                throw new AlbumNotFoundException();
            }

            return foundAlbum;
        }

        protected UserModel GetUser(string userEmail, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(model => string.Equals(model.Email, userEmail));

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with email {0} not found", userEmail));
            }

            return foundUser;
        }

        protected UserModel GetUser(int userId, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(user => user.Id == userId);

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with id {0} not found", userId));
            }

            return foundUser;
        }

        protected AlbumModel GetAlbum(int userId, int albumId, IUnitOfWork unitOfWork)
        {
            UserModel foundUser = unitOfWork.Users.Find(user => user.Id == userId);

            if (foundUser == null)
            {
                throw new UserNotFoundException(string.Format("User with id {0} not found", userId));
            }

            try
            {
                return foundUser.Albums.First(album => album.OwnerId == userId && !album.IsDeleted);
            }
            catch
            {
                throw new AlbumNotFoundException();
            }
        }

        protected AlbumModel GetAlbum(UserModel user, string albumName, IUnitOfWork unitOfWork)
        {
            try
            {
                return
                    user.Albums.Select(model => model)
                        .First(model => string.Equals(model.AlbumName, albumName) && !model.IsDeleted);
            }
            catch
            {
                throw new AlbumNotFoundException();
            }
        }

        protected AlbumModel GetAlbum(UserModel user, int albumId, IUnitOfWork unitOfWork)
        {
            try
            {
                return
                    user.Albums.Select(model => model)
                        .First(model => model.Id == albumId && !model.IsDeleted);
            }
            catch
            {
                throw new AlbumNotFoundException();
            }
        }
    }
}