using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Shared.Services
{

    public class RandomNumberGeneratorService : IRandomNumberGeneratorInterface
    {

        private static readonly Random Random = new Random();

        public string GenerateRandomNumber(int length, Mode mode = Mode.AlphaNumeric)
        {
            var characters = new List<char>();

            if (mode == Mode.Numeric || mode == Mode.AlphaNumeric || mode == Mode.AlphaNumericUpper || mode == Mode.AlphaNumericLower)
                for (var c = '0'; c <= '9'; c++)
                    characters.Add(c);

            if (mode == Mode.AlphaNumeric || mode == Mode.AlphaNumericUpper || mode == Mode.AlphaUpper)
                for (var c = 'A'; c <= 'Z'; c++)
                    characters.Add(c);

            if (mode == Mode.AlphaNumeric || mode == Mode.AlphaNumericLower || mode == Mode.AlphaLower)
                for (var c = 'a'; c <= 'z'; c++)
                    characters.Add(c);

            return new string(Enumerable.Repeat(characters, length)
              .Select(s => s[Random.Next(s.Count)]).ToArray());
        }
    }



    public enum Mode
    {
        AlphaNumeric = 1,
        AlphaNumericUpper = 2,
        AlphaNumericLower = 3,
        AlphaUpper = 4,
        AlphaLower = 5,
        Numeric = 6
    }
}
