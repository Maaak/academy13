﻿using System;
using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Exceptions;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    internal class GroupService : DbService, IGroupService
    {
        private readonly List<string> systemGroupList = new List<string>
        {
            "BlockedUsers"
        };

        public GroupService(IUnitOfWorkFactory workFactory)
            : base(workFactory)
        {
        }

        //todo: add check permission for creating group event 
        public void Create(int userId, GroupModel groupModel)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                if (unitOfWork.Groups.Find(group => group.GroupName == groupModel.GroupName) != null)
                {
                    throw new GroupAlreadyExistException(groupModel.GroupName);
                }
                if (IsGroupSystem(groupModel))
                {
                    throw new GroupAlreadyExistException(groupModel.GroupName);
                }
                unitOfWork.Groups.Add(groupModel);
                unitOfWork.SaveChanges();
            }
        }

        //todo: add check permission for creating group event 
        public void Create(int userId, string groupName)
        {
            var groupModel = new GroupModel
            {
                GroupName = groupName,
                OwnerId = userId
            };

            try
            {
                Create(userId, groupModel);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message, exception);
            }
        }

        public GroupModel GetGroup(int groupId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    return GetGroup(groupId, unitOfWork);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event 
        public void AddUser(int ownerId, int userId, int groupId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    GroupModel group = GetGroup(groupId);
                    UserModel owner = GetUser(ownerId, unitOfWork);
                    UserModel user = GetUser(userId, unitOfWork);

                    if (group.OwnerId == ownerId || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Find(groupId).Users.Add(user);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event 
        public void AddUser(int ownerId, int userId, string groupName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    GroupModel group = GetGroup(groupName, unitOfWork);

                    AddUser(ownerId, userId, group.Id);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event
        public void RemoveUser(int ownerId, int userId, int groupId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    GroupModel group = GetGroup(groupId);
                    UserModel owner = GetUser(ownerId, unitOfWork);
                    UserModel user = GetUser(userId, unitOfWork);

                    if (group.OwnerId == ownerId || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Find(groupId).Users.Remove(user);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for adding group event
        public void RemoveUser(int ownerId, int userId, string groupName)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    GroupModel group = GetGroup(groupName, unitOfWork);

                    RemoveUser(ownerId, userId, group.Id);
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        //todo: add check permission for deleting group event
        public void Delete(int ownerId, int groupId)
        {
            using (IUnitOfWork unitOfWork = WorkFactory.GetUnitOfWork())
            {
                try
                {
                    GroupModel group = GetGroup(groupId);
                    UserModel owner = GetUser(ownerId, unitOfWork);

                    if (IsGroupSystem(group))
                    {
                        throw new GroupAlreadyExistException(group.GroupName);
                    }

                    if (group.OwnerId == ownerId || owner.IsAdmin)
                    {
                        unitOfWork.Groups.Delete(group);
                        unitOfWork.SaveChanges();
                    }
                }
                catch (Exception exception)
                {
                    throw new Exception(exception.Message, exception);
                }
            }
        }

        private bool IsGroupSystem(GroupModel groupModel)
        {
            return systemGroupList.Contains(groupModel.GroupName);
        }
    }
}