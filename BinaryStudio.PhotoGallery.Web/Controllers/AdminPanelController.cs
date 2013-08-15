﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.NotificationsUtils;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Domain.Services.Tasks;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
	[RoutePrefix("AdminPanel")]
	public class AdminPanelController : Controller
	{
		private readonly IUserService userService;
		private readonly IUsersMonitorTask monitorService;

		public AdminPanelController(IUserService userService, IUsersMonitorTask monitorTask)
		{
			this.userService = userService;
			this.monitorService = monitorTask;
		}

		/// <summary>
		/// Administration page
		/// </summary>
		[GET("")]
		public ActionResult Index()
		{
			var users = this.userService.GetAllUsers().ToList();
			var extendedUserList = users.Select(user => new UserViewModel
				{
					IsOnline = this.monitorService.IsOnline(user.Email), 
					FirstName = user.FirstName, 
					LastName = user.LastName,
					Email = user.Email,
					IsAdmin = user.IsAdmin,
					AlbumsCount = user.Albums.Count
				}).ToList();
			return View(new UsersListViewModel { UserViewModels = extendedUserList});
		}

		[DELETE]
		public HttpResponseMessage DeleteUser(string eMail)
		{
			userService.DeleteUser(eMail);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		[POST]
		public HttpResponseMessage SendInvite(UserViewModel invitedUser)
		{
			var sender = new NotificationSender();
			var host = ConfigurationManager.AppSettings["EmailHost"];
			var fromEmail = ConfigurationManager.AppSettings["SenderEmail"];
			var fromPass = ConfigurationManager.AppSettings["SenderPass"];
			var toEmail = invitedUser.Email;
			var activationLink = "http://localhost:57367/Authorization/Signup#" +
			                     userService.CreateUser(invitedUser.Email, invitedUser.FirstName, invitedUser.LastName);
			var text = string.Format("Dear {0} {1}!\n\nYou have been invited to the great photogallery project" +
			                         " of Binary Studio! For the end of registration, please click on this link:\n{2} ",
									 invitedUser.FirstName,invitedUser.LastName,activationLink);

			sender.Send(host,fromEmail,fromPass,toEmail,text);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
