using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Authentication;
using Expensely.Application.Abstractions.Cryptography;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Application.Contracts.Authentication;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;

namespace Expensely.Application.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> handler.
    /// </summary>
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Fail<TokenResponse>(Errors.Authentication.InvalidEmailOrPassword);
            }

            PasswordVerificationResult passwordVerificationResult = _passwordHasher
                .VerifyHashedPassword(user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failure)
            {
                return Result.Fail<TokenResponse>(Errors.Authentication.InvalidEmailOrPassword);
            }

            TokenResponse tokenResponse = await _jwtProvider.CreateAsync(user);

            return Result.Ok(tokenResponse);
        }
    }
}
