﻿using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Database;
using BinaryStudio.PhotoGallery.Models;

namespace BinaryStudio.PhotoGallery.Domain.Services
{
    public interface IAlbumService
    {
        /// <summary>
        ///     Gets album id specified by his name.
        /// </summary>
        int GetAlbumId(int userId, string albumName);

        /// <summary>
        ///     Gets user's album specified by id.
        /// </summary>
        /// <param name="albumId">Album Id</param>
        AlbumModel GetAlbum(int albumId);

        int AlbumsCount(int userId);

        IEnumerable<AlbumModel> GetAlbumsRange(int userId, int skipCount, int takeCount);

        IEnumerable<AlbumTagModel> GetTags(int albumId);

        /// <summary>
        ///     Gets all albums for specified user.
        /// </summary>
        IEnumerable<AlbumModel> GetAllAlbums(int userId);

        /// <summary>
        ///     Creates album for specified user by his email.
        /// </summary>
        AlbumModel CreateAlbum(int userId, AlbumModel album);
        AlbumModel CreateAlbum(int userId, string albumName);

        /// <summary>
        ///     Creates system albums
        /// </summary>
        void CreateSystemAlbums(int userId);

        /// <summary>
        ///     Deletes specified album.
        /// </summary>
        void DeleteAlbum(int userId, int albumId);

        /// <summary>
        ///     Get all available albums for specified user
        /// </summary>
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId);
        IEnumerable<AlbumModel> GetAvailableAlbums(int userId, IUnitOfWork unitOfWork);
    }
}