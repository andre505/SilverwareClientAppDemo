﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IImageRepository
    {
        Task<string> Upload(IFormFile file, string fileName);
    }
}
