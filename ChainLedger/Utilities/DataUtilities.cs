using ChainLedger.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChainLedger.Utilities
{
    public class DataUtilities
    {
        public static Func<DateTime> GetUtcNow = () => DateTime.UtcNow;
        public static Func<Guid> GetGuid = () => Guid.NewGuid();
    }
}
