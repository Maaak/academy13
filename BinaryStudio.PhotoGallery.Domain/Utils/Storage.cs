﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Utils
{
    internal class Storage : IStorage
    {
        private readonly IUnitOfWorkFactory workFactory;
        private IPathUtil pathUtil;

        public IPathUtil PathUtil { set { pathUtil = value; }}

        public Storage(IUnitOfWorkFactory workFactory, IPathUtil pathUtil)
        {
            this.workFactory = workFactory;
            this.pathUtil = pathUtil;
        }

        public string GetAlbumPath(AlbumModel album)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                UserModel user = GetUser(album.UserModelId, unitOfWork);

                return pathUtil.BuildAlbumPath(user.Id, album.Id);
            }
        }

        public string GetAlbumPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = unitOfWork.Albums.Find(model => model.Id == photo.AlbumModelId);
                UserModel user = unitOfWork.Users.Find(model => model.Id == album.UserModelId);

                return pathUtil.BuildAlbumPath(user.Id, album.Id);
            }
        }

        public string GetOriginalPhotoPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.AlbumModelId, unitOfWork);
                UserModel user = GetUser(album.UserModelId, unitOfWork);

                return pathUtil.BuildOriginalPhotoPath(user.Id, album.Id, photo.Id, photo.Format);
            }
        }

        public IEnumerable<string> GetThumbnailDirectoryPath(PhotoModel photo)
        {
            string thumbnailDirectoryPath = GetThumbnailsPath(photo);

            return Directory.EnumerateDirectories(thumbnailDirectoryPath);
        }

        public IEnumerable<string> GetTemporaryDirectoriesPathes()
        {
            string photoDirectoryPath = pathUtil.BuildPhotoDirectoryPath();
            IEnumerable<string> usersDirectories = Directory.EnumerateDirectories(photoDirectoryPath);

            var temporaryPhotosDirectories = new Collection<string>();

            foreach (var userDirectory in usersDirectories)
            {
                string temporaryPhotosDirectory = pathUtil.BuildTemporaryDirectoryPath(userDirectory);
                temporaryPhotosDirectories.Add(temporaryPhotosDirectory);
            }

            return temporaryPhotosDirectories;
        }

        private string GetThumbnailsPath(PhotoModel photo)
        {
            using (IUnitOfWork unitOfWork = workFactory.GetUnitOfWork())
            {
                AlbumModel album = GetAlbum(photo.AlbumModelId, unitOfWork);
                UserModel user = GetUser(album.UserModelId, unitOfWork);

                return pathUtil.BuildThumbnailsPath(user.Id, album.Id);
            }
        }

        private UserModel GetUser(int userId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Users.Find(model => model.Id == userId);
        }

        private AlbumModel GetAlbum(int albumId, IUnitOfWork unitOfWork)
        {
            return unitOfWork.Albums.Find(model => model.Id == albumId && !model.IsDeleted);
        }
    }
}
