using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a monster that can attack the player
    /// </summary>
    public class Monster : Creature
    {
        // Properties
        public string Type { get; private set; }
        public Item DroppedItem { get; private set; }

        /// <summary>
        /// Creates a new monster with the specified parameters
        /// </summary>
        /// <param name="name">The monster's name</param>
        /// <param name="type">The type of monster (e.g., Goblin, Dragon)</param>
        /// <param name="health">The monster's initial health</param>
        /// <param name="attackPower">The monster's attack power</param>
        /// <param name="droppedItem">The item dropped when the monster is defeated (optional)</param>
        public Monster(string name, string type, int health, int attackPower, Item droppedItem = null)
            : base(name, health, attackPower)
        {
            Type = type;
            DroppedItem = droppedItem;
        }

        /// <summary>
        /// Returns a description of the monster
        /// </summary>
        /// <returns>A string describing the monster</returns>
        public override string GetDescription()
        {
            return $"{Name} ({Type}) - Health: {Health} - Attack: {AttackPower}";
        }

        /// <summary>
        /// Gets the item dropped by the monster when defeated
        /// </summary>
        /// <returns>The dropped item, or null if the monster doesn't drop anything</returns>
        public Item GetDroppedItem()
        {
            return DroppedItem;
        }

        /// <summary>
        /// Override of the base Attack method to include monster-specific behavior
        /// </summary>
        /// <returns>The amount of damage the monster deals</returns>
        public override int Attack()
        {
            // Add randomness to the attack
            Random random = new Random();
            
            // Different types of monsters have different attack patterns
            switch (Type.ToLower())
            {
                case "goblin":
                    // Goblins have a chance to miss
                    if (random.Next(1, 5) == 1) // 1 in 4 chance to miss
                    {
                        return 0;
                    }
                    break;
                    
                case "troll":
                    // Trolls have a chance to deal extra damage
                    if (random.Next(1, 4) == 1) // 1 in 3 chance for critical hit
                    {
                        return _attackPower * 2;
                    }
                    break;
                    
                case "dragon":
                    // Dragons always deal at least their attack power
                    return _attackPower + random.Next(3, 9); // Extra damage between 3 and 8
                    
                default:
                    // Default behavior for other monster types
                    break;
            }
            
            // Standard attack with slight variation
            int variation = random.Next(-1, 2); // -1 to +1 damage variation
            return Math.Max(1, _attackPower + variation); // Ensure at least 1 damage
        }
    }

    /// <summary>
    /// A goblin monster with specific attributes
    /// </summary>
    public class Goblin : Monster
    {
        /// <summary>
        /// Creates a new goblin with standard attributes
        /// </summary>
        /// <param name="name">The goblin's name</param>
        /// <param name="droppedItem">The item dropped when the goblin is defeated (optional)</param>
        public Goblin(string name, Item droppedItem = null)
            : base(name, "Goblin", 15, 5, droppedItem)
        {
        }
    }

    /// <summary>
    /// A troll monster with specific attributes
    /// </summary>
    public class Troll : Monster
    {
        /// <summary>
        /// Creates a new troll with standard attributes
        /// </summary>
        /// <param name="name">The troll's name</param>
        /// <param name="droppedItem">The item dropped when the troll is defeated (optional)</param>
        public Troll(string name, Item droppedItem = null)
            : base(name, "Troll", 30, 8, droppedItem)
        {
        }
    }

    /// <summary>
    /// A dragon monster with specific attributes
    /// </summary>
    public class Dragon : Monster
    {
        /// <summary>
        /// Creates a new dragon with standard attributes
        /// </summary>
        /// <param name="name">The dragon's name</param>
        /// <param name="droppedItem">The item dropped when the dragon is defeated (optional)</param>
        public Dragon(string name, Item droppedItem = null)
            : base(name, "Dragon", 100, 12, droppedItem)
        {
        }
    }
}