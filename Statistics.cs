using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Tracks and manages player statistics throughout the game
    /// </summary>
    public class Statistics
    {
        // Basic statistics
        private int _monstersDefeated;
        private int _roomsExplored;
        private int _itemsCollected;
        private int _totalDamageDealt;
        private int _totalDamageTaken;
        private int _potionsUsed;
        private int _timesPlayerDied;
        private DateTime _gameStartTime;
        private TimeSpan _totalPlayTime;
        private bool _isTrackingTime;

        // Advanced statistics
        private Dictionary<string, int> _monsterTypeDefeated;
        private Dictionary<string, int> _itemTypeCollected;
        private int _criticalHits;
        private int _highestDamageDealt;

        /// <summary>
        /// Creates a new Statistics tracker
        /// </summary>
        public Statistics()
        {
            Reset();
        }

        /// <summary>
        /// Resets all statistics to their default values
        /// </summary>
        public void Reset()
        {
            _monstersDefeated = 0;
            _roomsExplored = 0;
            _itemsCollected = 0;
            _totalDamageDealt = 0;
            _totalDamageTaken = 0;
            _potionsUsed = 0;
            _timesPlayerDied = 0;
            _gameStartTime = DateTime.Now;
            _totalPlayTime = TimeSpan.Zero;
            _isTrackingTime = false;
            _monsterTypeDefeated = new Dictionary<string, int>();
            _itemTypeCollected = new Dictionary<string, int>();
            _criticalHits = 0;
            _highestDamageDealt = 0;
        }

        /// <summary>
        /// Starts tracking play time
        /// </summary>
        public void StartTimeTracking()
        {
            if (!_isTrackingTime)
            {
                _gameStartTime = DateTime.Now;
                _isTrackingTime = true;
            }
        }

        /// <summary>
        /// Stops tracking play time and adds the elapsed time to total play time
        /// </summary>
        public void StopTimeTracking()
        {
            if (_isTrackingTime)
            {
                _totalPlayTime += DateTime.Now - _gameStartTime;
                _isTrackingTime = false;
            }
        }

        /// <summary>
        /// Records a monster defeat
        /// </summary>
        /// <param name="monsterType">Type of monster defeated</param>
        public void RecordMonsterDefeated(string monsterType)
        {
            _monstersDefeated++;
            
            if (_monsterTypeDefeated.ContainsKey(monsterType))
            {
                _monsterTypeDefeated[monsterType]++;
            }
            else
            {
                _monsterTypeDefeated[monsterType] = 1;
            }
        }

        /// <summary>
        /// Records a room being explored
        /// </summary>
        public void RecordRoomExplored()
        {
            _roomsExplored++;
        }

        /// <summary>
        /// Records an item being collected
        /// </summary>
        /// <param name="itemType">Type of item collected</param>
        public void RecordItemCollected(string itemType)
        {
            _itemsCollected++;
            
            if (_itemTypeCollected.ContainsKey(itemType))
            {
                _itemTypeCollected[itemType]++;
            }
            else
            {
                _itemTypeCollected[itemType] = 1;
            }
        }

        /// <summary>
        /// Records damage dealt by the player
        /// </summary>
        /// <param name="damage">Amount of damage dealt</param>
        /// <param name="isCritical">Whether the hit was a critical hit</param>
        public void RecordDamageDealt(int damage, bool isCritical = false)
        {
            _totalDamageDealt += damage;
            
            if (damage > _highestDamageDealt)
            {
                _highestDamageDealt = damage;
            }
            
            if (isCritical)
            {
                _criticalHits++;
            }
        }

        /// <summary>
        /// Records damage taken by the player
        /// </summary>
        /// <param name="damage">Amount of damage taken</param>
        public void RecordDamageTaken(int damage)
        {
            _totalDamageTaken += damage;
        }

        /// <summary>
        /// Records a potion being used
        /// </summary>
        public void RecordPotionUsed()
        {
            _potionsUsed++;
        }

        /// <summary>
        /// Records a player death
        /// </summary>
        public void RecordPlayerDeath()
        {
            _timesPlayerDied++;
        }

        /// <summary>
        /// Displays all statistics to the console
        /// </summary>
        public void DisplayStatistics()
        {
            // Update total play time if currently tracking
            if (_isTrackingTime)
            {
                TimeSpan currentSession = DateTime.Now - _gameStartTime;
                TimeSpan totalTime = _totalPlayTime + currentSession;
                FormatStatistics(totalTime);
                return;
            }
            
            FormatStatistics(_totalPlayTime);
        }
        
        /// <summary>
        /// Gets a formatted string of all statistics
        /// </summary>
        /// <returns>A string containing all tracked statistics</returns>
        public string GetStatisticsReport()
        {
            // Update total play time if currently tracking
            if (_isTrackingTime)
            {
                TimeSpan currentSession = DateTime.Now - _gameStartTime;
                TimeSpan totalTime = _totalPlayTime + currentSession;
                return BuildStatisticsReport(totalTime);
            }
            
            return BuildStatisticsReport(_totalPlayTime);
        }

        /// <summary>
        /// Formats and displays the statistics to the console
        /// </summary>
        /// <param name="playTime">The total play time</param>
        private void FormatStatistics(TimeSpan playTime)
        {
            Console.WriteLine("=== PLAYER STATISTICS ===\n");

            // Basic stats
            Console.WriteLine($"Play Time: {playTime.Hours:D2}:{playTime.Minutes:D2}:{playTime.Seconds:D2}");
            Console.WriteLine($"Monsters Defeated: {_monstersDefeated}");
            Console.WriteLine($"Rooms Explored: {_roomsExplored}");
            Console.WriteLine($"Items Collected: {_itemsCollected}");
            Console.WriteLine($"Total Damage Dealt: {_totalDamageDealt}");
            Console.WriteLine($"Total Damage Taken: {_totalDamageTaken}");
            Console.WriteLine($"Potions Used: {_potionsUsed}");
            Console.WriteLine($"Highest Damage Dealt: {_highestDamageDealt}");
            Console.WriteLine($"Critical Hits: {_criticalHits}");
            
            // Monster type breakdown
            Console.WriteLine("\n=== MONSTERS DEFEATED ===");
            foreach (var monster in _monsterTypeDefeated)
            {
                Console.WriteLine($"{monster.Key}: {monster.Value}");
            }
            
            // Item type breakdown
            Console.WriteLine("\n=== ITEMS COLLECTED ===");
            foreach (var item in _itemTypeCollected)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
        
        /// <summary>
        /// Builds a string report of all statistics
        /// </summary>
        /// <param name="playTime">The total play time</param>
        /// <returns>A formatted string of statistics</returns>
        private string BuildStatisticsReport(TimeSpan playTime)
        {
            string report = "=== PLAYER STATISTICS ===\n\n";
            
            // Basic stats
            report += $"Play Time: {playTime.Hours:D2}:{playTime.Minutes:D2}:{playTime.Seconds:D2}\n";
            report += $"Monsters Defeated: {_monstersDefeated}\n";
            report += $"Rooms Explored: {_roomsExplored}\n";
            report += $"Items Collected: {_itemsCollected}\n";
            report += $"Total Damage Dealt: {_totalDamageDealt}\n";
            report += $"Total Damage Taken: {_totalDamageTaken}\n";
            report += $"Potions Used: {_potionsUsed}\n";
            report += $"Highest Damage Dealt: {_highestDamageDealt}\n";
            report += $"Critical Hits: {_criticalHits}\n";
            
            // Monster type breakdown
            report += "\n=== MONSTERS DEFEATED ===\n";
            foreach (var monster in _monsterTypeDefeated)
            {
                report += $"{monster.Key}: {monster.Value}\n";
            }
            
            // Item type breakdown
            report += "\n=== ITEMS COLLECTED ===\n";
            foreach (var item in _itemTypeCollected)
            {
                report += $"{item.Key}: {item.Value}\n";
            }
            
            return report;
        }
    }
}
