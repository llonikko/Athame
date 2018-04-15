using System;
using System.Linq;

namespace Athame.Core.Utils
{
    public class RandomWords
    {
        private readonly Random rnd = new Random();

        // https://randomwordgenerator.com/
        private readonly string[] nouns = {
            "speech",
            "department",
            "mood",
            "road",
            "internet",
            "development",
            "throat",
            "steak",
            "emotion",
            "month"
        };

        private readonly string[] verbs = {
            "judge",
            "resist",
            "undertake",
            "point",
            "embody",
            "damage",
            "succeed",
            "enjoy",
            "establish",
            "reassure"
        };

        private readonly string[] adjectives =
        {
            "basic",
            "successful",
            "existing",
            "environmental",
            "political",
            "rare",
            "conscious",
            "federal",
            "ugly",
            "emotional"
        };

        private readonly string[] articles = {"a", "the", "my"};

        private readonly string[] firstNames =
        {
            "Felix",
            "Rusty",
            "BatMan",
            "Luna",
            "George",
            "Midnight",
            "Kiki",
            "Rocky",
            "Molly",
            "Smokey"
        };

        private readonly string[] lastNames = {
            "Schultz",
            "Cuevas",
            "Rhodes",
            "Taylor",
            "Glenn",
            "Reilly",
            "Morris",
            "Beltran",
            "Swanson",
            "Roth"
        };

        private bool CoinFlip()
        {
            return rnd.Next(2) != 0;
        }

        private string RandomCapitalisedWord(string[] set)
        {
            var index = rnd.Next(0, set.Length);
            return set[index].First().ToString().ToUpper() + set[index].Substring(1);
        }

        public string NewArticleAdjectiveNounTitle()
        {
            var article = RandomCapitalisedWord(articles);
            var adjective = RandomCapitalisedWord(adjectives);
            var noun = RandomCapitalisedWord(nouns);
            return article + " " + adjective + " " + noun;
        }

        public string NewVerbNounTitle()
        {
            var verb = RandomCapitalisedWord(verbs);
            var noun = RandomCapitalisedWord(nouns);
            return verb + " " + noun;
        }

        public string NewFullName()
        {
            var first = RandomCapitalisedWord(firstNames);
            var last = RandomCapitalisedWord(lastNames);
            return first + " " + last;
        }

        public string NewArtistName()
        {
            return CoinFlip() ? NewFullName() : NewArticleAdjectiveNounTitle();
        }

        public string NewTitle()
        {
            return CoinFlip() ? NewVerbNounTitle() : RandomCapitalisedWord(nouns);
        }

    }
}
