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
    using BinaryStudio.PhotoGallery.Web.ViewModels;

    [Authorize] // Only authorized users can access this controller
	[RoutePrefix("Home")]
    public class HomeController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IUserService _userService;
        private readonly IModelConverter _modelConverter;

        public HomeController(IUserService userService, IPhotoService photoService, IModelConverter modelConverter)
        {
            _photoService = photoService;
            _userService = userService;
            _modelConverter = modelConverter;
        }
        /// <summary>
        /// Main user page (click on "bingally")
        /// </summary>
        /// <returns>page with flow of public pictures</returns>
        [GET("Index/{photoNum}")]
        public ActionResult Index()
        {   
            var photoModels = _photoService.GetPhotos(User.Identity.Name, 0, 30);

            var infoViewModel = new InfoViewModel
                {
                    UserEmail = User.Identity.Name,
                    Photos = photoModels.Select(_modelConverter.GetViewModel).ToList()
                };

            return View(infoViewModel);
        }

        [HttpPost]
        public ActionResult GetPhotosViaAjax(int startIndex, int endIndex)
        {
            var photos = _photoService.GetPhotos(User.Identity.Name, startIndex, endIndex)
                                  .Select(_modelConverter.GetViewModel);
            return Json(photos);
        }

        /// <summary>
        /// Gallery page
        /// </summary>
        /// <returns>page with all users photos, sorted by date</returns>
        [GET("Gallery")]
        public ActionResult Gallery()
        {
            return View();
        }

        /// <summary>
        /// Album page
        /// </summary>
        /// <returns>page with all users albums</returns>
        [GET("Albums")]
        public ActionResult Albums()
        {
            return View();
        }

        /// <summary>
        /// Gruops page
        /// </summary>
        /// <returns>page with all users groups</returns>
        [GET("Groups")]
        public ActionResult Groups()
        {
            return View();
        }
    }
}
