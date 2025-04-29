using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Interface for objects that can take damage
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(int damage);
        bool IsAlive();
        int GetHealth();
    }

    /// <summary>
    /// Abstract base class for all living entities in the game
    /// </summary>
    public abstract class Creature : IDamageable
    {
        // Protected fields available to child classes
        protected string _name;
        protected int _health;
        protected int _maxHealth;
        protected int _attackPower;

        // Properties
        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public int Health
        {
            get { return _health; }
            protected set { _health = value; }
        }

        public int MaxHealth
        {
            get { return _maxHealth; }
            protected set { _maxHealth = value; }
        }

        public int AttackPower
        {
            get { return _attackPower; }
            protected set { _attackPower = value; }
        }

        /// <summary>
        /// Creates a new creature with the given parameters
        /// </summary>
        /// <param name="name">The creature's name</param>
        /// <param name="health">The creature's initial health</param>
        /// <param name="attackPower">The creature's attack power</param>
        public Creature(string name, int health, int attackPower)
        {
            _name = name;
            _health = health;
            _maxHealth = health;
            _attackPower = attackPower;
        }

        /// <summary>
        /// Reduces the creature's health by the specified amount
        /// </summary>
        /// <param name="damage">Amount of damage to take</param>
        public virtual void TakeDamage(int damage)
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
        /// Heals the creature by the specified amount
        /// </summary>
        /// <param name="amount">Amount of health to restore</param>
        public virtual void Heal(int amount)
        {
            if (amount > 0 && IsAlive())
            {
                _health += amount;
                // Ensure health doesn't exceed max health
                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
            }
        }

        /// <summary>
        /// Performs an attack and returns the damage dealt
        /// </summary>
        /// <returns>The amount of damage this creature deals</returns>
        public virtual int Attack()
        {
            // Basic attack just returns the attack power
            // Can be overridden by subclasses for more complex behavior
            return _attackPower;
        }

        /// <summary>
        /// Checks if the creature is still alive
        /// </summary>
        /// <returns>True if the creature has health remaining, false otherwise</returns>
        public bool IsAlive()
        {
            return _health > 0;
        }

        /// <summary>
        /// Gets the current health of the creature
        /// </summary>
        /// <returns>The current health value</returns>
        public int GetHealth()
        {
            return _health;
        }

        /// <summary>
        /// Gets a description of the creature
        /// </summary>
        /// <returns>A string describing the creature</returns>
        public abstract string GetDescription();
    }
}