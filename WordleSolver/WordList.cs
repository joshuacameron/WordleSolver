using System.Collections.Generic;
using System.IO;
using System.Linq;
using WordleSolver.Interfaces;

namespace WordleSolver
{
    public class WordList : IWordList
    {
        public IEnumerable<string> Words { get; set; }

        public int WordCount => Words.Count();

        public WordList(string wordlistFilename)
        {
            Words = File.ReadAllLines(wordlistFilename);
        }

        public WordList(IEnumerable<string> words)
        {
            Words = words;
        }

        private WordList() { }

        public IWordList GetWordsThatAreGivenLength(int length)
        {
            return new WordList() { Words = Words.Where(x => x.Length == length) };
        }

        public IWordList GetWordsContainingLettersButNotInPositions(IEnumerable<(char Letter, int Position)> lettersWithKnownNotPositions)
        {
            if (!lettersWithKnownNotPositions.Any())
            {
                return new WordList() { Words = Words };
            }

            var letterList = lettersWithKnownNotPositions.Select(x => x.Letter);

            return new WordList()
            {
                Words = Words
                    .Where(x => x.Intersect(letterList).Any())
                    .Where(x => !DoesWordHaveAnyLettersInPositions(x, lettersWithKnownNotPositions))
            };
        }

        public IWordList GetWordsContainingLettersInPositions(IEnumerable<(char Letter, int Position)> lettersWithKnownPositions)
        {
            if (!lettersWithKnownPositions.Any())
            {
                return new WordList() { Words = Words };
            }

            return new WordList() { Words = Words.Where(x => DoesWordHaveAnyLettersInPositions(x, lettersWithKnownPositions)) };
        }

        public IWordList GetWordsNotContainingLetters(IEnumerable<char> letters)
        {
            if (!letters.Any())
            {
                return new WordList() { Words = Words };
            }

            return new WordList() { Words = Words.Where(x => !x.Intersect(letters).Any()) };
        }

        public int GetWordCountContainingLetter(char letter)
        {
            return Words.Count(x => x.Contains(letter));
        }

        private static bool DoesWordHaveAnyLettersInPositions(string word, IEnumerable<(char Letter, int Position)> letters)
        {
            return letters.Any(x => word[x.Position] == x.Letter);
        }
    }
}
