using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace DungeonExplorer
{
    /// <summary>
    /// A comprehensive testing class to verify the game functionality
    /// </summary>
    public class Testing
    {
        private static StreamWriter _logFile;
        private static string _logFilePath;
        
        /// <summary>
        /// Generates a timestamped log filename in the format test_YYYY-MM-DD_HH-MM-SS.txt
        /// </summary>
        private static string GenerateLogFileName()
        {
            DateTime now = DateTime.Now;
            return $"test_{now:yyyy-MM-dd}_{now:HH-mm-ss}.txt";
        }
        
        /// <summary>
        /// Initializes the test log file
        /// </summary>
        private static void InitializeLogFile()
        {
            try
            {
                _logFilePath = GenerateLogFileName();
                _logFile = new StreamWriter(_logFilePath, false);
                _logFile.WriteLine($"DungeonExplorer Test Log - {DateTime.Now}");
                _logFile.WriteLine("======================================");
                _logFile.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing log file: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Logs a message to both console and log file
        /// </summary>
        private static void Log(string message)
        {
            Console.WriteLine(message);
            
            try
            {
                if (_logFile != null)
                {
                    _logFile.WriteLine(message);
                    _logFile.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Closes the log file
        /// </summary>
        private static void CloseLogFile()
        {
            try
            {
                if (_logFile != null)
                {
                    _logFile.WriteLine("\nAll tests completed at " + DateTime.Now);
                    _logFile.Close();
                    Console.WriteLine($"Test log written to {Path.GetFullPath(_logFilePath)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing log file: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Runs all the tests for the game
        /// </summary>
        public static void RunTests()
        {
            InitializeLogFile();
            Log("Running tests...");

            TestPlayer();
            TestRoom();
            TestItem();
            TestInventory();
            TestMonster();
            TestGameMap();
            TestCreatureHierarchy();
            TestPolymorphism();
            TestStatistics();

            Log("All tests passed successfully!");
            CloseLogFile();
        }

        /// <summary>
        /// Tests the Player class functionality
        /// </summary>
        private static void TestPlayer()
        {
            Log("Testing Player class...");

            // Create a test player
            Player player = new Player("TestPlayer", 100);

            // Check initial values
            Debug.Assert(player.Name == "TestPlayer", "Player name not set correctly");
            Debug.Assert(player.Health == 100, "Player health not set correctly");
            Debug.Assert(player.InventoryContents() == "No items", "New player should have no items");

            // Test inventory functionality
            Item sword = new Weapon("Sword", "A sharp sword", 10);
            player.PickUpItem(sword);
            Debug.Assert(player.InventoryContents().Contains("Sword"), "Item not added to inventory");

            // Test damage functionality
            player.TakeDamage(30);
            Debug.Assert(player.Health == 70, "Damage not applied correctly");
            Debug.Assert(player.IsAlive(), "Player should still be alive");

            // Test using a potion
            Item potion = new Potion("Health Potion", "Restores health", 20);
            player.PickUpItem(potion);
            player.UseItem("Health Potion");
            Debug.Assert(player.Health == 90, "Potion not used correctly");

            Log("Player tests passed!");
        }

        /// <summary>
        /// Tests the Room class functionality
        /// </summary>
        private static void TestRoom()
        {
            Log("Testing Room class...");

            // Test room with an item
            Room roomWithItem = new Room("Test Room");
            Item key = new Key("Golden Key", "A shiny key", "treasure");
            roomWithItem.AddItem(key);

            Debug.Assert(roomWithItem.GetDescription() == "Test Room", "Room description not set correctly");
            Debug.Assert(roomWithItem.HasItems(), "Room should have an item");
            Debug.Assert(roomWithItem.GetItems().Count == 1, "Room should have exactly one item");

            // Test taking the item
            Item takenItem = roomWithItem.TakeItem("Golden Key");
            Debug.Assert(takenItem.Name == "Golden Key", "Taken item should be the Key");
            Debug.Assert(!roomWithItem.HasItems(), "Room should no longer have an item after taking it");

            // Test adding a monster
            Monster goblin = new Goblin("Test Goblin");
            roomWithItem.AddMonster(goblin);
            Debug.Assert(roomWithItem.HasMonsters(), "Room should have a monster");
            Debug.Assert(roomWithItem.GetMonsters().Count == 1, "Room should have exactly one monster");

            // Test locked room
            Room lockedRoom = new Room("Locked Room", true, "gold");
            Debug.Assert(lockedRoom.IsLocked(), "Room should be locked");

            // Test unlocking room
            Key goldenKey = new Key("Golden Key", "A shiny key", "gold");
            bool unlocked = lockedRoom.Unlock(goldenKey);
            Debug.Assert(unlocked, "Room should be unlocked with the correct key");
            Debug.Assert(!lockedRoom.IsLocked(), "Room should no longer be locked");

            Log("Room tests passed!");
        }

        /// <summary>
        /// Tests the Item class and its derivatives
        /// </summary>
        private static void TestItem()
        {
            Log("Testing Item class hierarchy...");

            // Test base Item
            Item genericItem = new Item("Generic Item", "Just a test item");
            Debug.Assert(genericItem.Name == "Generic Item", "Item name not set correctly");
            Debug.Assert(genericItem.Description == "Just a test item", "Item description not set correctly");

            // Test Weapon
            Weapon sword = new Weapon("Sword", "A sharp sword", 10);
            Debug.Assert(sword.Name == "Sword", "Weapon name not set correctly");
            Debug.Assert(sword.Damage == 10, "Weapon damage not set correctly");
            Debug.Assert(sword.GetDetails().Contains("Damage: 10"), "Weapon details should include damage");

            // Test Potion
            Potion healingPotion = new Potion("Health Potion", "Restores health", 20);
            Debug.Assert(healingPotion.Name == "Health Potion", "Potion name not set correctly");
            Debug.Assert(healingPotion.HealAmount == 20, "Potion heal amount not set correctly");
            Debug.Assert(healingPotion.GetDetails().Contains("Heals: 20"), "Potion details should include heal amount");

            // Test Key
            Key treasureKey = new Key("Golden Key", "A shiny key", "treasure");
            Debug.Assert(treasureKey.Name == "Golden Key", "Key name not set correctly");
            Debug.Assert(treasureKey.UnlockCode == "treasure", "Key unlock code not set correctly");

            Log("Item tests passed!");
        }

        /// <summary>
        /// Tests the Inventory class functionality
        /// </summary>
        private static void TestInventory()
        {
            Log("Testing Inventory class...");

            // Create inventory and add items
            Inventory inventory = new Inventory();
            Debug.Assert(inventory.GetItemCount() == 0, "New inventory should be empty");
            Debug.Assert(inventory.ListItems() == "No items", "New inventory should report 'No items'");

            // Add different types of items
            Item genericItem = new Item("Generic Item", "Just a test item");
            Weapon sword = new Weapon("Sword", "A sharp sword", 10);
            Potion potion = new Potion("Health Potion", "Restores health", 20);

            inventory.AddItem(genericItem);
            inventory.AddItem(sword);
            inventory.AddItem(potion);

            Debug.Assert(inventory.GetItemCount() == 3, "Inventory should have 3 items");
            Debug.Assert(inventory.GetItems().Count == 3, "GetItems should return 3 items");

            // Test finding items
            Item foundItem = inventory.FindItem("Sword");
            Debug.Assert(foundItem != null, "FindItem should find the sword");
            Debug.Assert(foundItem.Name == "Sword", "Found item should be the sword");

            // Test LINQ filter for weapons
            List<Weapon> weapons = inventory.GetItemsByType<Weapon>();
            Debug.Assert(weapons.Count == 1, "There should be 1 weapon in inventory");
            Debug.Assert(weapons[0].Name == "Sword", "The weapon should be the sword");

            // Test removing items
            bool removed = inventory.RemoveItem("Health Potion");
            Debug.Assert(removed, "RemoveItem should return true for existing item");
            Debug.Assert(inventory.GetItemCount() == 2, "Inventory should have 2 items after removal");
            Debug.Assert(inventory.FindItem("Health Potion") == null, "Potion should be removed");

            Log("Inventory tests passed!");
        }

        /// <summary>
        /// Tests the Monster class and its derivatives
        /// </summary>
        private static void TestMonster()
        {
            Log("Testing Monster class...");

            // Test base Monster
            Item droppedItem = new Item("Monster Fang", "A sharp fang");
            Monster genericMonster = new Monster("Generic Monster", "Beast", 50, 10, droppedItem);
            
            Debug.Assert(genericMonster.Name == "Generic Monster", "Monster name not set correctly");
            Debug.Assert(genericMonster.Health == 50, "Monster health not set correctly");
            Debug.Assert(genericMonster.AttackPower == 10, "Monster attack power not set correctly");
            Debug.Assert(genericMonster.Type == "Beast", "Monster type not set correctly");
            
            // Test monster damage and death
            genericMonster.TakeDamage(40);
            Debug.Assert(genericMonster.Health == 10, "Monster health not reduced correctly");
            Debug.Assert(genericMonster.IsAlive(), "Monster should still be alive");
            
            genericMonster.TakeDamage(20);
            Debug.Assert(genericMonster.Health == 0, "Monster health should be 0");
            Debug.Assert(!genericMonster.IsAlive(), "Monster should be dead");
            
            // Test dropped item
            Item dropped = genericMonster.GetDroppedItem();
            Debug.Assert(dropped != null, "Monster should drop an item");
            Debug.Assert(dropped.Name == "Monster Fang", "Dropped item name is incorrect");
            
            // Test specific monster types
            Goblin goblin = new Goblin("Goblin Test");
            Debug.Assert(goblin.Type == "Goblin", "Goblin type is incorrect");
            Debug.Assert(goblin.Health == 15, "Goblin should have 15 health");
            
            Troll troll = new Troll("Troll Test");
            Debug.Assert(troll.Type == "Troll", "Troll type is incorrect");
            Debug.Assert(troll.Health == 30, "Troll should have 30 health");
            
            Dragon dragon = new Dragon("Dragon Test");
            Debug.Assert(dragon.Type == "Dragon", "Dragon type is incorrect");
            Debug.Assert(dragon.Health == 100, "Dragon should have 50 health");

            Log("Monster tests passed!");
        }

        /// <summary>
        /// Tests the GameMap class functionality
        /// </summary>
        private static void TestGameMap()
        {
            Log("Testing GameMap class...");

            // Create a simple map
            GameMap map = new GameMap();
            
            // Add rooms
            Room room1 = new Room("Room 1");
            Room room2 = new Room("Room 2");
            Room room3 = new Room("Room 3", true, "gold"); // Locked room
            
            bool added1 = map.AddRoom("room1", room1);
            bool added2 = map.AddRoom("room2", room2);
            bool added3 = map.AddRoom("room3", room3);
            
            Debug.Assert(added1 && added2 && added3, "Failed to add rooms to map");
            Debug.Assert(map.GetRoomCount() == 3, "Map should have 3 rooms");
            
            // Connect rooms
            bool connected1 = map.ConnectRooms("room1", "east", "room2");
            bool connected2 = map.ConnectRooms("room2", "west", "room1");
            bool connected3 = map.ConnectRooms("room2", "north", "room3");
            
            Debug.Assert(connected1 && connected2 && connected3, "Failed to connect rooms");
            
            // Test navigation
            List<string> directions1 = map.GetAvailableDirections("room1");
            Debug.Assert(directions1.Count == 1, "Room1 should have 1 exit");
            Debug.Assert(directions1[0] == "east", "Room1 should have east exit");
            
            string destination = map.GetDestinationRoomId("room1", "east");
            Debug.Assert(destination == "room2", "East from room1 should lead to room2");
            
            // Test default dungeon
            GameMap defaultMap = GameMap.CreateDefaultDungeon();
            Debug.Assert(defaultMap.GetRoomCount() > 0, "Default dungeon should have rooms");
            
            Log("GameMap tests passed!");
        }

        /// <summary>
        /// Tests the inheritance hierarchy of Creature class
        /// </summary>
        private static void TestCreatureHierarchy()
        {
            Log("Testing Creature hierarchy...");

            // Check if Player is a Creature
            Player player = new Player("Test Player", 100);
            Debug.Assert(player is Creature, "Player should inherit from Creature");
            Debug.Assert(player is IDamageable, "Player should implement IDamageable");

            // Check if Monster is a Creature
            Monster monster = new Monster("Test Monster", "Beast", 50, 10);
            Debug.Assert(monster is Creature, "Monster should inherit from Creature");
            Debug.Assert(monster is IDamageable, "Monster should implement IDamageable");

            // Check if goblin is a Monster
            Goblin goblin = new Goblin("Test Goblin");
            Debug.Assert(goblin is Monster, "Goblin should inherit from Monster");
            Debug.Assert(goblin is Creature, "Goblin should be a Creature via inheritance");

            Log("Creature hierarchy tests passed!");
        }

        /// <summary>
        /// Tests polymorphic behavior in the game
        /// </summary>
        private static void TestPolymorphism()
        {
            Log("Testing polymorphism...");

            // Test different attack behaviors
            Goblin goblin = new Goblin("Goblin");
            Troll troll = new Troll("Troll");
            Dragon dragon = new Dragon("Dragon");
            Player player = new Player("Hero", 100);

            // Give the player a weapon
            player.PickUpItem(new Weapon("Magic Sword", "A powerful sword", 15));

            // Array of creatures demonstrating polymorphism
            Creature[] creatures = { goblin, troll, dragon, player };

            // Each creature type should have different attack behavior
            foreach (Creature creature in creatures)
            {
                int damage = creature.Attack();
                Debug.Assert(damage >= 0, $"{creature.Name} attack damage should be non-negative");
                Log($"{creature.Name} attacks for {damage} damage");
            }

            // Test polymorphic method implementation
            foreach (Creature creature in creatures)
            {
                string description = creature.GetDescription();
                Debug.Assert(!string.IsNullOrEmpty(description), "Description should not be empty");
                Log(description);
            }

            // Test item polymorphism
            Item genericItem = new Item("Generic", "A generic item");
            Weapon weapon = new Weapon("Sword", "A sharp sword", 10);
            Potion potion = new Potion("Health Potion", "Restores health", 20);
            Key key = new Key("Golden Key", "A shiny key", "treasure");
            Torch torch = new Torch("Torch", "A flaming torch", 10);

            // Array of items demonstrating polymorphism
            Item[] items = { genericItem, weapon, potion, key, torch };

            // Each item type should have different details
            foreach (Item item in items)
            {
                string details = item.GetDetails();
                Debug.Assert(!string.IsNullOrEmpty(details), "Item details should not be empty");
                Log(details);
            }

            Log("Polymorphism tests passed!");
        }

        /// <summary>
        /// Helper method to throw an error with a message
        /// </summary>
        private static void ThrowError(string message)
        {
            Log($"ERROR: {message}");
            throw new Exception(message);
        }
        
        /// <summary>
        /// Tests the Statistics class functionality
        /// </summary>
        private static void TestStatistics()
        {
            Log("Testing Statistics class...");
            
            // Create a new statistics tracker
            Statistics stats = new Statistics();
            
            // Test initial values
            string initialReport = stats.GetStatisticsReport();
            Debug.Assert(initialReport.Contains("Monsters Defeated: 0"), "Initial monsters defeated should be 0");
            Debug.Assert(initialReport.Contains("Rooms Explored: 0"), "Initial rooms explored should be 0");
            Debug.Assert(initialReport.Contains("Items Collected: 0"), "Initial items collected should be 0");
            
            // Test recording monster defeats
            stats.RecordMonsterDefeated("Goblin");
            stats.RecordMonsterDefeated("Goblin");
            stats.RecordMonsterDefeated("Troll");
            
            string monsterReport = stats.GetStatisticsReport();
            Debug.Assert(monsterReport.Contains("Monsters Defeated: 3"), "Should have 3 monsters defeated");
            Debug.Assert(monsterReport.Contains("Goblin: 2"), "Should have 2 goblins defeated");
            Debug.Assert(monsterReport.Contains("Troll: 1"), "Should have 1 troll defeated");
            
            // Test recording rooms explored
            stats.RecordRoomExplored();
            stats.RecordRoomExplored();
            
            string roomReport = stats.GetStatisticsReport();
            Debug.Assert(roomReport.Contains("Rooms Explored: 2"), "Should have 2 rooms explored");
            
            // Test recording items collected
            stats.RecordItemCollected("Weapon");
            stats.RecordItemCollected("Potion");
            stats.RecordItemCollected("Weapon");
            
            string itemReport = stats.GetStatisticsReport();
            Debug.Assert(itemReport.Contains("Items Collected: 3"), "Should have 3 items collected");
            Debug.Assert(itemReport.Contains("Weapon: 2"), "Should have 2 weapons collected");
            Debug.Assert(itemReport.Contains("Potion: 1"), "Should have 1 potion collected");
            
            // Test damage tracking
            stats.RecordDamageDealt(10);
            stats.RecordDamageDealt(15, true); // Critical hit
            stats.RecordDamageDealt(20);
            
            stats.RecordDamageTaken(5);
            stats.RecordDamageTaken(10);
            
            string damageReport = stats.GetStatisticsReport();
            Debug.Assert(damageReport.Contains("Total Damage Dealt: 45"), "Should have dealt 45 total damage");
            Debug.Assert(damageReport.Contains("Total Damage Taken: 15"), "Should have taken 15 total damage");
            Debug.Assert(damageReport.Contains("Highest Damage Dealt: 20"), "Highest damage should be 20");
            Debug.Assert(damageReport.Contains("Critical Hits: 1"), "Should have 1 critical hit");
            
            // Test potion usage and player deaths
            stats.RecordPotionUsed();
            stats.RecordPotionUsed();
            stats.RecordPlayerDeath();
            
            string miscReport = stats.GetStatisticsReport();
            Debug.Assert(miscReport.Contains("Potions Used: 2"), "Should have used 2 potions");
            
            // Test time tracking
            stats.StartTimeTracking();
            // Wait a short time to ensure some time passes
            System.Threading.Thread.Sleep(100);
            stats.StopTimeTracking();
            
            string timeReport = stats.GetStatisticsReport();
            Debug.Assert(timeReport.Contains("Play Time:"), "Play time should be recorded");
            
            // Test reset functionality
            stats.Reset();
            string resetReport = stats.GetStatisticsReport();
            Debug.Assert(resetReport.Contains("Monsters Defeated: 0"), "Reset should set monsters defeated to 0");
            Debug.Assert(resetReport.Contains("Rooms Explored: 0"), "Reset should set rooms explored to 0");
            Debug.Assert(resetReport.Contains("Items Collected: 0"), "Reset should set items collected to 0");
            
            Log("Statistics tests passed!");
        }
    }
}