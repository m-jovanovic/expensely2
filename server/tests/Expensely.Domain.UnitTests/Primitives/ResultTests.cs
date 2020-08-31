using System;
using Expensely.Domain.Core;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Primitives
{
    public class ResultTests
    {
        [Fact]
        public void Should_be_successful_when_created_with_ok()
        {
            var result = Result.Success();

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().Be(Error.None);
        }

        [Fact]
        public void Should_be_successful_when_created_with_ok_and_value()
        {
            const string value = "value";
            var result = Result.Success(value);

            result.IsFailure.Should().BeFalse();
            result.IsSuccess.Should().BeTrue();
            result.Error.Should().Be(Error.None);
            result.Value().Should().Be(value);
        }

        [Fact]
        public void Should_be_failure_when_created_with_fail()
        {
            var result = Result.Failure(Errors.General.ServerError);

            result.IsFailure.Should().BeTrue();
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(Errors.General.ServerError);
        }

        [Fact]
        public void Should_throw_invalid_operation_exception_when_attempting_to_retrieve_value_of_failure_result()
        {
            Result<Result> result = Result.Failure<Result>(Errors.General.ServerError);

            result.Invoking(r => r.Value()).Should().Throw<InvalidOperationException>();
        }
    }
}
