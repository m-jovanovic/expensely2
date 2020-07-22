﻿using Expensely.Domain.Core.Primitives;
using Expensely.Domain.Core.Validators;

namespace Expensely.Domain.Validators.Password
{
    public class PasswordNotNullOrEmptyValidator : Validator<string>
    {
        public override Result Validate(string? request) =>
            string.IsNullOrWhiteSpace(request) ? Result.Fail(Errors.Password.NullOrEmpty) : base.Validate(request);
    }
}
