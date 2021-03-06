﻿using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Search;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Domain.Utils;
using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Domain
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUserService, UserService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAlbumService, AlbumService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPhotoService, PhotoService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPhotoCommentService, PhotoCommentService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICleanupTask, CleanupTask>(new ContainerControlledLifetimeManager());
            container.RegisterType<IStorage, Storage>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUsersMonitorTask, UsersMonitorTask>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchService, SearchService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPhotoSearchService, PhotoSearchService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IUserSearchService, UserSearchService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IAlbumSearchService, AlbumSearchService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICommentSearchService, CommentSearchService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchCacheTask, SearchCacheTask>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISecureService, SecureService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IGroupService, GroupService>(new ContainerControlledLifetimeManager());
			container.RegisterType<ITagService, TagService>(new ContainerControlledLifetimeManager());
        }
    }
}