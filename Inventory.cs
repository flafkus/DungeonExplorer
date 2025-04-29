using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents an inventory that can store multiple items
    /// </summary>
    public class Inventory
    {
        // List to store the items
        private List<Item> _items;

        /// <summary>
        /// Creates a new empty inventory
        /// </summary>
        public Inventory()
        {
            _items = new List<Item>();
        }

        /// <summary>
        /// Adds an item to the inventory
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
        /// Removes an item from the inventory
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item was removed, false otherwise</returns>
        public bool RemoveItem(Item item)
        {
            if (item != null)
            {
                return _items.Remove(item);
            }
            return false;
        }

        /// <summary>
        /// Removes an item from the inventory by name
        /// </summary>
        /// <param name="itemName">The name of the item to remove</param>
        /// <returns>True if the item was removed, false otherwise</returns>
        public bool RemoveItem(string itemName)
        {
            // Using LINQ to find the first item with matching name (case-insensitive)
            Item itemToRemove = _items.FirstOrDefault(item => string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));
            
            if (itemToRemove != null)
            {
                return _items.Remove(itemToRemove);
            }
            return false;
        }

        /// <summary>
        /// Finds an item in the inventory by name
        /// </summary>
        /// <param name="itemName">The name of the item to find</param>
        /// <returns>The found item, or null if not found</returns>
        public Item FindItem(string itemName)
        {
            // Using LINQ to find the first item with matching name (case-insensitive)
            return _items.FirstOrDefault(item => string.Equals(item.Name, itemName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all items in the inventory
        /// </summary>
        /// <returns>A list of all items</returns>
        public List<Item> GetItems()
        {
            return _items;
        }

        /// <summary>
        /// Gets the count of items in the inventory
        /// </summary>
        /// <returns>The number of items</returns>
        public int GetItemCount()
        {
            return _items.Count;
        }

        /// <summary>
        /// Lists all items in the inventory as a string
        /// </summary>
        /// <returns>A comma-separated list of item names or "No items" if empty</returns>
        public string ListItems()
        {
            if (_items.Count == 0)
            {
                return "No items";
            }

            // Using LINQ to create a list of item names
            return string.Join(", ", _items.Select(item => item.Name));
        }

        /// <summary>
        /// Lists all items in the inventory with their details
        /// </summary>
        /// <returns>A string with detailed information about each item</returns>
        public string ListItemDetails()
        {
            if (_items.Count == 0)
            {
                return "No items";
            }

            // Using LINQ to create a list of item details
            return string.Join("\n", _items.Select(item => item.GetDetails()));
        }

        /// <summary>
        /// Sorts items by name
        /// </summary>
        public void SortItemsByName()
        {
            // Using LINQ's OrderBy to sort items by name
            _items = _items.OrderBy(item => item.Name).ToList();
        }

        /// <summary>
        /// Gets items filtered by type
        /// </summary>
        /// <typeparam name="T">The type of items to filter</typeparam>
        /// <returns>A list of items of the specified type</returns>
        public List<T> GetItemsByType<T>() where T : Item
        {
            // Using LINQ's OfType to filter items by their type
            return _items.OfType<T>().ToList();
        }
    }
}