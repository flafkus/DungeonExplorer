using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Main game class that handles game flow and user interactions
    /// </summary>
    public class Game
    {
        private Player player;
        private GameMap gameMap;
        private string currentRoomId;
        private bool inCombat;
        private Monster currentEnemy;
        private Statistics statistics;

        /// <summary>
        /// Creates a new game with multiple rooms and initialises the player
        /// </summary>
        public Game()
        {
            SetupGame();
        }

        /// <summary>
        /// Sets up the initial game state with a player and multiple rooms
        /// </summary>
        private void SetupGame()
        {
            Console.WriteLine("Welcome to Dungeon Explorer!");
            Console.Write("Enter your name: ");
            string playerName = Console.ReadLine();

            // Create player with inputted name and 100 health
            player = new Player(playerName, 100);

            // Create game map
            gameMap = GameMap.CreateDefaultDungeon();

            // Set starting room
            currentRoomId = "entrance";
            inCombat = false;

            // Initialize statistics tracking
            statistics = new Statistics();
            statistics.StartTimeTracking();

            Console.WriteLine($"Welcome, {player.Name}! Your adventure begins at the dungeon entrance...");
        }

        /// <summary>
        /// Gets the current room the player is in
        /// </summary>
        /// <returns>The current room object</returns>
        private Room GetCurrentRoom()
        {
            return gameMap.GetRoom(currentRoomId);
        }

        /// <summary>
        /// Displays the available commands to the player
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\n=== COMMANDS ===");
            Console.WriteLine("look           - Look around the room");
            Console.WriteLine("status         - Check health & basic inventory");
            Console.WriteLine("inventory      - See detailed inventory");
            Console.WriteLine("stats          - View your game statistics");
            Console.WriteLine("take [#/name]  - Take item by number or name");
            Console.WriteLine("use [name]     - Use an item (potions, torch, etc.)");
            Console.WriteLine("discard [name] - Discard an item from your inventory");
            Console.WriteLine("move [dir]     - Move in a direction");

            // Only show combat commands if in combat
            if (inCombat)
            {
                Console.WriteLine("attack         - Attack the current monster");
                Console.WriteLine("flee           - Try to escape from combat");
            }

            Console.WriteLine("help           - Show this menu");
            Console.WriteLine("exit           - Exit the game");
        }

        /// <summary>
        /// Attempts to move the player in the specified direction
        /// </summary>
        /// <param name="direction">The direction to move</param>
        private void MovePlayer(string direction)
        {
            try
            {
                // Check if player is in combat
                if (inCombat)
                {
                    Console.WriteLine("You can't move while in combat! Defeat the monster first or try to flee.");
                    return;
                }

                // Check if there's a connection in the specified direction
                string destinationRoomId = gameMap.GetDestinationRoomId(currentRoomId, direction);
                if (destinationRoomId == null)
                {
                    Console.WriteLine($"You cannot move {direction} from here.");
                    return;
                }

                // Get the destination room
                Room destinationRoom = gameMap.GetRoom(destinationRoomId);
                if (destinationRoom == null)
                {
                    Console.WriteLine("Error: Destination room not found.");
                    return;
                }

                // Check if the room is locked
                if (destinationRoom.IsLocked())
                {
                    // Get all keys in player's inventory
                    var keys = player.GetAllKeys();

                    // Try each key to see if it unlocks the room
                    bool unlocked = false;
                    foreach (Key key in keys)
                    {
                        if (destinationRoom.Unlock(key))
                        {
                            Console.WriteLine($"You automatically use the {key.Name} to unlock the door.");
                            unlocked = true;
                            break;
                        }
                    }

                    if (!unlocked)
                    {
                        Console.WriteLine("This room is locked. You need to find the right key to unlock it.");
                        return;
                    }
                }

                // Move to the destination room
                currentRoomId = destinationRoomId;
                Console.WriteLine($"You move {direction}.");
                Console.WriteLine("\n=== LOCATION ===");
                Console.WriteLine(GetCurrentRoom().GetDescription());

                // Record room explored in statistics
                statistics.RecordRoomExplored();

                // Check for items
                if (GetCurrentRoom().HasItems())
                {
                    Console.WriteLine("\n=== ITEMS ===");
                    Console.WriteLine(GetCurrentRoom().GetItemsDescription());
                }

                // Check for monsters
                if (GetCurrentRoom().HasMonsters())
                {
                    Console.WriteLine("\n=== MONSTERS ===");
                    Console.WriteLine(GetCurrentRoom().GetMonstersDescription());
                    StartCombat();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error moving: {ex.Message}");
            }
        }

        /// <summary>
        /// Starts combat with a monster in the room
        /// </summary>
        private void StartCombat()
        {
            try
            {
                // Check if there are monsters in the room
                if (!GetCurrentRoom().HasMonsters())
                {
                    Console.WriteLine("There are no monsters to fight here.");
                    return;
                }

                // Get the strongest monster to fight
                currentEnemy = GetCurrentRoom().GetStrongestMonster();
                inCombat = true;

                Console.WriteLine("\n=== ENTERING COMBAT ===");
                Console.WriteLine($"You enter combat with {currentEnemy.Name}!");
                Console.WriteLine($"{currentEnemy.GetDescription()}");
                Console.WriteLine("\nType 'attack' to fight or try to 'flee'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting combat: {ex.Message}");
                inCombat = false;
            }
        }

        /// <summary>
        /// Processes player attack in combat
        /// </summary>
        private void ProcessAttack()
        {
            try
            {
                if (!inCombat || currentEnemy == null)
                {
                    Console.WriteLine("There's nothing to attack right now.");
                    return;
                }

                Console.WriteLine("\n=== IN COMBAT ===");

                // Player attacks first
                int playerDamage = player.Attack();
                Console.WriteLine($"You attack {currentEnemy.Name} for {playerDamage} damage!");
                currentEnemy.TakeDamage(playerDamage);

                // Record damage dealt in statistics
                bool isCritical = playerDamage > player.AttackPower + 1; // Simple critical hit detection
                statistics.RecordDamageDealt(playerDamage, isCritical);

                // Check if monster is defeated
                if (!currentEnemy.IsAlive())
                {
                    Console.WriteLine($"\n{currentEnemy.Name} has been defeated!");

                    // Check for dropped item
                    Item droppedItem = currentEnemy.GetDroppedItem();
                    if (droppedItem != null)
                    {
                        Console.WriteLine($"{currentEnemy.Name} dropped {droppedItem.Name}!");
                        GetCurrentRoom().AddItem(droppedItem);
                    }

                    // Record monster defeated in statistics
                    statistics.RecordMonsterDefeated(currentEnemy.Name);

                    // Remove monster from room
                    GetCurrentRoom().RemoveMonster(currentEnemy);
                    inCombat = false;
                    currentEnemy = null;

                    // Check if there are more monsters
                    if (GetCurrentRoom().HasMonsters())
                    {
                        Console.WriteLine("\nThere are still monsters in this room:");
                        Console.WriteLine(GetCurrentRoom().GetMonstersDescription());
                        StartCombat();
                    }

                    return;
                }

                // Monster attacks back
                Console.WriteLine($"\n{currentEnemy.Name} health: {currentEnemy.Health}");
                Console.WriteLine("\nMonster's turn:");
                int monsterDamage = currentEnemy.Attack();
                Console.WriteLine($"{currentEnemy.Name} attacks you for {monsterDamage} damage!");
                player.TakeDamage(monsterDamage);
                Console.WriteLine($"\nYour health: {player.Health}");

                // Record damage taken in statistics
                statistics.RecordDamageTaken(monsterDamage);

                // Check if player is defeated
                if (!player.IsAlive())
                {
                    Console.WriteLine("\n=== GAME OVER ===");
                    Console.WriteLine("You have been defeated! Game over.");

                    // Record player death in statistics
                    statistics.RecordPlayerDeath();

                    // Display final statistics
                    statistics.StopTimeTracking();
                    Console.WriteLine(statistics.GetStatisticsReport());

                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during attack: {ex.Message}");
            }
        }

        /// <summary>
        /// Attempts to flee from combat
        /// </summary>
        private void TryToFlee()
        {
            if (!inCombat)
            {
                Console.WriteLine("You're not in combat.");
                return;
            }

            // 50% chance to flee
            Random random = new Random();
            if (random.Next(2) == 0)
            {
                Console.WriteLine("You successfully flee from combat!");
                inCombat = false;
                currentEnemy = null;

                // Move back to the previous room if possible
                string previousRoomId = gameMap.GetDestinationRoomId(currentRoomId, "back");
                if (previousRoomId != null)
                {
                    currentRoomId = previousRoomId;
                    Console.WriteLine("You retreat to the previous room.");
                    Console.WriteLine(GetCurrentRoom().GetDescription());
                }
            }
            else
            {
                Console.WriteLine("You failed to flee!");

                // Monster gets a free attack
                int monsterDamage = currentEnemy.Attack();
                Console.WriteLine($"{currentEnemy.Name} attacks you for {monsterDamage} damage while you try to flee!");
                player.TakeDamage(monsterDamage);
                Console.WriteLine($"Your health: {player.Health}");

                // Record damage taken in statistics
                statistics.RecordDamageTaken(monsterDamage);

                // Check if player is defeated
                if (!player.IsAlive())
                {
                    Console.WriteLine("You have been defeated! Game over.");

                    // Record player death in statistics
                    statistics.RecordPlayerDeath();

                    // Display final statistics
                    statistics.StopTimeTracking();
                    Console.WriteLine(statistics.GetStatisticsReport());

                    Console.WriteLine("Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// Takes an item from the current room
        /// </summary>
        /// <param name="itemNameOrNumber">The name or number of the item to take</param>
        private void TakeItem(string itemNameOrNumber)
        {
            try
            {
                if (inCombat)
                {
                    Console.WriteLine("You can't take items while in combat!");
                    return;
                }

                if (string.IsNullOrEmpty(itemNameOrNumber))
                {
                    Console.WriteLine("Please specify an item to take.");
                    return;
                }

                Item item = null;
                // Check if the parameter is a number
                if (int.TryParse(itemNameOrNumber, out int itemNumber) && itemNumber > 0)
                {
                    // Get items in the room
                    List<Item> roomItems = GetCurrentRoom().GetItems();

                    // Check if the number is valid
                    if (itemNumber <= roomItems.Count)
                    {
                        // Get the item by index (adjusting for 1-based numbering)
                        string itemName = roomItems[itemNumber - 1].Name;
                        item = GetCurrentRoom().TakeItem(itemName);
                    }
                    else
                    {
                        Console.WriteLine($"There is no item number {itemNumber} in this room.");
                        return;
                    }
                }
                else
                {
                    // Try to take the item by name
                    item = GetCurrentRoom().TakeItem(itemNameOrNumber);
                }

                if (item != null)
                {
                    player.PickUpItem(item);
                    // Note: TakeItem already removes the item from the room, so no need to call RemoveItem
                    
                    // Record item collected in statistics
                    string itemType = item.GetType().Name;
                    statistics.RecordItemCollected(itemType);
                }
                else
                {
                    Console.WriteLine($"There is no {itemNameOrNumber} to take here.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error taking item: {ex.Message}");
            }
        }

        /// <summary>
        /// Uses an item from the player's inventory
        /// </summary>
        /// <param name="itemName">The name of the item to use</param>
        private void UseItem(string itemName)
        {
            try
            {
                if (string.IsNullOrEmpty(itemName))
                {
                    Console.WriteLine("Please specify an item to use.");
                    return;
                }

                // Handle special case for keys
                if (itemName.ToLower().Contains("key"))
                {
                    // Get available directions
                    List<string> directions = gameMap.GetAvailableDirections(currentRoomId);

                    // Check each direction for a locked room
                    foreach (string direction in directions)
                    {
                        string destinationRoomId = gameMap.GetDestinationRoomId(currentRoomId, direction);
                        Room destinationRoom = gameMap.GetRoom(destinationRoomId);

                        if (destinationRoom != null && destinationRoom.IsLocked())
                        {
                            // Try to find the key in the player's inventory
                            var keys = player.GetAllWeapons().OfType<Key>().ToList();
                            Key key = keys.FirstOrDefault(k => k.Name.Contains(itemName));

                            if (key != null && destinationRoom.Unlock(key))
                            {
                                Console.WriteLine($"You unlocked the {direction} door with {key.Name}!");
                                return;
                            }
                        }
                    }

                    Console.WriteLine("There's no locked door nearby that this key fits.");
                    return;
                }

                // Use the item
                bool used = player.UseItem(itemName);
                if (!used)
                {
                    Console.WriteLine($"You couldn't use {itemName}.");
                }

                // Record potion used in statistics
                if (itemName.ToLower().Contains("potion"))
                {
                    statistics.RecordPotionUsed();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error using item: {ex.Message}");
            }
        }

        /// <summary>
        /// Displays the player's detailed inventory
        /// </summary>
        private void DisplayInventory()
        {
            Console.WriteLine("\n=== YOUR INVENTORY ===");

            // Get all items by type using LINQ
            var weapons = player.GetAllWeapons();
            var potions = player.GetAllPotions();
            var keys = player.GetAllKeys();

            if (weapons.Count > 0)
            {
                Console.WriteLine("\nWeapons:");
                foreach (var weapon in weapons)
                {
                    Console.WriteLine($"- {weapon.GetDetails()}");
                }
            }

            if (potions.Count > 0)
            {
                Console.WriteLine("\nPotions:");
                foreach (var potion in potions)
                {
                    Console.WriteLine($"- {potion.GetDetails()}");
                }
            }

            if (keys.Count > 0)
            {
                Console.WriteLine("\nKeys:");
                foreach (var key in keys)
                {
                    Console.WriteLine($"- {key.GetDetails()}");
                }
            }

            // Other items
            var otherItems = player._inventory.GetItems().Where(i =>
                !(i is Weapon) && !(i is Potion) && !(i is Key)).ToList();

            if (otherItems.Count > 0)
            {
                Console.WriteLine("\nOther Items:");
                foreach (var item in otherItems)
                {
                    Console.WriteLine($"- {item.GetDetails()}");
                }
            }

            if (player._inventory.GetItemCount() == 0)
            {
                Console.WriteLine("\nYour inventory is empty.");
            }
        }

        /// <summary>
        /// Processes player commands
        /// </summary>
        /// <param name="command">The command entered by the player</param>
        /// <returns>True if the game should continue, false if the player wants to exit</returns>
        private bool ProcessCommand(string command)
        {
            try
            {
                string lowerCommand = command.ToLower().Trim();

                // Extract the action and parameter
                string action = lowerCommand;
                string parameter = "";

                int spaceIndex = lowerCommand.IndexOf(' ');
                if (spaceIndex > 0)
                {
                    action = lowerCommand.Substring(0, spaceIndex);
                    parameter = lowerCommand.Substring(spaceIndex + 1).Trim();
                }

                switch (action)
                {
                    case "look":
                        Console.WriteLine("\n=== ROOM DESCRIPTION ===");
                        Console.WriteLine(GetCurrentRoom().GetDescription());

                        // Show items
                        if (GetCurrentRoom().HasItems())
                        {
                            Console.WriteLine("\n=== ITEMS ===");
                            Console.WriteLine(GetCurrentRoom().GetItemsDescription());
                        }

                        // Show monsters
                        if (GetCurrentRoom().HasMonsters())
                        {
                            Console.WriteLine("\n=== MONSTERS ===");
                            Console.WriteLine(GetCurrentRoom().GetMonstersDescription());
                        }

                        // Show available exits
                        Console.WriteLine("\n=== EXITS ===");
                        List<string> directions = gameMap.GetAvailableDirections(currentRoomId);
                        if (directions.Count > 0)
                        {
                            foreach (string direction in directions)
                            {
                                string destinationRoomId = gameMap.GetDestinationRoomId(currentRoomId, direction);
                                Room destinationRoom = gameMap.GetRoom(destinationRoomId);

                                if (destinationRoom.IsLocked())
                                {
                                    Console.WriteLine($"- {direction}: locked door");
                                }
                                else
                                {
                                    // Get just the first part of the description
                                    string shortDesc = destinationRoom.GetDescription().Split('.')[0];
                                    Console.WriteLine($"- {direction}: {shortDesc}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("No visible exits.");
                        }
                        return true;

                    case "status":
                        Console.WriteLine("\n=== CHARACTER STATUS ===");
                        Console.WriteLine($"Name: {player.Name}");
                        Console.WriteLine($"Health: {player.Health}/{player.MaxHealth}");
                        Console.WriteLine($"Attack Power: {player.AttackPower}");

                        Console.WriteLine("\n=== QUICK INVENTORY ===");
                        Console.WriteLine(player.InventoryContents());
                        return true;

                    case "inventory":
                        DisplayInventory();
                        return true;

                    case "take":
                        TakeItem(parameter);
                        return true;

                    case "use":
                        UseItem(parameter);
                        return true;
                        
                    case "discard":
                        DiscardItem(parameter);
                        return true;

                    case "move":
                        MovePlayer(parameter);
                        return true;

                    case "attack":
                        ProcessAttack();
                        return true;

                    case "flee":
                        TryToFlee();
                        return true;

                    case "help":
                        DisplayHelp();
                        return true;

                    case "stats":
                        statistics.DisplayStatistics();
                        return true;

                    case "exit":
                        Console.WriteLine("Thanks for playing Dungeon Explorer!");
                        // Stop tracking time and display final statistics
                        statistics.StopTimeTracking();
                        statistics.DisplayStatistics();
                        return false;

                    default:
                        Console.WriteLine("Invalid Command. Type 'help' for a list of commands.");
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing command: {ex.Message}");
                return true;
            }
        }

        /// <summary>
        /// Discards an item from the player's inventory
        /// </summary>
        /// <param name="itemName">The name of the item to discard</param>
        private void DiscardItem(string itemName)
        {
            try
            {
                if (inCombat)
                {
                    Console.WriteLine("You can't discard items while in combat!");
                    return;
                }

                if (string.IsNullOrEmpty(itemName))
                {
                    Console.WriteLine("Please specify an item to discard.");
                    return;
                }

                // Use the player's DiscardItem method
                player.DiscardItem(itemName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error discarding item: {ex.Message}");
            }
        }

        /// <summary>
        /// Clears the console and adds a separator line
        /// </summary>
        private void DisplaySeparator()
        {
            Console.WriteLine("\n" + new string('-', 60) + "\n");
        }

        /// <summary>
        /// Starts the game loop and handles user input
        /// </summary>
        public void Start()
        {
            // Display the initial room description
            Console.Clear();
            Console.WriteLine("\n=== DUNGEON EXPLORER ===\n");
            Console.WriteLine(GetCurrentRoom().GetDescription());
            if (GetCurrentRoom().HasItems())
            {
                Console.WriteLine(GetCurrentRoom().GetItemsDescription());
            }

            DisplayHelp();

            // Change the playing logic into true and populate the while loop
            bool playing = true;
            while (playing)
            {
                Console.Write("\n> ");
                string command = Console.ReadLine();

                // Add separator for better readability
                DisplaySeparator();

                // Process the command and check if we should continue playing
                playing = ProcessCommand(command);

                // Check if player has died
                if (!player.IsAlive())
                {
                    Console.WriteLine("You have died! Game over.");
                    playing = false;
                }
            }
        }
    }
}