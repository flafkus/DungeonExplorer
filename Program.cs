using System;

namespace DungeonExplorer 
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Dungeon Explorer - OOP Assessment 2");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("1. Start Game");
                Console.WriteLine("2. Exit");
                Console.WriteLine("3. Run Tests");
                Console.WriteLine("--------------------------------");
                Console.Write("Select an option: ");
                
                string option = Console.ReadLine();
                
                switch (option)
                {
                    case "1":
                        // Create and start the game
                        Console.WriteLine("Starting Dungeon Explorer...");
                        Game game = new Game();
                        game.Start();
                        break;
                        
                    case "2":
                        Console.WriteLine("Exiting...");
                        break;

                    case "3":
                        // Run the tests
                        Console.WriteLine("Running tests...");
                        Testing.RunTests();
                        break;  

                    default:
                        Console.WriteLine("Invalid option. Exiting...");
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred: {exception.Message}");
                Console.WriteLine($"Stack trace: {exception.StackTrace}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}