using System;

namespace DungeonExplorer
{
    /// <summary>
    /// A simplified testing class to verify the game functionality
    /// </summary>
    public class Testing
    {
        /// <summary>
        /// Runs all the tests for the game
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("Running tests...");

            TestPlayer();
            TestRoom();

            Console.WriteLine("All tests passed successfully!");
        }

        /// <summary>
        /// Tests the Player class functionality
        /// </summary>
        private static void TestPlayer()
        {
            Console.WriteLine("Testing Player class...");

            // Create a test player
            Player player = new Player("TestPlayer", 100);

            // Check initial values
            if (player.Name != "TestPlayer")
                ThrowError("Player name not set correctly");

            if (player.Health != 100)
                ThrowError("Player health not set correctly");

            if (player.InventoryContents() != "No items")
                ThrowError("New player should have no items");

            // Test inventory functionality
            player.PickUpItem("Sword");
            if (player.InventoryContents() != "Sword")
                ThrowError("Item not added to inventory");

            // Test damage functionality
            player.TakeDamage(30);
            if (player.Health != 70)
                ThrowError("Damage not applied correctly");

            if (!player.IsAlive())
                ThrowError("Player should still be alive");

            Console.WriteLine("Player tests passed!");
        }

        /// <summary>
        /// Tests the Room class functionality
        /// </summary>
        private static void TestRoom()
        {
            Console.WriteLine("Testing Room class...");

            // Test room with an item
            Room roomWithItem = new Room("Test Room", "Key");

            if (roomWithItem.GetDescription() != "Test Room")
                ThrowError("Room description not set correctly");

            if (!roomWithItem.HasItem())
                ThrowError("Room should have an item");

            if (roomWithItem.GetItemName() != "Key")
                ThrowError("Item name not retrieved correctly");

            // Test taking the item
            string takenItem = roomWithItem.TakeItem();

            if (takenItem != "Key")
                ThrowError("Taken item should be the Key");

            if (roomWithItem.HasItem())
                ThrowError("Room should no longer have an item after taking it");

            // Test empty room
            Room emptyRoom = new Room("Empty Room");

            if (emptyRoom.HasItem())
                ThrowError("Empty room should not have an item");

            Console.WriteLine("Room tests passed!");
        }

        /// <summary>
        /// Helper method to throw an error with a message
        /// </summary>
        private static void ThrowError(string message)
        {
            throw new Exception($"Test failed: {message}");
        }
    }
}