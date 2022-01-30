# Wordle Solver

![Solved Wardle image](https://github.com/joshuacameron/WordleSolver/blob/main/Images/wordleCracker.png?raw=true)

[Wordle](https://www.powerlanguage.co.uk/wordle/) is a free online game that gives a new daily word puzzle every day. This is a project to calculate the best next word to try on Wordle games given the board's state. To calculate the best next word the Solver filters the possible word list down to only the words that are possible as solutions, and then it figures out which word helps rule out the most possible solutions in what is left. The heart of this Solver walking through the simple algorithm is fully commented [here](https://github.com/joshuacameron/WordleSolver/blob/main/WordleSolver/WordleSolver.cs#L18).

## How to use WordleSolver
```C#
//Load the word list from the text file
var wordList = new WordList("./Resources/words_alpha.txt");

//Create a solver with that word list
var wordleSolver = new WordleSolver(wordList);

//Setup the board's state
var wordleBoardState = new WordleBoardState();

//List the letters which are 'green' on the board, and which position they're known to be in
wordleBoardState.KnownContainingLettersKnownPosition.Add(('u', 2));

//List the letters which are 'yellow' on the board, and which position they're known to NOT be in
wordleBoardState.KnownContainingLettersKnownNotPositions.Add(('r', 0));

//List the letters which were 'grey' on the board and not used in the target word
wordleBoardState.KnownNotContainingLetters.Add('a');
wordleBoardState.KnownNotContainingLetters.Add('i');
wordleBoardState.KnownNotContainingLetters.Add('s');
wordleBoardState.KnownNotContainingLetters.Add('e');

//Run the method to get the ordered list of next words to try
var nextWords = wordleSolver.GetNextWords(wordleBoardState);
var nextGuess = nextWords.First();
```

## Testing
- To ensure bugs aren't introduced which break the code a test has been added which ensures the Solver can solve each of the 16,000 possible 5 letter words

## Future work
- Wordle has it's own word list built in, extract it and use that instead of [dwyl/english-words](https://github.com/dwyl/english-words)
- Performance analyse / optimise the code where possible given architecture

## Credits:
- Word list with only alpha chracters [dwyl/english-words](https://github.com/dwyl/english-words)
