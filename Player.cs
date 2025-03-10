using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a player in the dungeon with a name, health, and inventory
    /// </summary>
    public class Player
    {
        // Backing fields
        private string _name;
        private int _health;
        private List<string> _inventory = new List<string>();

        // Properties
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }

        public int Health
        {
            get { return _health; }
            private set { _health = value; }
        }

        /// <summary>
        /// Creates a new player with the given name and health
        /// </summary>
        /// <param name="name">The player's name</param>
        /// <param name="health">The player's initial health</param>
        public Player(string name, int health)
        {
            _name = name;
            _health = health;
        }

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void PickUpItem(string item)
        {
            if (!string.IsNullOrEmpty(item))
            {
                _inventory.Add(item);
            }
        }

        /// <summary>
        /// Returns a string containing all items in the player's inventory
        /// </summary>
        /// <returns>A comma-separated list of inventory items</returns>
        public string InventoryContents()
        {
            if (_inventory.Count == 0)
            {
                return "No items";
            }
            return string.Join(", ", _inventory);
        }

        /// <summary>
        /// Reduces the player's health by the specified amount
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                _health -= damage;
                // Ensure health doesn't go below 0
                if (_health < 0)
                {
                    _health = 0;
                }
            }
        }

        /// <summary>
        /// Checks if the player is still alive
        /// </summary>
        /// <returns>True if the player has health remaining, false otherwise</returns>
        public bool IsAlive()
        {
            return _health > 0;
        }
    }
}