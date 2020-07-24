using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Messaging;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.ValueObjects;

namespace Expensely.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Represents the <see cref="RegisterCommand"/> handler.
    /// </summary>
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="authenticationService">The authentication service.</param>
        public RegisterCommandHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return Result.Fail<TokenResponse>(Errors.Authentication.PasswordsDoNotMatch);
            }

            Result<Password> passwordResult = Password.Create(request.Password);

            if (passwordResult.IsFailure)
            {
                return Result.Fail<TokenResponse>(passwordResult.Error);
            }

            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Fail<TokenResponse>(emailResult.Error);
            }

            Email email = emailResult.Value();

            bool isUnique = await _userRepository.IsUniqueAsync(email);

            if (!isUnique)
            {
                return Result.Fail<TokenResponse>(Errors.Authentication.DuplicateEmail);
            }

            Result<TokenResponse> result = await _authenticationService.RegisterAsync(
                request.FirstName,
                request.LastName,
                email,
                passwordResult.Value());

            return result;
        }
    }
}
