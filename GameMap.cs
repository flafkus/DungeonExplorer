using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the game map with multiple interconnected rooms
    /// </summary>
    public class GameMap
    {
        // Dictionary of rooms by their ID
        private Dictionary<string, Room> _rooms;
        
        // Dictionary of connections between rooms
        private Dictionary<string, Dictionary<string, string>> _connections;

        /// <summary>
        /// Creates a new empty game map
        /// </summary>
        public GameMap()
        {
            _rooms = new Dictionary<string, Room>();
            _connections = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        /// Adds a room to the map
        /// </summary>
        /// <param name="roomId">The unique ID for the room</param>
        /// <param name="room">The room to add</param>
        /// <returns>True if the room was added, false if a room with that ID already exists</returns>
        public bool AddRoom(string roomId, Room room)
        {
            try
            {
                if (string.IsNullOrEmpty(roomId) || room == null || _rooms.ContainsKey(roomId))
                {
                    return false;
                }

                _rooms.Add(roomId, room);
                _connections[roomId] = new Dictionary<string, string>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a room by its ID
        /// </summary>
        /// <param name="roomId">The ID of the room to get</param>
        /// <returns>The room, or null if not found</returns>
        public Room GetRoom(string roomId)
        {
            if (string.IsNullOrEmpty(roomId) || !_rooms.ContainsKey(roomId))
            {
                return null;
            }

            return _rooms[roomId];
        }

        /// <summary>
        /// Connects two rooms in a specified direction
        /// </summary>
        /// <param name="fromRoomId">The ID of the starting room</param>
        /// <param name="direction">The direction of the connection (e.g., "north", "east")</param>
        /// <param name="toRoomId">The ID of the destination room</param>
        /// <returns>True if the connection was added, false otherwise</returns>
        public bool ConnectRooms(string fromRoomId, string direction, string toRoomId)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(fromRoomId) || string.IsNullOrEmpty(direction) || 
                    string.IsNullOrEmpty(toRoomId) || !_rooms.ContainsKey(fromRoomId) || 
                    !_rooms.ContainsKey(toRoomId))
                {
                    return false;
                }

                // Add the connection
                _connections[fromRoomId][direction] = toRoomId;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the available directions from a specified room
        /// </summary>
        /// <param name="roomId">The ID of the room</param>
        /// <returns>A list of available directions, or an empty list if the room doesn't exist</returns>
        public List<string> GetAvailableDirections(string roomId)
        {
            if (string.IsNullOrEmpty(roomId) || !_connections.ContainsKey(roomId))
            {
                return new List<string>();
            }

            return _connections[roomId].Keys.ToList();
        }

        /// <summary>
        /// Gets the destination room ID when moving in a specific direction from a room
        /// </summary>
        /// <param name="roomId">The ID of the starting room</param>
        /// <param name="direction">The direction to move</param>
        /// <returns>The destination room ID, or null if there's no connection in that direction</returns>
        public string GetDestinationRoomId(string roomId, string direction)
        {
            if (string.IsNullOrEmpty(roomId) || string.IsNullOrEmpty(direction) || 
                !_connections.ContainsKey(roomId) || !_connections[roomId].ContainsKey(direction))
            {
                return null;
            }

            return _connections[roomId][direction];
        }

        /// <summary>
        /// Gets all room IDs in the map
        /// </summary>
        /// <returns>A list of all room IDs</returns>
        public List<string> GetAllRoomIds()
        {
            return _rooms.Keys.ToList();
        }

        /// <summary>
        /// Gets the number of rooms in the map
        /// </summary>
        /// <returns>The number of rooms</returns>
        public int GetRoomCount()
        {
            return _rooms.Count;
        }

        /// <summary>
        /// Creates a simple dungeon map with predefined rooms
        /// </summary>
        /// <returns>A GameMap instance with interconnected rooms</returns>
        public static GameMap CreateDefaultDungeon()
        {
            GameMap map = new GameMap();

            // Create rooms
            Room entrance = new Room("You are at the entrance of a dark, damp dungeon. Torches flicker on the walls, casting eerie shadows.");
            Room hallway = new Room("A long, narrow hallway stretches before you. The stone walls are covered in moss, and you hear distant dripping.");
            Room chamber = new Room("A large chamber with ancient pillars. There appears to be an altar in the center, stained with something dark.");
            Room library = new Room("A small room filled with rotting bookshelves. Most books have decayed, but a few tomes remain intact.");
            Room treasureRoom = new Room("A glittering room filled with gold coins and precious gems. However, it seems too good to be true.", true, "gold");
            Room bossRoom = new Room("A massive cavern with stalactites hanging from the ceiling. The air feels heavy with dread.");
            Room secretPassage = new Room("A narrow, hidden passage that seems to have been unused for centuries.");
            
            // Add rooms to map
            map.AddRoom("entrance", entrance);
            map.AddRoom("hallway", hallway);
            map.AddRoom("chamber", chamber);
            map.AddRoom("library", library);
            map.AddRoom("treasureRoom", treasureRoom);
            map.AddRoom("bossRoom", bossRoom);
            map.AddRoom("secretPassage", secretPassage);
            
            // Connect rooms
            map.ConnectRooms("entrance", "forward", "hallway");
            
            map.ConnectRooms("hallway", "back", "entrance");
            map.ConnectRooms("hallway", "left", "chamber");
            map.ConnectRooms("hallway", "right", "library");
            map.ConnectRooms("hallway", "forward", "bossRoom");
            
            map.ConnectRooms("chamber", "back", "hallway");
            map.ConnectRooms("chamber", "hidden", "secretPassage");
            
            map.ConnectRooms("library", "back", "hallway");
            
            map.ConnectRooms("secretPassage", "forward", "treasureRoom");
            map.ConnectRooms("secretPassage", "back", "chamber");
            
            map.ConnectRooms("treasureRoom", "back", "secretPassage");
            
            map.ConnectRooms("bossRoom", "back", "hallway");
            
            // Add items to rooms
            entrance.AddItem(new Torch("Wall Torch", "A flaming torch taken from the wall", 3));
            hallway.AddItem(new Key("Rusty Key", "An old, rusted key", "chamber"));
            chamber.AddItem(new Key("Golden Key", "A shiny golden key", "gold"));
            library.AddItem(new Potion("Health Potion", "A small vial of red liquid", 20));
            library.AddItem(new Item("Ancient Tome", "A book written in a strange language."));
            secretPassage.AddItem(new Weapon("Silver Dagger", "A finely crafted dagger with silver inlays", 8));
            treasureRoom.AddItem(new Weapon("Golden Sword", "A sword made of pure gold. Attack enemies with a sign of wealth.", 10));
            
            // Add monsters to rooms
            hallway.AddMonster(new Goblin("Sneaky Goblin", new Item("Goblin Ear", "A severed goblin ear. Gross.")));
            chamber.AddMonster(new Troll("Cave Troll", new Potion("Large Health Potion", "A large vial of red liquid", 40)));
            bossRoom.AddMonster(new Dragon("Ancient Dragon", new Weapon("Dragon Slayer", "A legendary sword with runes etched into the blade", 15)));
            
            return map;
        }
    }
}