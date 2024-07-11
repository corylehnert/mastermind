using System;
using System.Linq;

namespace mastermind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            var exit = false;

            while (!exit) // exit loop for when the player wants to stop playing Mastermind
            {
                
                var hasWon = false;
                var gameEnd = false;
                var attempts = 0;
                var answer = "" + rnd.Next(1, 6) + rnd.Next(1, 6) + rnd.Next(1, 6) + rnd.Next(1, 6); // generate a 4 digit number with each digit being a random number between 1 and 6

                while (!gameEnd) // loop for current game
                {
                    Console.Write("Guess the 4 digit number: ");
                    var guess = Console.ReadLine();
                    if (guess.Length != 4) //make sure that the length of the number guessed is 4
                    {
                        Console.WriteLine("Please enter a 4 digit number.");
                        continue;
                    }
                    Console.WriteLine();

                    if (guess == answer) //guess is correct
                    {
                        hasWon = true;
                        gameEnd = true;
                    }
                    else // guess is incorrect
                    {
                        // add to attempt counter 
                        attempts++;

                        // find the sequence of both correct and misplaced digits from the player's guess
                        var correctAnswers = DetermineCorrectGuesses(guess, answer);
                        var correctGuesses = correctAnswers.Item1;
                        var misplacedGuesses = DeterminMisplacedGuesses(guess, answer, correctAnswers.Item2);

                        // display the accuracy of the player's guess and attempts remaining
                        Console.WriteLine("You guessed:                     " + guess);
                        Console.WriteLine("Correct guesses:                 " + correctGuesses);
                        Console.WriteLine("Correct, but in the wrong order: " + misplacedGuesses);
                        Console.WriteLine();
                        Console.WriteLine("Incorrect! Try Again! " + (10 - attempts) + " remaining.");

                        // after attempt 10, the game is over
                        if (attempts == 10)
                        {
                            gameEnd = true;
                        }

                    }
                }

                // determine if the player has won or lost
                if (hasWon)
                {
                    Console.WriteLine("You Won! You found the answer in " + attempts + " attempts!");
                }
                else
                {
                    Console.WriteLine("You Lose! The correct answer was " + answer);
                }

                // determine if the player wants to play another round
                exit = DetermineNextRound();
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        /// <summary>
        /// Determine whether or not the all instances of a digit were found in the player's guess either correctly or misplaced
        /// </summary>
        /// <param name="correctSequence">String of correct digits that were already found by the player</param>
        /// <param name="misplacedSequence">String of misplaced digits that were found by the player</param>
        /// <param name="answer">Correct answer to the puzzle</param>
        /// <param name="digit">Current digit being evaluated from the player's guess</param>
        /// <returns>true, if there is more instances of a digit that haven't been found; false if all instances of digit were found</returns>
        private static bool AreAllnstancesOfDigitFound(string correctSequence, string misplacedSequence, string answer, char digit)
        {
            var countOfDigit = answer.Count(x => x == digit);
            var countOfCorrectDigitsFound = correctSequence.Count(x => x == digit);
            var countOfMisplacedDigitsFound = misplacedSequence.Count(x => x == digit);

            return countOfDigit >= 1 && (countOfCorrectDigitsFound + countOfMisplacedDigitsFound) < countOfDigit;

        }

        /// <summary>
        /// Prompt player to see if they want to play another game of Mastermind
        /// Repeating the prompt if they don't select either 'Y' or 'N' options
        /// </summary>
        /// <returns>true if they enter 'Y'; false if they enter 'N'</returns>
        private static bool DetermineNextRound()
        {
            while (true)
            {
                Console.Write("Would you like to play again(Y/N): ");
                var playAgain = Console.ReadLine();
                if (playAgain.ToUpper() == "N")
                {
                    return true;
                }
                else if (playAgain.ToUpper() == "Y")
                {
                    Console.Clear();
                    return false;
                }
                Console.WriteLine("Please either Y or N");
            }
        }

        /// <summary>
        /// Determine which digits are in the correct position in the guess from the player.
        /// </summary>
        /// <param name="guess">Guess provided by the player</param>
        /// <param name="answer">Correct answer to the puzzle</param>
        /// <returns>Tuple containing the '+' string representation of the correct sequence and a string with only the correct digits guessed shown</returns>
        private static (string, string) DetermineCorrectGuesses(string guess, string answer)
        {
            var correctGuesses = "";
            var correctSequence = "";
            for(int i = 0; i < answer.Length; i++)
            {
                if (guess[i] == answer[i])
                {
                    correctGuesses += "+";
                    correctSequence += guess[i];
                }
                else
                {
                    correctGuesses += " ";
                    correctSequence += " ";
                }
            }
            return (correctGuesses, correctSequence);
        }

        /// <summary>
        /// Determine which digits are in the guess from the player, but in the wrong position and not already found.
        /// </summary>
        /// <param name="guess">Guess from the player</param>
        /// <param name="answer">Correct answer to the puzzle</param>
        /// <param name="correctSequence">Current sequence of correct digits currently found by the player</param>
        /// <returns></returns>
        private static string DeterminMisplacedGuesses(string guess, string answer, string correctSequence)
        {
            var misplacedGuesses = "";
            var misplacedSequence = ""; 
            for (int i = 0; i < answer.Length; i++)
            {
                if(correctSequence[i] != ' ') // the digit is correct and in position
                {
                    misplacedGuesses += " ";
                    misplacedSequence += " ";
                    continue;
                }
                else if (guess[i] != answer[i] && answer.Contains(guess[i]) && AreAllnstancesOfDigitFound(correctSequence, misplacedSequence, answer, guess[i]))
                {
                    misplacedGuesses += "-";
                    misplacedSequence += guess[i];
                }
                else
                {
                    misplacedGuesses += " ";
                    misplacedSequence += " ";
                }
            }
            return misplacedGuesses;
        }
    }
}
