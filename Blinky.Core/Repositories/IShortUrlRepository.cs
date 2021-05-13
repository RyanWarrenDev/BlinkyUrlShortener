using Blinky.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Core.Repositories
{
    public interface IShortUrlRepository
    {
        IEnumerable<ShortUrl> GetAllShortUrls();

        ShortUrl GetShortUrlByToken(string token);

        ShortUrl InsertShortUrl(ShortUrl shortUrl);

        void UpdateShortUrl(ShortUrl shortUrl);

        bool DeleteShortUrl(ShortUrl shortUrl);
    }
}
