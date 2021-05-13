using Blinky.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Blinky.Services
{
    public class ShortTokenGenerator : IShortTokenGenerator
    {
        private RNGCryptoServiceProvider cRng;

        private readonly char[] Base64Characters = new[] {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n' , 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_'
        };

        public ShortTokenGenerator()
        {
            cRng = new RNGCryptoServiceProvider();
        }

        public string GenerateShortToken(int tokenLength)
        {
            var shortToken = string.Empty;

            while(shortToken.Length != tokenLength)
            {
                shortToken += GetSingleShortTokenCharacter();
            }

            return shortToken;
        }

        public void Dispose()
        {
            cRng.Dispose();
        }

        #region Token Generation
        private char GetSingleShortTokenCharacter()
        {
            //Get value in Base64 range
            var randomInt = Next(0, 64);

            var base64 = Base64Characters[randomInt];

            return base64;
        }

        /// <summary>
        /// I could lie and say I'm a genius that done this myself but it's actually derived from the below article.
        /// It essentially just brings the Range functionality from Random to the cryptographic random number generator.
        /// https://docs.microsoft.com/en-us/archive/msdn-magazine/2007/september/net-matters-tales-from-the-cryptorandom
        /// </summary>
        /// <param name="minValue">Min acceptable value</param>
        /// <param name="maxExclusiveValue">Max acceptable value + 1</param>
        /// <returns></returns>
        public int Next(int minValue, int maxExclusiveValue)
        {
            if (minValue == maxExclusiveValue)
                return minValue;

            if (minValue > maxExclusiveValue)
            {
                throw new ArgumentOutOfRangeException($"{nameof(minValue)} must be lower than {nameof(maxExclusiveValue)}");
            }

            var diff = (long)maxExclusiveValue - minValue;
            var upperBound = uint.MaxValue / diff * diff;

            uint ui;
            do
            {
                ui = GetRandomUInt();
            }
            while (ui >= upperBound);

            return (int)(minValue + (ui % diff));
        }

        private uint GetRandomUInt()
        {
            var randomBytes = GenerateRandomBytes(sizeof(uint));

            return BitConverter.ToUInt32(randomBytes, 0);
        }

        private byte[] GenerateRandomBytes(int bytesNumber)
        {
            var buffer = new byte[bytesNumber];
            cRng.GetBytes(buffer);

            return buffer;
        }
        #endregion Token Generation
    }
}
