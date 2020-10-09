using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Core.Abstractions.Cryptography;
using Expensely.Application.Core.Abstractions.Messaging;
using Expensely.Application.Core.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core;
using Expensely.Domain.Core.Result;
using Expensely.Domain.Core.Result.Extensions;
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
            await CreateUserResult(
                    FirstName.Create(request.FirstName),
                    LastName.Create(request.LastName),
                    Email.Create(request.Email),
                    Password.Create(request.Password),
                    _passwordHasher.HashPassword)
                .Ensure(user => _userRepository.IsEmailUniqueAsync(user.Email), Errors.Authentication.DuplicateEmail)
                .Tap(user =>
                {
                    // TODO: Add role(s) to user.
                    _userRepository.Insert(user);
                });

        /// <summary>
        /// Creates the user entity based on the specified command.
        /// </summary>
        /// <param name="firstNameResult">The first name result.</param>
        /// <param name="lastNameResult">The last name result.</param>
        /// <param name="emailResult">The email result.</param>
        /// <param name="passwordResult">The password result.</param>
        /// <param name="hashPassword">The hash password function.</param>
        /// <returns>The result of the user creation process containing the user or an error.</returns>
        private static Result<User> CreateUserResult(
            Result<FirstName> firstNameResult,
            Result<LastName> lastNameResult,
            Result<Email> emailResult,
            Result<Password> passwordResult,
            Func<Password, string> hashPassword) =>
            Result
                .FirstFailureOrSuccess(firstNameResult, lastNameResult, emailResult, passwordResult)
                .Map(() => Result.Success(
                    new User(
                        Guid.NewGuid(),
                        firstNameResult.Value(),
                        lastNameResult.Value(),
                        emailResult.Value(),
                        hashPassword(passwordResult.Value()))));
    }
}
