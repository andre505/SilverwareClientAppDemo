using Application.Features.Clients;
using Application.Interfaces.Repositories;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Clients
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private readonly IClientRepositoryAsync _clientRepository;

        public CreateClientCommandValidator(IClientRepositoryAsync clientRepository)
        {
            this._clientRepository = clientRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueClient).WithMessage("{PropertyName} already exists.");

            //RuleFor(p => p.EmployeeUserId)
            //    .NotEmpty().WithMessage("{PropertyName} is required.")
            //    .NotNull().WithMessage("{PropertyName} must not exceed 50 characters.");

            RuleFor(p => p.EmailAddress)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .NotNull().WithMessage("{PropertyName} must not exceed 50 characters.").EmailAddress().WithMessage("A valid email is required");
        }

        private async Task<bool> IsUniqueClient(string ClientName, CancellationToken cancellationToken)
        {
            return await _clientRepository.IsUniqueClientAsync(ClientName);
        }
    }
}
