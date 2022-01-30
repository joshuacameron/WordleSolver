using System.Collections.Generic;
using System.Linq;
using WordleSolver.Interfaces;

namespace WordleSolver
{
    public class WordleSolver : IWordleSolver
    {
        private const int WordLength = 5;

        private readonly IWordList _wordList;

        public WordleSolver(IWordList wordList)
        {
            _wordList = wordList.GetWordsThatAreGivenLength(WordLength);
        }

        public IEnumerable<string> GetNextWords(WordleBoardState wordleBoardState)
        {
            //'green' - Filter the word list to just contain words which have letters in known places
            var wordsContainingLettersInPositions = _wordList.GetWordsContainingLettersInPositions(wordleBoardState.KnownContainingLettersKnownPosition);

            //'yellow' - Filter THAT word list so it just contains words which contain letters that must exist in the words, but not in given locations
            var wordsContainingLettersButNotInPositions = wordsContainingLettersInPositions.GetWordsContainingLettersButNotInPositions(wordleBoardState.KnownContainingLettersKnownNotPositions);

            //'grey' - Filter THAT word list to not contain words that have any of the letters that don't exist
            var wordsNotContainingLetters = wordsContainingLettersButNotInPositions.GetWordsNotContainingLetters(wordleBoardState.KnownNotContainingLetters);

            /*
             * Now there will still be many possibles words to try next, potentiallys 100s or 1000s
             * The following steps figure out which word will give the most information for future guesses
             * 
             * It does this by counting the number of times each letter that hasn't been seen occurs in the left over word list
             * Once it knows this, it's possible to weigh each word by how many possible words it's letters will rule out
            */

            var letterCounts = GetLetterCounts(wordsNotContainingLetters, wordleBoardState.UnknownLetters);

            var wordScores = GetWordScores(wordsNotContainingLetters, letterCounts);

            var wordScoresOrdered = wordScores.OrderByDescending(x => x.Value);

            return wordScoresOrdered.Select(x => x.Key);
        }

        private static Dictionary<char, int> GetLetterCounts(IWordList wordList, List<char> letters)
        {
            return letters.ToDictionary(x => x, x => wordList.GetWordCountContainingLetter(x));
        }

        private static Dictionary<string, int> GetWordScores(IWordList wordList, Dictionary<char, int> letterCounts)
        {
            return wordList.Words.ToDictionary(word => word, word => word.Distinct().Sum(letter =>
            {
                letterCounts.TryGetValue(letter, out int letterCount);
                return letterCount;
            }));
        }
    }
}
