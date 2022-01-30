using System.Collections.Generic;

namespace WordleSolver.Interfaces
{
    public interface IWordList
    {
        IEnumerable<string> Words { get; set; }
        int WordCount { get; }

        IWordList GetWordsThatAreGivenLength(int length);
        IWordList GetWordsContainingLettersButNotInPositions(IEnumerable<(char Letter, int Position)> lettersWithKnownNotPositions);
        IWordList GetWordsContainingLettersInPositions(IEnumerable<(char Letter, int Position)> lettersWithKnownPositions);
        IWordList GetWordsNotContainingLetters(IEnumerable<char> letters);

        int GetWordCountContainingLetter(char letter);
    }
}
