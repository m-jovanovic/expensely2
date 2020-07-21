using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Messaging;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Application.Authentication.Commands.Register
{
    public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Fail<string>(emailResult.Error);
            }

            Email email = emailResult.Value();

            bool isUnique = await _userRepository.IsUniqueAsync(email);

            if (!isUnique)
            {
                return Result.Fail<string>(Errors.Authentication.DuplicateEmail);
            }

            Result<string> result = await _authenticationService.RegisterAsync(
                request.FirstName,
                request.LastName,
                email,
                request.Password);

            return result;
        }
    }
}
