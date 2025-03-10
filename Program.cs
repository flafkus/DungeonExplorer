using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Entry class for the Dungeon Explorer game
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates a new game and starts it
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                // Create and start the game
                Console.WriteLine("Starting Dungeon Explorer...");
                Game game = new Game();
                game.Start();
            }
            catch (Exception ex)
            {
                // Basic error handling
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}