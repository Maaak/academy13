﻿using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IUserService
    {
        IEnumerable<UserModel> GetAllUsers(); 
        
        UserModel GetUser(string userEmail);

        void CreateUser(UserModel userModel, AuthInfoModel.ProviderType providerType = AuthInfoModel.ProviderType.Local);

        void DeleteUser(string userEmail);

        bool IsUserValid(string userEmail, string userPassword);

        bool IsUserExist(string userEmail);

        bool IsUserExist(string authProvider, string token);
    }
}