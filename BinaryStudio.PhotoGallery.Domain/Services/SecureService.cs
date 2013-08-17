﻿using System;
using System.Collections.Generic;
using System.Linq;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class SecureService : DbService, ISecureService
    {
        public SecureService(IUnitOfWorkFactory workFactory)
            : base(workFactory)
        {
        }

        public bool CanUserViewComments(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeComments;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserAddComment(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddComments;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserViewPhotos(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeePhotos;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserAddPhoto(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanAddPhotos;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public bool CanUserDeletePhoto(int userId, int photoId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                var user = unitOfWork.Users.Find(userId);
                var photo = unitOfWork.Photos.Find(photoId);
                var album = unitOfWork.Albums.Find(photo.AlbumId);

                // if user is album owner OR user is photo owner OR user is admin
                return (album.OwnerId == userId) || (photo.OwnerId == userId) || (user.IsAdmin);
            }
        }

        public bool CanUserViewLikes(int userId, int albumId)
        {
            Predicate<AvailableGroupModel> predicate = group => @group.CanSeeLikes;

            return CanUserDoAction(userId, albumId, predicate);
        }

        public IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork)
        {
            UserModel user = GetUser(userId, unitOfWork);
            List<GroupModel> userGroups = user.Groups.ToList();

            if (user.IsAdmin)
            {
                return unitOfWork.Albums.All().ToList();
            }

            IEnumerable<int> albumIds = unitOfWork.AvailableGroups.All().ToList().Join(userGroups,
                avialableGroupModel => avialableGroupModel.Id,
                groupModel => groupModel.Id, (avialableGroupModel, groupModel) => new
                    {
                        avialableGroupModel.CanSeePhotos, avialableGroupModel .AlbumId
                    })
                .Where(arg => arg.CanSeePhotos)
                .Select(arg => arg.AlbumId)
                .Distinct();

            return albumIds.Select(albumId => GetAlbum(albumId, unitOfWork));
        }

        /// <summary>
        ///     Checks if user take a part in even one group, that have enough permissions to do some action
        /// </summary>
        private bool CanUserDoAction(int userId, int albumId, Predicate<AvailableGroupModel> predicate)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                bool canUser;

                UserModel user = GetUser(userId, unitOfWork);
                AlbumModel album = GetAlbum(albumId, unitOfWork);


                if (user.IsAdmin || (album.OwnerId == userId))
                {
                    canUser = true;
                }
                else
                {
                    List<AvailableGroupModel> availableGropusCanDo =
                        unitOfWork.Albums.Find(albumId).AvailableGroups.ToList().FindAll(predicate);


                    GroupModel userGroups = unitOfWork.Users.Find(userId).Groups.ToList()
                        .Find(
                            group =>
                                availableGropusCanDo.Find(x => x.GroupId == @group.Id) != null);

                    canUser = userGroups != null;
                }

                return canUser;
            }
        }
    }
}