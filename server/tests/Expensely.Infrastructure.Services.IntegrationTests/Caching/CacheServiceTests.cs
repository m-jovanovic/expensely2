using Expensely.Application.Core.Abstractions.Caching;
using Expensely.Infrastructure.Services.Caching;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Expensely.Infrastructure.Services.IntegrationTests.Caching
{
    public class CacheServiceTests
    {
        private const string CacheKeyPrefix = "test-cache-key";
        private const string CacheKey1 = "test-cache-key-1";
        private const string CacheKey2 = "test-cache-key-2";

        private readonly ICacheService _cacheService;

        public CacheServiceTests()
        {
            _cacheService = new CacheService(new MemoryCache(new MemoryCacheOptions()));
        }

        [Fact]
        public void Should_return_null_if_value_is_not_cached()
        {
            object? result = _cacheService.GetValue<object>(CacheKey1);

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
            _cacheService.SetValue(CacheKey1, value, 1);

            object? result = _cacheService.GetValue<object>(CacheKey1);

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
            _cacheService.SetValue(CacheKey1, value, 1);

            _cacheService.RemoveValue(CacheKey1);

            _cacheService.GetValue<object>(CacheKey1).Should().BeNull();
        }

        [Fact]
        public void Should_remove_remove_cached_values_by_pattern()
        {
            var value = new
            {
                Value1 = "Some value",
                Value2 = 100.0m
            };
            _cacheService.SetValue(CacheKey1, value, 1);
            _cacheService.SetValue(CacheKey2, value, 1);

            _cacheService.RemoveByPattern(CacheKeyPrefix);

            _cacheService.GetValue<object>(CacheKey1).Should().BeNull();
            _cacheService.GetValue<object>(CacheKey2).Should().BeNull();
        }
    }
}
