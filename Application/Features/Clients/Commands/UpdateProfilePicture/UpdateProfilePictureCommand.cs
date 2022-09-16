using Application.Exceptions;
using Application.Interfaces.Repositories;
using Application.Wrappers;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System;

namespace Application.Features.Clients
{
    public class UpdateProfilePictureCommand : IRequest<Response<string>>
    {
        public int Id { get; set; }
        public  IFormFile profileImage { get; set; }


        public class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, Response<string>>
        {
            private readonly IClientRepositoryAsync _clientRepository;
            private readonly IImageRepository _imageRepository;

            public UpdateProfilePictureCommandHandler(IClientRepositoryAsync clientRepository, IImageRepository imageRepository)
            {
                _clientRepository = clientRepository;
                _imageRepository = imageRepository;
            }
            public async Task<Response<string>> Handle(UpdateProfilePictureCommand command, CancellationToken cancellationToken)
            {
                var client = await _clientRepository.GetByIdAsync(command.Id);

                if (client == null)
                {
                    throw new ApiException($"Client Not Found.");
                }
                else
                {

                    if (command.profileImage != null && command.profileImage.Length > 0)
                    {
                        var validExtensions = new List<string>
                        {
                           ".jpeg",
                           ".png",
                           ".gif",
                           ".jpg"
                        };
                        var extension = Path.GetExtension(command.profileImage.FileName);
                        if (validExtensions.Contains(extension))
                        {
                            var fileName = Guid.NewGuid() + Path.GetExtension(command.profileImage.FileName);

                            var fileImagePath = await _imageRepository.Upload(command.profileImage, fileName);

                            if (await _clientRepository.UpdateProfileImage(command.Id, fileImagePath))
                            {
                                return new Response<string>(fileImagePath, $"Image Uploaded Successfully");
                            }
                        }
                        else
                        {
                            throw new ApiException($"Invalid file extension.");
                        }
                    }

                    throw new ApiException($"Please upload a valid image");
                }
            }
        }
    }
}
