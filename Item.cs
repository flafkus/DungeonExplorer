using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Interface for items that can be collected by the player
    /// </summary>
    public interface ICollectible
    {
        string Name { get; }
        string Description { get; }
        string GetDetails();
    }

    /// <summary>
    /// Base class for all items in the game
    /// </summary>
    public class Item : ICollectible
    {
        // Properties
        public string Name { get; protected set; }
        public string Description { get; protected set; }

        /// <summary>
        /// Creates a new item with the specified name and description
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <param name="description">The description of the item</param>
        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Gets detailed information about the item
        /// </summary>
        /// <returns>A string with the item's details</returns>
        public virtual string GetDetails()
        {
            return $"{Name}: {Description}";
        }
    }

    /// <summary>
    /// Represents a weapon that can be used by the player to deal damage
    /// </summary>
    public class Weapon : Item
    {
        // Properties
        public int Damage { get; private set; }

        /// <summary>
        /// Creates a new weapon with the specified parameters
        /// </summary>
        /// <param name="name">The name of the weapon</param>
        /// <param name="description">The description of the weapon</param>
        /// <param name="damage">The damage the weapon deals</param>
        public Weapon(string name, string description, int damage) 
            : base(name, description)
        {
            Damage = damage;
        }

        /// <summary>
        /// Gets detailed information about the weapon including its damage
        /// </summary>
        /// <returns>A string with the weapon's details</returns>
        public override string GetDetails()
        {
            return $"{Name}: {Description} (Damage: {Damage})";
        }
    }

    /// <summary>
    /// Represents a potion that can heal the player
    /// </summary>
    public class Potion : Item
    {
        // Properties
        public int HealAmount { get; private set; }

        /// <summary>
        /// Creates a new potion with the specified parameters
        /// </summary>
        /// <param name="name">The name of the potion</param>
        /// <param name="description">The description of the potion</param>
        /// <param name="healAmount">The amount of health the potion restores</param>
        public Potion(string name, string description, int healAmount)
            : base(name, description)
        {
            HealAmount = healAmount;
        }

        /// <summary>
        /// Gets detailed information about the potion including its healing power
        /// </summary>
        /// <returns>A string with the potion's details</returns>
        public override string GetDetails()
        {
            return $"{Name}: {Description} (Heals: {HealAmount})";
        }
    }

    /// <summary>
    /// Represents a key that can be used to unlock doors
    /// </summary>
    public class Key : Item
    {
        // Properties
        public string UnlockCode { get; private set; }

        /// <summary>
        /// Creates a new key with the specified parameters
        /// </summary>
        /// <param name="name">The name of the key</param>
        /// <param name="description">The description of the key</param>
        /// <param name="unlockCode">The code that determines what this key unlocks</param>
        public Key(string name, string description, string unlockCode)
            : base(name, description)
        {
            UnlockCode = unlockCode;
        }

        /// <summary>
        /// Gets detailed information about the key
        /// </summary>
        /// <returns>A string with the key's details</returns>
        public override string GetDetails()
        {
            return $"{Name}: {Description} (Key for: {UnlockCode})";
        }
    }
}