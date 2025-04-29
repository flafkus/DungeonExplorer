using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a torch that provides light and can be used as a weak weapon
    /// </summary>
    public class Torch : Weapon
    {
        // Properties
        public int BurnTime { get; private set; }

        /// <summary>
        /// Creates a new torch with the specified parameters
        /// </summary>
        /// <param name="name">The name of the torch</param>
        /// <param name="description">The description of the torch</param>
        /// <param name="burnTime">How long the torch will burn in minutes</param>
        public Torch(string name, string description, int burnTime)
            : base(name, description, 3) // Torch does 3 damage as a weapon
        {
            BurnTime = burnTime;
        }

        /// <summary>
        /// Gets detailed information about the torch including its burn time
        /// </summary>
        /// <returns>A string with the torch's details</returns>
        public override string GetDetails()
        {
            return $"{Name}: {Description} (Damage: {Damage}, Burn Time: {BurnTime} minutes)";
        }
    }
}