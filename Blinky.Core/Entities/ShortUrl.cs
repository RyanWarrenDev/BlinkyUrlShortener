using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Core.Entities
{
    public class ShortUrl : BaseEntity
    {
        public string OriginalUrl { get; set; }

        public string ShortToken { get; set; }

        public int Clicks { get; set; }
    }
}
