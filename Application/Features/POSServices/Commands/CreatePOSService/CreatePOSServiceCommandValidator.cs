using Application.Features.Clients;
using Application.Features.POSServices;
using Application.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.POSServices
{
    public class CreatePOSServiceCommandValidator : AbstractValidator<CreatePOSServiceCommand>
    {
        private readonly IPOSServiceRepositoryAsync _posServiceRepository;

        public CreatePOSServiceCommandValidator(IPOSServiceRepositoryAsync posServiceRepository)
        {
            this._posServiceRepository = posServiceRepository;

            RuleFor(p => p.ServiceName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniquePOSService).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.ServiceDescription)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        }

        private async Task<bool> IsUniquePOSService(string PoSServiceId, CancellationToken cancellationToken)
        {
            return await _posServiceRepository.IsUniquePOSServiceAsync(PoSServiceId);
        }
    }
}
