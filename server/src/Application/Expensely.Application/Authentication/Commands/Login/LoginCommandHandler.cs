using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions;
using Expensely.Application.Contracts.Authentication;
using Expensely.Application.Messaging;
using Expensely.Domain.Core.Primitives;

namespace Expensely.Application.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> handler.
    /// </summary>
    internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public LoginCommandHandler(IAuthenticationService authenticationService)
            => _authenticationService = authenticationService;

        /// <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
            => await _authenticationService.LoginAsync(request.Email, request.Password);
    }
}
