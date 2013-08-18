﻿using BinaryStudio.PhotoGallery.Core.UserUtils;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Tests.Mocked;
using BinaryStudio.PhotoGallery.Models;
using FluentAssertions;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace BinaryStudio.PhotoGallery.Domain.Tests
{
    [TestFixture]
    internal class UserServiceTest
    {
        private readonly IUserService userService;

        public UserServiceTest()
        {
            IUnityContainer container = Bootstrapper.Initialise();

            var cryptoProvider = container.Resolve<ICryptoProvider>(); // mock? 
            var albumService = container.Resolve<IAlbumService>(); // mock? 
            var unitOfWorkFactory = new TestUnitOfWorkFactory();

            userService = new UserService(unitOfWorkFactory, cryptoProvider, albumService);
        }

        [Test]
        public void UserShoulBeAbsent()
        {
            // body
            bool isExist = userService.IsUserExist("nononono@gmail.com");

            // tear down
            isExist.Should().Be(false);
        }

        [Test]
        public void UserShouldBeAdded()
        {
            // setup
            var userModel = new UserModel
                {
                    Email = "bbb@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Nick",
                    FirstName = "First",
                    LastName = "Last"
                };

            // body
            userService.CreateUser(userModel);
            bool isExist = userService.IsUserExist(userModel.Email);

            // tear down
            isExist.Should().Be(true);
        }

        [Test]
        public void UserShouldBeDeleted()
        {
            // setup
            var userModel = new UserModel
                {
                    Email = "aaa@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Bill",
                    FirstName = "Billy",
                    LastName = "Last"
                };

            // body
            userService.CreateUser(userModel);
            bool isPresentAfterCreation = userService.IsUserExist(userModel.Email);

            userService.DeleteUser(userModel.Email);
            bool isPresentAfterDeleting = userService.IsUserExist(userModel.Email);

            // tear down
            isPresentAfterCreation.Should().Be(true);

            isPresentAfterDeleting.Should().Be(false);
        }

        [Test]
        public void UserShouldBeNotValid()
        {
            // setup
            const string EMAIL_TO_CHECK = "sss@gmail.com";
            const string PASSWORD_TO_CHECK = "uuh ooh";

            var userModel = new UserModel
                {
                    Email = "sss@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Bill",
                    FirstName = "Billy",
                    LastName = "Last"
                };

            // body
            userService.CreateUser(userModel);
            bool isValid = userService.IsUserValid(EMAIL_TO_CHECK, PASSWORD_TO_CHECK);

            // tear down
            isValid.Should().Be(false);
        }

        [Test]
        public void UserShouldBeValid()
        {
            // setup
            const string EMAIL_TO_CHECK = "aaa@gmail.com";
            const string PASSWORD_TO_CHECK = "abc123";

            var userModel = new UserModel
                {
                    Email = "aaa@gmail.com",
                    UserPassword = "abc123",
                    NickName = "Bill",
                    FirstName = "Billy",
                    LastName = "Last"
                };

            // body
            userService.CreateUser(userModel);
            bool isValid = userService.IsUserValid(EMAIL_TO_CHECK, PASSWORD_TO_CHECK);

            // tear down
            isValid.Should().Be(true);
        }
    }
}