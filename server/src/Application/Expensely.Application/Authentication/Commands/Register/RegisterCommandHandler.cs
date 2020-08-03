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
            Result<Password> passwordResult = Password.Create(request.Password);

            if (passwordResult.IsFailure)
            {
                return Result.Fail(passwordResult.Error);
            }

            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Fail(emailResult.Error);
            }

            Email email = emailResult.Value();

            bool isUnique = await _userRepository.IsUniqueAsync(email);

            if (!isUnique)
            {
                return Result.Fail(Errors.Authentication.DuplicateEmail);
            }

            string passwordHash = _passwordHasher.HashPassword(passwordResult.Value());

            var user = new User(Guid.NewGuid(), request.FirstName, request.LastName, email, passwordHash);

            // TODO: Add role to user.
            _userRepository.Insert(user);

            return Result.Ok();
        }
    }
}
