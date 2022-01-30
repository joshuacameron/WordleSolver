using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WordleSolver;
using WordleSolver.Interfaces;

namespace WordleSolverTests
{
    [Parallelizable(ParallelScope.All)]
    public class Tests
    {
        private const string WordListFilename = "./Resources/words_alpha.txt";
        private const int WordLength = 5;
        private const char NonAlphaChar = '-';

        private static readonly IWordList WordList = new WordList(WordListFilename).GetWordsThatAreGivenLength(WordLength);
        private static readonly IEnumerable<string> Words = WordList.Words;

        [Test]
        [TestCaseSource(nameof(Words))]
        public void TestCanSolveAllPossibleWords(string word)
        {
            if (!CanSolve(word))
            {
                Assert.Fail("Failed to solve word " + word);
            }
        }

        private static bool CanSolve(string targetWord)
        {
            var wordleSolver = new WordleSolver.WordleSolver(WordList);

            var wordleBoardState = new WordleBoardState();

            while (!IsWordleBoardSolved(wordleBoardState))
            {
                var nextWords = wordleSolver.GetNextWords(wordleBoardState);

                if(!nextWords.Any())
                {
                    return false;
                }

                var selectedWord = nextWords.First();

                ProcessGuess(wordleBoardState, targetWord, selectedWord);
            }

            return true;
        }

        private static void ProcessGuess(WordleBoardState currentState, string targetWord, string newGuess)
        {
            var newGuessExcludingMatches = newGuess;

            //Has the letter, and in correct position
            for (int i = 0; i < newGuess.Length; i++)
            {
                if(newGuess[i] == targetWord[i])
                {
                    if(!currentState.KnownContainingLettersKnownPosition.Any(x => x.Position == i))
                    {
                        currentState.KnownContainingLettersKnownPosition.Add((newGuess[i], i));
                    }

                    newGuessExcludingMatches = newGuessExcludingMatches.ReplaceAt(i, NonAlphaChar);
                }
            }

            for(int i = 0; i < newGuessExcludingMatches.Length; i++)
            {
                var currentLetter = newGuessExcludingMatches[i];
                if(currentLetter == NonAlphaChar)
                {
                    continue;
                }

                //Has the letter, and not in that position
                if (targetWord.Contains(currentLetter))
                {
                    if (!currentState.KnownContainingLettersKnownNotPositions.Any(x => x.Letter == currentLetter && x.Position == i))
                    {
                        currentState.KnownContainingLettersKnownNotPositions.Add((currentLetter, i));
                    }
                }
                //Does not have letter
                else
                {
                    if(!currentState.KnownNotContainingLetters.Contains(currentLetter))
                    {
                        currentState.KnownNotContainingLetters.Add(currentLetter);
                    }
                }
            }
        }

        private static bool IsWordleBoardSolved(WordleBoardState wordleBoardState)
        {
            return wordleBoardState.KnownContainingLettersKnownPosition.Count == WordLength;
        }
    }
}