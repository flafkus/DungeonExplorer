using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Main game class that handling game flow and user interactions
    /// </summary>
    internal class Game

    {
        private Player player;
        private Room[] rooms;
        private int currentRoomIndex;

        // Room connection constants
        private const int ENTRANCE = 0;
        private const int HALLWAY = 1;
        private const int CHAMBER = 2;
        private const int LIBRARY = 3;

        // Room connection structure
        private Dictionary<int, Dictionary<string, int>> roomConnections;

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

            // Create rooms
            rooms = new Room[4];
            rooms[ENTRANCE] = new Room("You are at the entrance of a dark, damp dungeon. A hallway lies ahead.", "Torch");
            rooms[HALLWAY] = new Room("A long, narrow hallway. There are doors to the left and right.", "Rusty Key");
            rooms[CHAMBER] = new Room("A large chamber with ancient pillars. There appears to be an altar in the center.", "Silver Key");
            rooms[LIBRARY] = new Room("A small room filled with rotting bookshelves. Most books have decayed.", "Dusty Tome");

            // Initialize room connections
            SetupRoomConnections();

            // Set starting room
            currentRoomIndex = ENTRANCE;

            Console.WriteLine($"Welcome, {player.Name}! Your adventure begins at the dungeon entrance...");
        }

        /// <summary>
        /// Sets up the connections between rooms
        /// </summary>
        private void SetupRoomConnections()
        {
            roomConnections = new Dictionary<int, Dictionary<string, int>>();

            // Set up connections for entrance room
            roomConnections[ENTRANCE] = new Dictionary<string, int>
            {
                { "forward", HALLWAY }
            };

            // Set up connections for hallway room
            roomConnections[HALLWAY] = new Dictionary<string, int>
            {
                { "back", ENTRANCE },
                { "left", CHAMBER },
                { "right", LIBRARY }
            };

            // Set up connections for chamber room
            roomConnections[CHAMBER] = new Dictionary<string, int>
            {
                { "back", HALLWAY }
            };

            // Set up connections for library room
            roomConnections[LIBRARY] = new Dictionary<string, int>
            {
                { "back", HALLWAY }
            };
        }

        /// <summary>
        /// Gets the current room the player is in
        /// </summary>
        /// <returns>The current room object</returns>
        private Room GetCurrentRoom()
        {
            return rooms[currentRoomIndex];
        }

        /// <summary>
        /// Displays the available commands to the player
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("look - Look around the current room");
            Console.WriteLine("status - Check your health and inventory");
            Console.WriteLine("take - Take the item in the room");
            Console.WriteLine("move [forward/back/left/right] - Moves in the specified direction");
            Console.WriteLine("help - Display this help message");
            Console.WriteLine("exit - Exit the game");
        }

        /// <summary>
        /// Attempts to move the player in the specified direction
        /// </summary>
        /// <param name="direction">The direction to move (forward, back, left, right)</param>
        private void MovePlayer(string direction)
        {
            // Check if the current room has a connection in the specified direction
            if (roomConnections.ContainsKey(currentRoomIndex) &&
                roomConnections[currentRoomIndex].ContainsKey(direction))
            {
                // Get the next room index
                int nextRoom = roomConnections[currentRoomIndex][direction];

                // Move to the next room
                currentRoomIndex = nextRoom;
                Console.WriteLine($"You move {direction}.");
                Console.WriteLine(GetCurrentRoom().GetDescription());
                if (GetCurrentRoom().HasItem())
                {
                    Console.WriteLine($"You see a {GetCurrentRoom().GetItemName()} here.");
                }
            }
            else
            {
                Console.WriteLine($"You cannot move {direction} from here.");
            }
        }

        /// <summary>
        /// Processes player commands
        /// </summary>
        /// <param name="command">The command entered by the player</param>
        /// <returns>True if the game should continue, false if the player wants to exit</returns>
        private bool ProcessCommand(string command)
        {
            string lowerCommand = command.ToLower().Trim();

            switch (lowerCommand)
            {
                case "look":
                    Console.WriteLine(GetCurrentRoom().GetDescription());
                    if (GetCurrentRoom().HasItem())
                    {
                        Console.WriteLine($"You see a {GetCurrentRoom().GetItemName()} here.");
                    }

                    // Show available exits
                    Console.WriteLine("Possible directions:");
                    if (roomConnections.ContainsKey(currentRoomIndex))
                    {
                        foreach (var direction in roomConnections[currentRoomIndex].Keys)
                        {
                            int targetRoom = roomConnections[currentRoomIndex][direction];
                            Console.WriteLine($"  {direction}: leads to {GetRoomDescription(targetRoom)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  None");
                    }
                    return true;

                case "status":
                    Console.WriteLine($"Health: {player.Health}");
                    Console.WriteLine($"Inventory: {player.InventoryContents()}");
                    return true;

                case "take":
                    if (GetCurrentRoom().HasItem())
                    {
                        string item = GetCurrentRoom().TakeItem();
                        player.PickUpItem(item);
                        Console.WriteLine($"You picked up the {item}.");
                    }
                    else
                    {
                        Console.WriteLine("There is nothing to take here.");
                    }
                    return true;

                case "move forward":
                    MovePlayer("forward");
                    return true;

                case "move back":
                    MovePlayer("back");
                    return true;

                case "move left":
                    MovePlayer("left");
                    return true;

                case "move right":
                    MovePlayer("right");
                    return true;

                case "help":
                    DisplayHelp();
                    return true;

                case "exit":
                    Console.WriteLine("Thank you for playing Dungeon Explorer!");
                    return false;

                default:
                    Console.WriteLine("Invalid Command. Type 'help' for a list of commands.");
                    return true;
            }
        }

        /// <summary>
        /// Gets a simple description of a room based on its index
        /// </summary>
        /// <param name="roomIndex">The index of the room</param>
        /// <returns>A simple description of the room</returns>
        private string GetRoomDescription(int roomIndex)
        {
            switch (roomIndex)
            {
                case ENTRANCE:
                    return "the entrance";
                case HALLWAY:
                    return "a hallway";
                case CHAMBER:
                    return "a chamber";
                case LIBRARY:
                    return "a library";
                default:
                    return "an unknown location";
            }
        }

        /// <summary>
        /// Starts the game loop and handles user input
        /// </summary>
        public void Start()
        {
            // Display the initial room description
            Console.WriteLine(GetCurrentRoom().GetDescription());
            if (GetCurrentRoom().HasItem())
            {
                Console.WriteLine($"You see a {GetCurrentRoom().GetItemName()} here.");
            }

            DisplayHelp();

            // Change the playing logic into true and populate the while loop
            bool playing = true;
            while (playing)
            {
                Console.Write("\n> ");
                string command = Console.ReadLine();

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