using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests.Error
{
    public sealed class ErrorTests
    {
        [Fact]
        public void Error_codes_must_be_unique()
        {
            List<PropertyInfo> properties = typeof(Errors)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .SelectMany(x => x.GetProperties(BindingFlags.Public | BindingFlags.Static))
                .Where(x => x.PropertyType == typeof(Core.Primitives.Error))
                .ToList();

            int numberOfUniqueCodes = properties.Select(GetErrorCode)
                .Distinct()
                .Count();

            properties.Count.Should().Be(numberOfUniqueCodes);
        }

        private static string GetErrorCode(PropertyInfo property)
        {
            MethodInfo? propertyGetMethod = property.GetMethod;

            if (propertyGetMethod is null)
            {
                throw new Exception();
            }

            object[] parameters = propertyGetMethod!.GetParameters()
                .Select<ParameterInfo, object>(x =>
                {
                    if (x.ParameterType == typeof(string))
                    {
                        return string.Empty;
                    }

                    if (x.ParameterType == typeof(long))
                    {
                        return 0;
                    }

                    throw new Exception();
                })
                .ToArray();

            var error = (Core.Primitives.Error)propertyGetMethod.Invoke(null, parameters)!;

            return error!.Code;
        }
    }
}