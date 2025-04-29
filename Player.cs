using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a player in the dungeon who can collect items and battle monsters
    /// </summary>
    public class Player : Creature
    {
        // The player's inventory - internal allows access from Game class
        internal Inventory _inventory;

        /// <summary>
        /// Creates a new player with the given name and health
        /// </summary>
        /// <param name="name">The player's name</param>
        /// <param name="health">The player's initial health</param>
        public Player(string name, int health) : base(name, health, 10)
        {
            _inventory = new Inventory();
        }

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="item">The item to add</param>
        public void PickUpItem(Item item)
        {
            if (item != null)
            {
                _inventory.AddItem(item);
                Console.WriteLine($"You picked up {item.Name}.");

                // If it's a weapon, equip it automatically if better than current attack power
                if (item is Weapon weapon && weapon.Damage > _attackPower)
                {
                    _attackPower = weapon.Damage;
                    Console.WriteLine($"You equipped {weapon.Name} (Damage: {weapon.Damage})");
                }
            }
        }

        /// <summary>
        /// Legacy method to add items by name (for backward compatibility)
        /// </summary>
        /// <param name="itemName">The name of the item to add</param>
        public void PickUpItem(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                Item item = new Item(itemName, "A mysterious item.");
                _inventory.AddItem(item);
            }
        }

        /// <summary>
        /// Returns a string containing all items in the player's inventory
        /// </summary>
        /// <returns>A comma-separated list of inventory items</returns>
        public string InventoryContents()
        {
            return _inventory.ListItems();
        }

        /// <summary>
        /// Uses an item from the inventory based on its name
        /// </summary>
        /// <param name="itemName">The name of the item to use</param>
        /// <returns>True if the item was used successfully, false otherwise</returns>
        public bool UseItem(string itemName)
        {
            try
            {
                // Try to find the item in inventory
                Item item = _inventory.FindItem(itemName);
                if (item == null)
                {
                    Console.WriteLine($"You don't have a {itemName}.");
                    return false;
                }

                // Use the item based on its type
                if (item is Potion potion)
                {
                    // Use the potion to heal
                    Heal(potion.HealAmount);
                    Console.WriteLine($"You used the {potion.Name} and restored {potion.HealAmount} health. Current health: {Health}");
                    _inventory.RemoveItem(potion);
                    return true;
                }
                else if (item is Torch torch)
                {
                    // Use the torch to light up the area
                    Console.WriteLine($"You hold up the {torch.Name}, illuminating the area. It will burn for {torch.BurnTime} more minutes.");
                    Console.WriteLine("This makes it easier to see details in the room.");
                    return true;
                }

                Console.WriteLine($"You can't use {item.Name}.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error trying to use item: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets all weapons in the player's inventory
        /// </summary>
        /// <returns>A list of weapons</returns>
        public List<Weapon> GetAllWeapons()
        {
            // Using LINQ to filter items that are weapons
            return _inventory.GetItems().OfType<Weapon>().ToList();
        }

        /// <summary>
        /// Gets all potions in the player's inventory
        /// </summary>
        /// <returns>A list of potions</returns>
        public List<Potion> GetAllPotions()
        {
            // Using LINQ to filter items that are potions
            return _inventory.GetItems().OfType<Potion>().ToList();
        }
        
        /// <summary>
        /// Gets all keys in the player's inventory
        /// </summary>
        /// <returns>A list of keys</returns>
        public List<Key> GetAllKeys()
        {
            // Using LINQ to filter items that are keys
            return _inventory.GetItems().OfType<Key>().ToList();
        }

        /// <summary>
        /// Override of the base Attack method to include weapon bonuses
        /// </summary>
        /// <returns>The amount of damage the player deals</returns>
        public override int Attack()
        {
            // Get the player's attack power (including equipped weapons)
            int damage = _attackPower;
            
            // Add randomness to the attack
            Random random = new Random();
            int variation = random.Next(-2, 3); // -2 to +2 damage variation
            
            return Math.Max(1, damage + variation); // Ensure at least 1 damage
        }

        /// <summary>
        /// Get a description of the player
        /// </summary>
        /// <returns>A string describing the player</returns>
        public override string GetDescription()
        {
            return $"{Name} - Health: {Health}/{MaxHealth} - Attack: {AttackPower}";
        }
    }
}