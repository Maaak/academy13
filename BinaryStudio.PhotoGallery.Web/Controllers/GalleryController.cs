﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Web.ViewModels;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("gallery")]
    public class GalleryController : BaseController
    {
        [GET("")]
        public ActionResult Index()
        {
            return View(new PublicPhotosViewModel { UserId = User.Id });
        }
    }
}
