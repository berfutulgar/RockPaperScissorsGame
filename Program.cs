using System;
using System.IO;

namespace RockPaperScissorsGame
{
    class Program
    {
        static string scoreFilePath = "scores.txt"; // Dosya yolu

        static void Main(string[] args)
        {
            int previousPlayerWins = 0;
            int previousComputerWins = 0;
            int previousDraws = 0;

            int currentPlayerWins = 0;
            int currentComputerWins = 0;
            int currentDraws = 0;

            LoadScores(ref previousPlayerWins, ref previousComputerWins, ref previousDraws);

            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Rock-Paper-Scissors Game!");

                Console.WriteLine($"\nPrevious Score: You: {previousPlayerWins}, Computer: {previousComputerWins}, Draws: {previousDraws}");
                Console.WriteLine($"Current Session Score: You: {currentPlayerWins}, Computer: {currentComputerWins}, Draws: {currentDraws}");

                Console.WriteLine("\nPlease choose: (R)ock, (P)aper, (S)cissors");

                string? playerChoice = GetPlayerChoice();
                string computerChoice = GetComputerChoice();

                if (playerChoice != null)
                {
                    Console.WriteLine($"\nYou chose: {playerChoice}");
                    Console.WriteLine($"Computer chose: {computerChoice}");

                    string result = DetermineWinner(playerChoice, computerChoice);

                    switch (result)
                    {
                        case "Player":
                            currentPlayerWins++;
                            break;
                        case "Computer":
                            currentComputerWins++;
                            break;
                        case "Draw":
                            currentDraws++;
                            break;
                    }

                    Console.WriteLine($"\nUpdated Current Session Score: You: {currentPlayerWins}, Computer: {currentComputerWins}, Draws: {currentDraws}");

                    SaveScores(previousPlayerWins + currentPlayerWins, previousComputerWins + currentComputerWins, previousDraws + currentDraws);
                }

                playAgain = AskToPlayAgain();
            }

            Console.WriteLine($"\nFinal Score for This Session: You: {currentPlayerWins}, Computer: {currentComputerWins}, Draws: {currentDraws}");
            Console.WriteLine("Thanks for playing!");
        }

        static string? GetPlayerChoice()
        {
            string? choice = null;
            while (true)
            {
                choice = Console.ReadLine()?.ToUpper();
                if (choice == "R" || choice == "P" || choice == "S")
                    break;
                else
                    Console.WriteLine("Invalid input! Please choose (R)ock, (P)aper, or (S)cissors:");
            }
            return choice switch
            {
                "R" => "Rock",
                "P" => "Paper",
                "S" => "Scissors",
                _ => null
            };
        }

        static string GetComputerChoice()
        {
            Random random = new Random();
            int choice = random.Next(1, 4);
            return choice switch
            {
                1 => "Rock",
                2 => "Paper",
                3 => "Scissors",
                _ => throw new InvalidOperationException("Invalid random choice")
            };
        }

        static string DetermineWinner(string playerChoice, string computerChoice)
        {
            if (playerChoice == computerChoice)
            {
                Console.WriteLine("It's a draw!");
                return "Draw";
            }
            else if ((playerChoice == "Rock" && computerChoice == "Scissors") ||
                     (playerChoice == "Paper" && computerChoice == "Rock") ||
                     (playerChoice == "Scissors" && computerChoice == "Paper"))
            {
                Console.WriteLine("You win!");
                return "Player";
            }
            else
            {
                Console.WriteLine("Computer wins!");
                return "Computer";
            }
        }

        static bool AskToPlayAgain()
        {
            Console.WriteLine("\nDo you want to play again? (Y/N)");
            string? answer = Console.ReadLine()?.ToUpper();
            return answer == "Y";
        }

        static void SaveScores(int playerWins, int computerWins, int draws)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(scoreFilePath))
                {
                    writer.WriteLine(playerWins);
                    writer.WriteLine(computerWins);
                    writer.WriteLine(draws);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while saving scores: {ex.Message}");
            }
        }

        static void LoadScores(ref int playerWins, ref int computerWins, ref int draws)
        {
            if (File.Exists(scoreFilePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(scoreFilePath);
                    if (lines.Length >= 3)
                    {
                        playerWins = int.Parse(lines[0]);
                        computerWins = int.Parse(lines[1]);
                        draws = int.Parse(lines[2]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while loading scores: {ex.Message}");
                }
            }
        }
    }
}
