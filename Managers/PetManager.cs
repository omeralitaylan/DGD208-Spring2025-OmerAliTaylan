using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGD208_Spring2025_OmerAliTaylan.Models;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.Managers
{
    public class PetManager
    {
        private readonly List<Pet> _pets;
        private readonly Dictionary<ItemType, List<Item>> _items;

        public PetManager()
        {
            _pets = new List<Pet>();
            _items = new Dictionary<ItemType, List<Item>>
            {
                { ItemType.Food, new List<Item> 
                    {
                        new Item("Basic Food", ItemType.Food, PetStat.Hunger, 30, 2),
                        new Item("Premium Food", ItemType.Food, PetStat.Hunger, 50, 3)
                    }
                },
                { ItemType.Toy, new List<Item>
                    {
                        new Item("Simple Toy", ItemType.Toy, PetStat.Fun, 30, 2),
                        new Item("Interactive Toy", ItemType.Toy, PetStat.Fun, 50, 3)
                    }
                },
                { ItemType.Bed, new List<Item>
                    {
                        new Item("Basic Bed", ItemType.Bed, PetStat.Sleep, 30, 2),
                        new Item("Luxury Bed", ItemType.Bed, PetStat.Sleep, 50, 3)
                    }
                }
            };

            // Subscribe to pet death events
            foreach (var pet in _pets)
            {
                pet.PetDied += OnPetDied;
            }
        }

        private void OnPetDied(object sender, EventArgs e)
        {
            if (sender is Pet deadPet)
            {
                Console.WriteLine($"\n{deadPet.Name} has passed away...");
                _pets.Remove(deadPet);
            }
        }

        public Pet AdoptPet(string name, PetType type)
        {
            var pet = new Pet(name, type);
            pet.PetDied += OnPetDied;
            _pets.Add(pet);
            return pet;
        }

        public void AddExistingPet(Pet pet)
        {
            if (pet != null && !_pets.Any(p => p.Name.Equals(pet.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _pets.Add(pet);
            }
        }

        public IEnumerable<Pet> GetAllPets()
        {
            return _pets.Where(p => !p.IsDead).AsReadOnly();
        }

        public Pet GetPet(string name)
        {
            return _pets.FirstOrDefault(p => 
                p.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && !p.IsDead);
        }

        public IEnumerable<Item> GetAvailableItems(ItemType type)
        {
            return _items[type];
        }

        public async Task UseItemAsync(Item item, Pet pet)
        {
            if (item != null && pet != null && !pet.IsDead)
            {
                await item.UseOnAsync(pet);
            }
        }

        public void UpdateAllPetStats()
        {
            foreach (var pet in _pets.ToList())
            {
                if (!pet.IsDead)
                {
                    pet.DecreaseStats();
                }
            }
        }
    }
} 