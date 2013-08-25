﻿using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using BinaryStudio.PhotoGallery.Core.PathUtils;

namespace BinaryStudio.PhotoGallery.Core.PhotoUtils
{
    internal class CollageProcessor : ICollageProcessor
    {
        private const int MAX_HEIGHT = 1024;

        private readonly IPathUtil pathUtil;

        public CollageProcessor(IPathUtil pathUtil)
        {
            this.pathUtil = pathUtil;
        }

        public void CreateCollage(int userId, int albumId, int width, int rows)
        {
            string collagesDirectoryPath = pathUtil.BuildAbsoluteCollagesDirPath(userId, albumId);

            MakeCollage(userId, albumId, width, rows, collagesDirectoryPath);
        }

        private void MakeCollage(int userId, int albumId, int width, int rows, string collagesDirectoryPath)
        {
            int height = rows * MAX_HEIGHT;

            string collagePath = pathUtil.BuildAbsoluteCollagePath(userId, albumId);

            using (Image image = new Bitmap(width, height))
            {
                Graphics graphics = Graphics.FromImage(image);

                SetUpGraphics(graphics);

                IEnumerable<string> thumbnailsPaths = pathUtil.BuildAbsoluteThumbnailsPaths(userId, albumId, ImageSize.Small);

                TileImages(graphics, thumbnailsPaths, width, height);

                Directory.CreateDirectory(collagesDirectoryPath);

                image.Save(collagePath, ImageFormat.Jpeg);
            }
        }

        private void TileImages(Graphics graphics, IEnumerable<string> thumbnails, int width, int heigth)
        {
            int iter = 0;
            int sumWidth = 0;

            foreach (string file in thumbnails)
            {
                using (Image thumbImage = Image.FromFile(file))
                {
                    graphics.DrawImageUnscaled(thumbImage, sumWidth, iter);
                    sumWidth += thumbImage.Width;
                    if (sumWidth >= width)
                    {
                        sumWidth = 0;
                        iter += MAX_HEIGHT;
                        if (iter >= heigth)
                            break;
                    }
                }
            }
        }

        private void SetUpGraphics(Graphics grfx)
        {
            grfx.CompositingQuality = CompositingQuality.HighQuality;
            grfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grfx.SmoothingMode = SmoothingMode.HighQuality;
        }
    }
}