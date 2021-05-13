using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Data
{
    public class BlinkyContext
    {
        public readonly LiteDatabase Context;

        public BlinkyContext(IOptions<LiteDbConfig> options)
        {
            try
            {
                var context = new LiteDatabase(options.Value.Path);
                if (context != null)
                    Context = context;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating or accessing database!", ex);
            }
        }
    }
}
