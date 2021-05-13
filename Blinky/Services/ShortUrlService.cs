using Blinky.Core.Entities;
using Blinky.Core.Repositories;
using Blinky.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Blinky.Services
{
    public class ShortUrlService : IShortUrlService, IDisposable
    {
        private const int DEFAULT_TOKEN_LENGTH = 8;

        private readonly IHttpContextAccessor HttpContext;

        private readonly IShortUrlRepository ShortUrlRepository;

        private readonly IShortTokenGenerator ShortTokenGenerator;

        private readonly int tokenLength;

        private readonly IMemoryCache _cache;

        public ShortUrlService(IShortUrlRepository shortUrlRepository, IShortTokenGenerator tokenGenerator, IConfiguration configuration, 
            IHttpContextAccessor httpContext, IMemoryCache cache)
        {
            HttpContext = httpContext;
            _cache = cache;

            ShortUrlRepository = shortUrlRepository;
            ShortTokenGenerator = tokenGenerator;

            var configTokenLength = configuration.GetValue<int>("TokenGeneration:TokenLength");
            tokenLength = configTokenLength != default ? configTokenLength : DEFAULT_TOKEN_LENGTH;
        }

        #region Interface Implmentation
        public IEnumerable<ShortUrl> GetAllShortUrls()
        {
            return ShortUrlRepository.GetAllShortUrls();
        }

        public string GenerateShortUrl(string originalUrl)
        {
            if (string.IsNullOrEmpty(originalUrl) || !Uri.IsWellFormedUriString(originalUrl, UriKind.Absolute))
                return "Missing or incorrectly formed URL, please correct and try again";

            var shortToken = GetShortToken();

            var shortenedUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortToken = shortToken
            };

            ShortUrlRepository.InsertShortUrl(shortenedUrl);

            var shortUrl = $"https://{HttpContext.HttpContext.Request.Host}/{shortToken}" ?? "Unable to generate short url";

            return shortUrl;
        }

        public string GetOriginalUrl(string token)
        {
            var originalUrl = _cache.Get<string>(token);

            if(originalUrl == null)
            {
                var shortUrl = ShortUrlRepository.GetShortUrlByToken(token);
                _cache.Set(token, shortUrl.OriginalUrl);

                originalUrl = shortUrl.OriginalUrl;
            }
            
            return originalUrl;
        }

        public void IncrementClick(string token)
        {
            var shortUrl = ShortUrlRepository.GetShortUrlByToken(token);
            shortUrl.Clicks += 1;

            ShortUrlRepository.UpdateShortUrl(shortUrl);
        }

        public void Dispose()
        {
            ShortTokenGenerator.Dispose();
        }
        #endregion Interface Implementation

        #region Token Generation
        private string GetShortToken()
        {
            var shortToken = ShortTokenGenerator.GenerateShortToken(tokenLength);

            while (TokenExists(shortToken))
            {
                shortToken = ShortTokenGenerator.GenerateShortToken(tokenLength);
            }

            return shortToken;
        }

        private bool TokenExists(string token) => ShortUrlRepository.GetShortUrlByToken(token) != null;
        #endregion Token Generation
    }
}
