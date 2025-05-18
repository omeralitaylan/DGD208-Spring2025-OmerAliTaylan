using System;
using System.Threading.Tasks;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.Models
{
    public class Item
    {
        public string Name { get; }
        public ItemType Type { get; }
        public PetStat AffectedStat { get; }
        public int EffectValue { get; }
        public int UsageDurationSeconds { get; }

        public Item(string name, ItemType type, PetStat affectedStat, int effectValue, int usageDurationSeconds = 2)
        {
            Name = name;
            Type = type;
            AffectedStat = affectedStat;
            EffectValue = effectValue;
            UsageDurationSeconds = usageDurationSeconds;
        }

        public async Task UseOnAsync(Pet pet)
        {
            if (pet.IsDead)
            {
                Console.WriteLine($"{pet.Name} has passed away and can't use items anymore.");
                return;
            }

            Console.WriteLine($"Using {Name} on {pet.Name}...");
            await Task.Delay(UsageDurationSeconds * 1000);
            pet.UpdateStat(AffectedStat, EffectValue);
            Console.WriteLine($"Finished using {Name} on {pet.Name}!");
        }

        public override string ToString()
        {
            return $"{Name} ({Type}) - Affects {AffectedStat} by {EffectValue} - Takes {UsageDurationSeconds} seconds";
        }
    }
} 