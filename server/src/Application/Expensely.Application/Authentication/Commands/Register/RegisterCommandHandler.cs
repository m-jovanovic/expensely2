using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Cryptography;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Extensions;
using Expensely.Domain.Users;

namespace Expensely.Application.Authentication.Commands.Register
{
    /// <summary>
    /// Represents the <see cref="RegisterCommand"/> handler.
    /// </summary>
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken) =>
            await CreateUser(request)
                .Ensure(user => _userRepository.IsEmailUniqueAsync(user.Email), Errors.Authentication.DuplicateEmail)
                .Tap(user =>
                {
                    // TODO: Add role(s) to user.
                    _userRepository.Insert(user);
                });

        /// <summary>
        /// Creates the user entity based on the specified command.
        /// </summary>
        /// <param name="command">The register command.</param>
        /// <returns>The result of the user creation process containing the user or an error.</returns>
        private Result<User> CreateUser(RegisterCommand command)
        {
            Result<FirstName> firstNameResult = FirstName.Create(command.FirstName);
            Result<LastName> lastNameResult = LastName.Create(command.LastName);
            Result<Email> emailResult = Email.Create(command.Email);
            Result<Password> passwordResult = Password.Create(command.Password);

            return Result.FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult)
                .Map(() => Result.Success(
                    new User(
                        Guid.NewGuid(),
                        firstNameResult.Value(),
                        lastNameResult.Value(),
                        emailResult.Value(),
                        _passwordHasher.HashPassword(passwordResult.Value()))));
        }
    }
}
