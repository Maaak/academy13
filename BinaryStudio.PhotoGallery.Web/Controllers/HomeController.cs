﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.ViewModels;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize] // Only authorized user can access this controller
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;

        public HomeController(IUserService userService, IPhotoService photoService)
        {
            _photoService = photoService;
            _userService = userService;
        }
        /// <summary>
        /// Main user page
        /// </summary>
        /// <returns>Return page with flow of pictures</returns>
		[GET]
        public ActionResult Index()
        {
            // for example&test get 20 photos
            var viewmodels = _photoService.GetPhotos(User.Identity.Name, 0, 20);
            List<PhotoViewModel> photos = viewmodels.Select(ModelConverter.GetViewModel).ToList();
            return View(photos);
        }

        [GET]
        public ActionResult Settings()
        {
            return View();
        }
    }
}
