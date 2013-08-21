﻿using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("upload")]
    public class UploadController : Controller
    {
		[GET("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}