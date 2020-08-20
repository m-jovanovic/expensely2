using System;
using System.Threading;
using System.Threading.Tasks;
using Expensely.Application.Abstractions.Cryptography;
using Expensely.Application.Abstractions.Messaging;
using Expensely.Application.Abstractions.Repositories;
using Expensely.Domain;
using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Entities;
using Expensely.Domain.ValueObjects;

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
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            Result<User> userResult = CreateUser(request);

            if (userResult.IsFailure)
            {
                return Result.Fail(userResult.Error);
            }

            User user = userResult.Value();

            bool isUnique = await _userRepository.IsEmailUniqueAsync(user.Email);

            if (!isUnique)
            {
                return Result.Fail(Errors.Authentication.DuplicateEmail);
            }

            // TODO: Add role(s) to user.
            _userRepository.Insert(user);

            return Result.Ok();
        }

        /// <summary>
        /// Creates the user entity based on the specified command.
        /// </summary>
        /// <param name="command">The register command.</param>
        /// <returns>The result of the user creation process containing the user or an error.</returns>
        private Result<User> CreateUser(RegisterCommand command)
        {
            Result<FirstName> firstNameResult = FirstName.Create(command.FirstName);

            if (firstNameResult.IsFailure)
            {
                return Result.Fail<User>(firstNameResult.Error);
            }

            Result<LastName> lastNameResult = LastName.Create(command.LastName);

            if (lastNameResult.IsFailure)
            {
                return Result.Fail<User>(lastNameResult.Error);
            }

            Result<Email> emailResult = Email.Create(command.Email);

            if (emailResult.IsFailure)
            {
                return Result.Fail<User>(emailResult.Error);
            }

            Result<Password> passwordResult = Password.Create(command.Password);

            if (passwordResult.IsFailure)
            {
                return Result.Fail<User>(passwordResult.Error);
            }

            string passwordHash = _passwordHasher.HashPassword(passwordResult.Value());

            var user = new User(
                Guid.NewGuid(),
                firstNameResult.Value(),
                lastNameResult.Value(),
                emailResult.Value(),
                passwordHash);

            return Result.Ok(user);
        }
    }
}
