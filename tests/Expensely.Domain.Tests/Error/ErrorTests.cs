using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Expensely.Domain.Tests.Error
{
    public sealed class ErrorTests
    {
        [Fact]
        public void Error_codes_must_be_unique()
        {
            List<MethodInfo> methods = typeof(Core.Primitives.Error)
                .GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(x => x.ReturnType == typeof(Core.Primitives.Error))
                .ToList();

            int numberOfUniqueCodes = methods.Select(GetErrorCode)
                .Distinct()
                .Count();

            Assert.Equal(methods.Count, numberOfUniqueCodes);
        }

        private static string GetErrorCode(MethodInfo method)
        {
            object[] parameters = method.GetParameters()
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

            var error = (Core.Primitives.Error)method.Invoke(null, parameters)!;

            return error!.Code;
        }
    }
}
