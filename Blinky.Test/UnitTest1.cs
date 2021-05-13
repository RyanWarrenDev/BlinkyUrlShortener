using Blinky.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blinky.Test
{
    public class UnitTest1
    {
        /// <summary>
        /// Given the random nature of key generation I used it should be fairly unique but still likely to 
        /// cause collisions. That's why i've added additional checks in code to ensure if it is a duplicate token
        /// it will generate a new one.
        /// Obvioulsy in a production distributed system the key generation would be counter based with each instance
        /// being assigned a range, likely managed by something like ZooKeeper.
        /// Like i said given the random nature of key generation used this test is luck of the draw really, i've ran it
        /// loads of times and once was able to generate 9Mil tokens with 0 collisons but that's just luck.
        /// </summary>
        /// <param name="tokensToGenerate"># of unique tokens to generate</param>
        [Theory]
        [InlineData(2500000)]
        public void ShouldBeUnique(int tokensToGenerate)
        {
            var tokenGenerator = new ShortTokenGenerator();

            var tokens = new List<string>();

            for(int i = 0; i < tokensToGenerate; i++)
            {
                tokens.Add(tokenGenerator.GenerateShortToken(7));
            }

            tokens.Should().OnlyHaveUniqueItems();
        }
    }
}
