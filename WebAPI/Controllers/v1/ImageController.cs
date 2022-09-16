using Application.Features.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Application.Interfaces;
using Application.Interfaces.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SilverwarePOS.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ImageController : BaseApiController
    {
        IClientRepositoryAsync _clientRepositoryAsync;
        private readonly IImageRepository imageRepository;

        public ImageController(IClientRepositoryAsync clientRepositoryAsync,
            IImageRepository imageRepository)
        {
            this._clientRepositoryAsync = clientRepositoryAsync;
            this.imageRepository = imageRepository;
        }


        //[HttpPost("{clientId}/upload-image")]
        //public async Task<IActionResult> UploadImage([FromRoute] int ClientId, UpdateProfilePictureCommand command)
        //{
        //    if (ClientId != command.Id)
        //    {
        //        return BadRequest();
        //    }
        //    return Ok(await Mediator.Send(command));
        //}

        [HttpPost("{clientId}/upload-image")]
        //[Route("[controller]/{studentId:guid}/upload-image")]
        public async Task<IActionResult> UploadImage([FromRoute] int clientId, IFormFile profileImage)
        {
            var validExtensions = new List<string>
            {
               ".jpeg",
               ".png",
               ".gif",
               ".jpg"
            };

            if (profileImage != null && profileImage.Length > 0)
            {
                var extension = Path.GetExtension(profileImage.FileName);
                if (validExtensions.Contains(extension))
                {
                    var client = await _clientRepositoryAsync.GetByIdAsync(clientId);
                    if (client != null)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);

                        var fileImagePath = await imageRepository.Upload(profileImage, fileName);

                        if (await _clientRepositoryAsync.UpdateProfileImage(clientId, fileImagePath))
                        {
                            return Ok(fileImagePath);
                        }

                        return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading image");
                    }
                }

                return BadRequest("This is not a valid Image format");
            }

            return NotFound();
        }


    }
}
