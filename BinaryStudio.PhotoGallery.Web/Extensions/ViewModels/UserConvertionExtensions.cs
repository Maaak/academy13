using BinaryStudio.PhotoGallery.Core.PhotoUtils;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels.User;

namespace BinaryStudio.PhotoGallery.Web.Extensions.ViewModels
{
    public static class UserConvertionExtensions
    {
        public static UserViewModel ToUserViewModel(this UserModel model, bool isBlocked)
        {
            var viewModel = new UserViewModel
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Department = model.Department,
                Birthday = model.Birthday,
                IsActivated = model.IsActivated,
                IsBlocked = isBlocked
            };
            viewModel.PhotoUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Medium);
            return viewModel;
        }
        public static UserViewModel ToNoneUserViewModel(this UserModel model)
        {
            var viewModel = new UserViewModel
            {
                FirstName = "None",
                LastName = "None",
                AlbumsCount = 0,
                PhotoCount = 0
            };
            viewModel.PhotoUrl = viewModel.PathUtil.BuildAvatarPath(model.Id, ImageSize.Medium);
            return viewModel;
        }
        public static UserViewModel ToUserViewModel(this UserModel model, int photoCount, int albumCount)
        {
            var viewModel = model.ToUserViewModel(false);
            viewModel.AlbumsCount = albumCount;
            viewModel.PhotoCount = photoCount;
            return viewModel;
        }
    }
}