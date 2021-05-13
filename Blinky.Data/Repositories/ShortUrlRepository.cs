using Blinky.Core.Entities;
using Blinky.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Data.Repositories
{
    public class ShortUrlRepository : BaseRepository<ShortUrl>, IShortUrlRepository
    {
        public ShortUrlRepository(BlinkyContext context) : base(context)
        { }

        public IEnumerable<ShortUrl> GetAllShortUrls()
        {
            return All();
        }

        public bool DeleteShortUrl(ShortUrl shortUrl)
        {
            return Delete(shortUrl.Id);
        }

        public ShortUrl GetShortUrlByToken(string token)
        {
            return Collection.Find(w => w.ShortToken == token)?.FirstOrDefault();
        }

        public ShortUrl InsertShortUrl(ShortUrl shortUrl)
        {
            shortUrl.Clicks = 0;
            return Insert(shortUrl);
        }

        public void UpdateShortUrl(ShortUrl shortUrl)
        {
            Update(shortUrl);
        }
    }
}
