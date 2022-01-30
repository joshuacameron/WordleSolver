using System.Collections.Generic;
using System.Linq;

namespace WordleSolver
{
    public class WordleBoardState
    {
        public List<(char Letter, int Position)> KnownContainingLettersKnownNotPositions { get; set; }
        public List<(char Letter, int Position)> KnownContainingLettersKnownPosition { get; set; }
        public List<char> KnownNotContainingLetters { get; set; }

        public WordleBoardState()
        {
            KnownContainingLettersKnownNotPositions = new List<(char Letter, int Position)>();
            KnownContainingLettersKnownPosition = new List<(char Letter, int Position)>();
            KnownNotContainingLetters = new List<char>();
        }

        public List<char> UnknownLetters
        {
            get
            {
                return Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x)
                    .Where(x => !KnownContainingLettersKnownNotPositions.Where(y => y.Letter == x).Any())
                    .Where(x => !KnownContainingLettersKnownPosition.Where(y => y.Letter == x).Any())
                    .Where(x => !KnownNotContainingLetters.Contains(x))
                    .ToList();
            }
        }
    }
}
