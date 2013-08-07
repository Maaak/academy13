﻿using System.Linq;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using BinaryStudio.PhotoGallery.Core;
using BinaryStudio.PhotoGallery.Core.SocialNetworkUtils.Facebook;
using BinaryStudio.PhotoGallery.Domain.Services;
using BinaryStudio.PhotoGallery.Models;
using BinaryStudio.PhotoGallery.Web.Utils;

namespace BinaryStudio.PhotoGallery.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Photo")]
    public class PhotoController : Controller
    {
        private readonly IPhotoService _photoService;
        private readonly IModelConverter _modelConverter;

        public PhotoController(IPhotoService photoService, IModelConverter modelConverter)
        {
            _photoService = photoService;
            _modelConverter = modelConverter;
        }

        [HttpPost]
        public ActionResult GetPhoto(int photoID, int offset)
        {
            var photoModel = _photoService.GetPhoto(photoID);

            if (offset != 0)
            {
                photoModel =
                    _photoService.GetPhotos(User.Identity.Name, photoModel.AlbumModelId, photoID + offset,
                                            photoID + offset + 1).ToList()[0];
            }


            return Json(_modelConverter.GetViewModel(photoModel));
        }

        [HttpPost]
        public ActionResult GetPhotos(string albumName, int begin, int last)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumName, begin, last);

            return Json(photoModels.Select(model => _modelConverter.GetViewModel(model)).ToList());
        }

        [POST]
        public ActionResult GetPhotosIDFromAlbum(int albumID, int begin, int end)
        {
            var photoModels = _photoService.GetPhotos(User.Identity.Name, albumID, begin, end);

            return Json(photoModels.Select(model => _modelConverter.GetViewModel(model)).ToList());
        }

        [HttpPost]
        public ActionResult FbSync(string photoID)
        {
           /* var photoModel = _photoService.GetPhoto(Int32.Parse(photoID));
            var photoPath = new List<string>();
*/
            return Redirect(FB.CreateAuthURL(Randomizer.GetString(16)));
            //FB.AddPhotosToAlbum(photoPath,"MakTest",);
        }
        [GET("{photoID}")]
        public ActionResult Index(string photoID)
        {
            ViewBag.PhotoID = photoID;
            return View();
        }
    }
}