namespace DungeonExplorer
{
    /// <summary>
    /// Represents a room in the dungeon with a description and an item
    /// </summary>
    public class Room
    {
        private string description;
        private string item;

        /// <summary>
        /// Creates a new room with the given description and optional item
        /// </summary>
        /// <param name="description">The description of the room</param>
        /// <param name="item">Optional item contained in the room</param>
        public Room(string description, string item = null)
        {
            this.description = description;
            this.item = item;
        }

        /// <summary>
        /// Returns the description of the room
        /// </summary>
        /// <returns>The room description</returns>
        public string GetDescription()
        {
            return description;
        }

        /// <summary>
        /// Checks if the room contains an item
        /// </summary>
        /// <returns>True if the room has an item, false otherwise</returns>
        public bool HasItem()
        {
            return !string.IsNullOrEmpty(item);
        }

        /// <summary>
        /// Gets the item in the room and removes it
        /// </summary>
        /// <returns>The item that was in the room, or null if there was no item</returns>
        public string TakeItem()
        {
            string takenItem = item;
            item = null;
            return takenItem;
        }

        /// <summary>
        /// Gets the name of the item in the room without removing it
        /// </summary>
        /// <returns>The name of the item, or null if there is no item</returns>
        public string GetItemName()
        {
            return item;
        }
    }
}