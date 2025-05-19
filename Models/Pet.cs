using System;
using System.Collections.Generic;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.Models
{
    public class Pet
    {
        public event EventHandler<PetStatEventArgs> StatChanged;
        public event EventHandler PetDied;

        private readonly Dictionary<PetStat, int> _stats;
        private bool _isDead;
        
        public string Name { get; }
        public PetType Type { get; }
        public string Personality { get; }
        public bool IsDead => _isDead;
        
        public Pet(string name, PetType type)
        {
            Name = name;
            Type = type;
            Personality = GeneratePersonality();
            _isDead = false;
            _stats = new Dictionary<PetStat, int>
            {
                { PetStat.Hunger, 50 },
                { PetStat.Sleep, 50 },
                { PetStat.Fun, 50 }
            };
        }

        private string GeneratePersonality()
        {
            var personalities = new Dictionary<PetType, string[]>
            {
                { PetType.Dog, new[] { "Playful", "Loyal", "Energetic" } },
                { PetType.Cat, new[] { "Independent", "Curious", "Lazy" } },
                { PetType.Bird, new[] { "Chatty", "Social", "Shy" } },
                { PetType.Fish, new[] { "Peaceful", "Active", "Calm" } },
                { PetType.Hamster, new[] { "Adventurous", "Timid", "Friendly" } }
            };

            var random = new Random();
            var possiblePersonalities = personalities[Type];
            return possiblePersonalities[random.Next(possiblePersonalities.Length)];
        }

        public int GetStat(PetStat stat)
        {
            return _stats[stat];
        }

        public void UpdateStat(PetStat stat, int amount)
        {
            if (_isDead) return;

            int oldValue = _stats[stat];
            _stats[stat] = Math.Clamp(_stats[stat] + amount, 0, 100);

            if (oldValue != _stats[stat])
            {
                OnStatChanged(new PetStatEventArgs(stat, oldValue, _stats[stat]));
            }

            CheckDeath();
        }

        private void CheckDeath()
        {
            if (_isDead) return;

            foreach (var stat in _stats)
            {
                if (stat.Value <= 0)
                {
                    _isDead = true;
                    OnPetDied();
                    break;
                }
            }
        }

        public void DecreaseStats()
        {
            if (_isDead) return;

            foreach (var stat in _stats.Keys)
            {
                UpdateStat(stat, -1);
            }
        }

        protected virtual void OnStatChanged(PetStatEventArgs e)
        {
            StatChanged?.Invoke(this, e);
        }

        protected virtual void OnPetDied()
        {
            PetDied?.Invoke(this, EventArgs.Empty);
        }

        public Dictionary<string, object> ToSaveData()
        {
            return new Dictionary<string, object>
            {
                { "Name", Name },
                { "Type", Type.ToString() },
                { "Personality", Personality },
                { "Stats", _stats.ToDictionary(k => k.Key.ToString(), v => v.Value) },
                { "IsDead", _isDead }
            };
        }

        public static Pet FromSaveData(Dictionary<string, object> data)
        {
            var name = (string)data["Name"];
            var type = Enum.Parse<PetType>((string)data["Type"]);
            var pet = new Pet(name, type);
            
            var stats = (Dictionary<string, int>)data["Stats"];
            foreach (var stat in stats)
            {
                var petStat = Enum.Parse<PetStat>(stat.Key);
                pet._stats[petStat] = stat.Value;
            }

            if (data.ContainsKey("IsDead"))
            {
                pet._isDead = (bool)data["IsDead"];
            }
            
            return pet;
        }
    }

    public class PetStatEventArgs : EventArgs
    {
        public PetStat Stat { get; }
        public int OldValue { get; }
        public int NewValue { get; }

        public PetStatEventArgs(PetStat stat, int oldValue, int newValue)
        {
            Stat = stat;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
} 