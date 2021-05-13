using Blinky.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Core.Services
{
    public interface IShortUrlService
    {
        IEnumerable<ShortUrl> GetAllShortUrls();

        string GenerateShortUrl(string originalUrl);

        string GetOriginalUrl(string token);

        void IncrementClick(string token);
    }
}
