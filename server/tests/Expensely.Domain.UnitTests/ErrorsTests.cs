using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Expensely.Domain.Core;
using FluentAssertions;
using Xunit;

namespace Expensely.Domain.UnitTests
{
    public sealed class ErrorsTests
    {
        [Fact]
        public void Error_codes_must_be_unique()
        {
            List<PropertyInfo> properties = typeof(Errors)
                .GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                .SelectMany(x => x.GetProperties(BindingFlags.Public | BindingFlags.Static))
                .Where(x => x.PropertyType == typeof(Error))
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

            var error = (Error)propertyGetMethod.Invoke(null, parameters)!;

            return error!.Code;
        }
    }
}