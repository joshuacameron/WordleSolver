using System.Collections.Generic;

namespace WordleSolver.Interfaces
{
    public interface IWordleSolver
    {
        IEnumerable<string> GetNextWords(WordleBoardState wordleBoardState);
    }
}
