﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryStudio.PhotoGallery.Database
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        

        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
