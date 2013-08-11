﻿using System.Collections.Generic;
using BinaryStudio.PhotoGallery.Domain.Services.Search.FoundItems;

namespace BinaryStudio.PhotoGallery.Domain.Services.Search
{
    public interface ISearchService
    {
        IEnumerable<IFoundItem> Search(SearchArguments searchArguments);
    }
}