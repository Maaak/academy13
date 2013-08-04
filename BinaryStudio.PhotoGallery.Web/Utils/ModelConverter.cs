﻿using BinaryStudio.PhotoGallery.Core.PathUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Web.ViewModels.PhotoPage;

namespace BinaryStudio.PhotoGallery.Web.Utils
{
    public static class ModelConverter
    {
        public static UserModel GetModel(RegistrationViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    UserPassword = viewModel.Password
                };

            return userModel;
        }

        public static UserModel GetModel(AuthorizationViewModel viewModel)
        {
            var userModel = new UserModel
                {
                    Email = viewModel.Email,
                    UserPassword = viewModel.Password
                };

            return userModel;
        }

        public static PhotoViewModel TestGetViewModel(PhotoModel model)
        {
            var photoModel = new PhotoViewModel
                {
                PhotoThumbSource = DeprecatedPathUtil.BuildThumbnailsPath(model.UserModelId, model.AlbumModelId)
                                       + "/" + model.PhotoName,
                PhotoSource = DeprecatedPathUtil.BuildAlbumPath(model.UserModelId, model.AlbumModelId)
                                  + "/" + model.PhotoName,
                    AlbumId = model.AlbumModelId,
                    PhotoId = model.Id
                };

            return photoModel;
        }

        public static PhotoViewModel GetViewModel(PhotoModel photoModel)
        {
            return new PhotoViewModel
                {
                    PhotoThumbSource =
                        DeprecatedPathUtil.BuildThumbnailsPath(photoModel.UserModelId, photoModel.AlbumModelId)
                        + "/" + photoModel.PhotoName,
                    PhotoSource = DeprecatedPathUtil.BuildAlbumPath(photoModel.UserModelId, photoModel.AlbumModelId)
                                  + "/" + photoModel.PhotoName,
                    AlbumId = photoModel.AlbumModelId,
                    PhotoId = photoModel.Id
                };
        }

        public static PhotoCommentViewModel GetViewModel(PhotoCommentModel photoCommentModel, UserModel userModel)
        {
            return new PhotoCommentViewModel
                {
                    UserInfo = new UserInfoViewModel()
                        {
                            OwnerFirstName = userModel.FirstName,
                            OwnerLastName = userModel.LastName
                        },
                        Rating   = photoCommentModel.Rating,
                        DateOfCreating = photoCommentModel.DateOfCreating,
                        // this shit needs fixing
                        Reply = photoCommentModel.Reply,
                        Text = photoCommentModel.Text
                };
        }
    }
}