using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Core.Abstractions.Authentication;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Extensions;
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
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken) =>
            await Result.Create(request)
                .Bind(
                    command => _userRepository.GetByEmailAsync(command.Email),
                    Errors.Authentication.InvalidEmailOrPassword)
                .Ensure(
                    user => user.VerifyPasswordHash(request.Password, _passwordHashChecker),
                    Errors.Authentication.InvalidEmailOrPassword)
                .Bind(user => _jwtProvider.CreateAsync(user));
    }
}
