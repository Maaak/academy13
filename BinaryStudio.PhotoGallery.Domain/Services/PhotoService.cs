﻿using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class PhotoService : DbService, IPhotoService
    {
        private readonly ISecureService _secureService;

        public PhotoService(IUnitOfWorkFactory workFactory, ISecureService secureService) : base(workFactory)
        {
            _secureService = secureService;
        }

        public void AddPhoto(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.UserId, photo.UserId, unitOfWork);

                album.Photos.Add(photo);

                unitOfWork.SaveChanges();
            }
        }

        public void AddPhoto(string userEmail, string albumName, PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {

                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    photo.UserId = user.Id;
                    album.Photos.Add(photo);

                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to photos", null);
                }
            }
        }

        public void AddPhotos(string userEmail, string albumName, IEnumerable<PhotoModel> photos)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserAddPhoto(user.Id, album.Id))
                {
                    foreach (var photo in photos)
                    {
                        album.Photos.Add(photo);
                    }
                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to photos", null);
                }
            }
        }

        public void DeletePhoto(string userEmail, PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = GetUser(userEmail, unitOfWork);

                if (_secureService.CanUserDeletePhoto(user.Id, photo.Id))
                {
                    var photoToDelete = unitOfWork.Photos.Find(model => model.Id == photo.Id);
                    photoToDelete.IsDeleted = true;

                    unitOfWork.SaveChanges();
                }
                else
                {
                    throw new NoEnoughPrivileges("User can't get access to photos", null);
                }

            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, string albumName, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);
                AlbumModel album = GetAlbum(user, albumName, unitOfWork);

                if (_secureService.CanUserViewPhotos(user.Id, album.Id))
                {
                    return album.Photos.OrderBy(model => model.DateOfCreation)
                                .ThenBy(model => model.Id)
                                .Skip(skipCount)
                                .Take(takeCount)
                                .ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int albumId, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

                if (_secureService.CanUserViewPhotos(user.Id, albumId))
                {
                    return unitOfWork.Photos.Filter(model => model.AlbumId == albumId)
                                     .Where(model => !model.IsDeleted)
                                     .OrderByDescending(model => model.DateOfCreation)
                                     .ThenBy(model => model.Id)
                                     .Skip(skipCount)
                                     .Take(takeCount)
                                     .ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<PhotoModel> GetPhotos(string userEmail, int skipCount, int takeCount)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(userEmail, unitOfWork);

// todo: create a criterions for grabing all user albums' IDs and check every for access permissions
                return unitOfWork.Photos.Filter(model => model.UserId == user.Id)
                                 .Where(model => !model.IsDeleted)
                                 .OrderByDescending(model => model.DateOfCreation)
                                 .ThenBy(model => model.Id)
                                 .Skip(skipCount)
                                 .Take(takeCount)
                                 .ToList();
            }
        }

        public PhotoModel GetPhoto(string userEmail, int photoID)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = GetUser(userEmail, unitOfWork);
                var photo = unitOfWork.Photos.Find(photoID);

                if (_secureService.CanUserViewPhotos(user.Id, photo.AlbumId))
                {
                    return photo;
                }

                throw new NoEnoughPrivileges("User can't get access to photos", null);
            }
        }

        public IEnumerable<UserModel> GetLikes(string userEmail, int photoID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = GetUser(userEmail, unitOfWork);
                var photo = unitOfWork.Photos.Find(photoID);

                if (_secureService.CanUserViewLikes(user.Id, photo.AlbumId))
                {
                    return photo.Likes.ToList();
                }

                throw new NoEnoughPrivileges("User can't get access to photo's likes", null);
            }
        }

        public void AddLike(string userEmail, int photoID)
        {
            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = GetUser(userEmail, unitOfWork);

                unitOfWork.Photos.Find(photoID).Likes.Add(user);
                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<PhotoModel> GetPublicPhotos(string userEmail, int skipCount, int takeCount)
        {

            using (var unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = GetUser(userEmail, unitOfWork);

                var publicPhotos = _secureService.GetAvailableAlbums(user.Id, unitOfWork)
                                                 .SelectMany(it => it.Photos)
                                                 .Where(photo => !photo.IsDeleted)
                                                 .OrderByDescending(photo => photo.DateOfCreation)
                                                 .ThenBy(photo => photo.Id)
                                                 .Skip(skipCount)
                                                 .Take(takeCount);
                return publicPhotos;
            }
        }
    }
}