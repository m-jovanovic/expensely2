using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Users;
using Expensely.Domain.Users.Services;

namespace Expensely.Application.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> handler.
    /// </summary>
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHashChecker _passwordHashChecker;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="passwordHashChecker">The password hash checker.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public LoginCommandHandler(IUserRepository userRepository, IPasswordHashChecker passwordHashChecker, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHashChecker = passwordHashChecker;
        }

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            User? user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return Result.Failure<TokenResponse>(Errors.Authentication.InvalidEmailOrPassword);
            }

            if (!user.VerifyPasswordHash(request.Password, _passwordHashChecker))
            {
                return Result.Failure<TokenResponse>(Errors.Authentication.InvalidEmailOrPassword);
            }

            TokenResponse tokenResponse = await _jwtProvider.CreateAsync(user);

            return Result.Success(tokenResponse);
        }
    }
}
