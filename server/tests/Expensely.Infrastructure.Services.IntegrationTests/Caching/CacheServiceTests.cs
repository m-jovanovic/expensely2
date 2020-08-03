using Expensely.Application.Abstractions.Caching;
using Expensely.Infrastructure.Services.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Expensely.Infrastructure.Services.IntegrationTests.Caching
{
    public class CacheServiceTests
    {
        private const string CacheKey = "test-cache-key";
        private readonly ICacheService _cacheService;

        public CacheServiceTests()
        {
            _cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public void Should_return_null_if_value_is_not_cached()
        {
            object? result = _cacheService.GetValue<object>(CacheKey);

            result.Should().BeNull();
        }

        [Fact]
        public void Should_return_value_if_value_is_cached()
        {
            var value = new
            {
                Value1 = "Some value",
                Value2 = 100.0m
            };
            _cacheService.SetValue(CacheKey, value, 1);

            object? result = _cacheService.GetValue<object>(CacheKey);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(value);
        }

        [Fact]
        public void Should_remove_cached_value()
        {
            var value = new
            {
                Value1 = "Some value",
                Value2 = 100.0m
            };
            _cacheService.SetValue(CacheKey, value, 1);

            _cacheService.RemoveValue(CacheKey);
            object? result = _cacheService.GetValue<object>(CacheKey);

            result.Should().BeNull();
        }
    }
}
