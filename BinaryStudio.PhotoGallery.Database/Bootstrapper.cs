﻿using Microsoft.Practices.Unity;

namespace BinaryStudio.PhotoGallery.Database
{
    public static class Bootstrapper
    {
        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new ContainerControlledLifetimeManager());
        }
    }
}
