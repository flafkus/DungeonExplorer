using System;

namespace DungeonExplorer 
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Create and start the game
                // Testing.RunTests();

                Console.WriteLine("Starting Dungeon Explorer...");
                Game game = new Game();
                game.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
