using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a room in the dungeon with a description, items, and monsters
    /// </summary>
    public class Room
    {
        private string _description;
        private List<Item> _items;
        private List<Monster> _monsters;
        private bool _isLocked;
        private string _unlockCode;

        /// <summary>
        /// Creates a new room with the given description and optional parameters
        /// </summary>
        /// <param name="description">The description of the room</param>
        /// <param name="isLocked">Whether the room is locked</param>
        /// <param name="unlockCode">The code required to unlock the room</param>
        public Room(string description, bool isLocked = false, string unlockCode = null)
        {
            _description = description;
            _items = new List<Item>();
            _monsters = new List<Monster>();
            _isLocked = isLocked;
            _unlockCode = unlockCode;
        }

        /// <summary>
        /// Returns the description of the room
        /// </summary>
        /// <returns>The room description</returns>
        public string GetDescription()
        {
            return _description;
        }

        /// <summary>
        /// Adds an item to the room
        /// </summary>
        /// <param name="item">The item to add</param>
        public void AddItem(Item item)
        {
            if (item != null)
            {
                _items.Add(item);
            }
        }

        /// <summary>
        /// Legacy method to support old code
        /// Adds an item to the room by name
        /// </summary>
        /// <param name="itemName">The name of the item</param>
        public void AddItem(string itemName)
        {
            if (!string.IsNullOrEmpty(itemName))
            {
                Item item = new Item(itemName, $"A {itemName}.");
                _items.Add(item);
            }
        }

        /// <summary>
        /// Adds a monster to the room
        /// </summary>
        /// <param name="monster">The monster to add</param>
        public void AddMonster(Monster monster)
        {
            if (monster != null)
            {
                _monsters.Add(monster);
            }
        }

        /// <summary>
        /// Checks if the room contains any items
        /// </summary>
        /// <returns>True if the room has any items, false otherwise</returns>
        public bool HasItems()
        {
            return _items.Count > 0;
        }

        /// <summary>
        /// Gets a list of all items in the room
        /// </summary>
        /// <returns>A list of items</returns>
        public List<Item> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// Legacy method to support old code
        /// Checks if the room contains an item
        /// </summary>
        /// <returns>True if the room has an item, false otherwise</returns>
        public bool HasItem()
        {
            return HasItems();
        }

        /// <summary>
        /// Legacy method to support old code
        /// Gets the name of the first item in the room without removing it
        /// </summary>
        /// <returns>The name of the item, or null if there is no item</returns>
        public string GetItemName()
        {
            if (HasItems())
            {
                return _items[0].Name;
            }
            return null;
        }

        /// <summary>
        /// Legacy method to support old code
        /// Gets the first item in the room and removes it
        /// </summary>
        /// <returns>The name of the item that was in the room, or null if there was no item</returns>
        public string TakeItem()
        {
            if (HasItems())
            {
                string itemName = _items[0].Name;
                _items.RemoveAt(0);
                return itemName;
            }
            return null;
        }

        /// <summary>
        /// Gets an item from the room by name and removes it
        /// </summary>
        /// <param name="itemName">The name of the item to take</param>
        /// <returns>The item if found, null otherwise</returns>
        public Item TakeItem(string itemName)
        {
            try
            {
                // Using LINQ to find the item by name (case-insensitive)
                Item item = _items.FirstOrDefault(i => 
                    string.Equals(i.Name, itemName, StringComparison.OrdinalIgnoreCase));
                
                if (item != null)
                {
                    _items.Remove(item);
                    return item;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Checks if the room contains any monsters
        /// </summary>
        /// <returns>True if the room has any monsters, false otherwise</returns>
        public bool HasMonsters()
        {
            return _monsters.Count > 0;
        }

        /// <summary>
        /// Gets a list of all monsters in the room
        /// </summary>
        /// <returns>A list of monsters</returns>
        public List<Monster> GetMonsters()
        {
            return _monsters;
        }

        /// <summary>
        /// Removes a monster from the room
        /// </summary>
        /// <param name="monster">The monster to remove</param>
        /// <returns>True if the monster was removed, false otherwise</returns>
        public bool RemoveMonster(Monster monster)
        {
            if (monster != null)
            {
                return _monsters.Remove(monster);
            }
            return false;
        }

        /// <summary>
        /// Checks if the room is locked
        /// </summary>
        /// <returns>True if the room is locked, false otherwise</returns>
        public bool IsLocked()
        {
            return _isLocked;
        }

        /// <summary>
        /// Tries to unlock the room with a key
        /// </summary>
        /// <param name="key">The key to use</param>
        /// <returns>True if the room was unlocked, false otherwise</returns>
        public bool Unlock(Key key)
        {
            // For debugging
            // Console.WriteLine($"Attempting to unlock with key code '{key.UnlockCode}', room requires code '{_unlockCode}'");
            
            if (_isLocked && key != null && key.UnlockCode == _unlockCode)
            {
                _isLocked = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the strongest monster in the room
        /// </summary>
        /// <returns>The monster with the highest attack power, or null if there are no monsters</returns>
        public Monster GetStrongestMonster()
        {
            // Using LINQ to find the monster with the highest attack power
            return _monsters.OrderByDescending(m => m.AttackPower).FirstOrDefault();
        }

        /// <summary>
        /// Gets a string with descriptions of all monsters in the room
        /// </summary>
        /// <returns>A string describing all monsters in the room</returns>
        public string GetMonstersDescription()
        {
            if (!HasMonsters())
            {
                return "There are no monsters in this room.";
            }

            // Using LINQ and string interpolation to create numbered descriptions
            string monsterList = "";
            for (int i = 0; i < _monsters.Count; i++)
            {
                monsterList += $"{i+1}. {_monsters[i].GetDescription()}\n";
            }
            return monsterList.TrimEnd();
        }

        /// <summary>
        /// Gets a string with descriptions of all items in the room
        /// </summary>
        /// <returns>A string describing all items in the room</returns>
        public string GetItemsDescription()
        {
            if (!HasItems())
            {
                return "There are no items in this room.";
            }

            // Using LINQ and string interpolation to create numbered descriptions
            string itemList = "Items in this room:\n";
            for (int i = 0; i < _items.Count; i++)
            {
                itemList += $"{i+1}. {_items[i].GetDetails()}\n";
            }
            return itemList.TrimEnd();
        }
    }
}