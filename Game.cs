using System;
using System.Threading.Tasks;
using System.Linq;
using DGD208_Spring2025_OmerAliTaylan.UI;
using DGD208_Spring2025_OmerAliTaylan.Models;
using DGD208_Spring2025_OmerAliTaylan.Managers;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan
{
    public class Game
    {
        private bool _isRunning;
        private readonly Menu _mainMenu;
        private readonly PetManager _petManager;
        private readonly Menu _adoptionMenu;
        private readonly Menu _itemMenu;
        private readonly TrainingManager _trainingManager;
        private readonly Menu _trainingMenu;

        public Game()
        {
            _mainMenu = new Menu("Pet Simulator Main Menu");
            _petManager = new PetManager();
            _adoptionMenu = new Menu("Pet Adoption Menu");
            _itemMenu = new Menu("Item Selection Menu");
            _trainingManager = new TrainingManager();
            _trainingMenu = new Menu("Training Menu");
            InitializeMenus();
        }

        private void InitializeMenus()
        {
            _mainMenu.AddMenuItem(1, "Display Creator Info");
            _mainMenu.AddMenuItem(2, "Adopt a Pet");
            _mainMenu.AddMenuItem(3, "View Pet Stats");
            _mainMenu.AddMenuItem(4, "Use Item");
            _mainMenu.AddMenuItem(5, "Train Pet");
            _mainMenu.AddMenuItem(0, "Exit Game");

            var petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>();
            int index = 1;
            foreach (var type in petTypes)
            {
                _adoptionMenu.AddMenuItem(index++, $"Adopt a {type}");
            }
            _adoptionMenu.AddMenuItem(0, "Back to Main Menu");

            index = 1;
            foreach (ItemType type in Enum.GetValues(typeof(ItemType)))
            {
                _itemMenu.AddMenuItem(index++, $"Use {type}");
            }
            _itemMenu.AddMenuItem(0, "Back to Main Menu");
        }

        public async Task RunAsync()
        {
            _isRunning = true;
            Console.WriteLine("Welcome to Pet Simulator!");

            while (_isRunning)
            {
                _mainMenu.Display();

                if (_mainMenu.TryGetMenuOption(out int choice))
                {
                    await HandleMenuChoice(choice);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }

                await Task.Delay(3000); // Wait 3 seconds between actions
                _petManager.UpdateAllPetStats(); // Update pet stats each turn
            }
        }

        private async Task HandleMenuChoice(int choice)
        {
            switch (choice)
            {
                case 0:
                    _isRunning = false;
                    Console.WriteLine("Thank you for playing! Goodbye!");
                    break;
                case 1:
                    DisplayCreatorInfo();
                    break;
                case 2:
                    await HandlePetAdoption();
                    break;
                case 3:
                    DisplayPetStats();
                    break;
                case 4:
                    await HandleItemUsage();
                    break;
                case 5:
                    await HandlePetTraining();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private async Task HandlePetAdoption()
        {
            _adoptionMenu.Display();
            if (_adoptionMenu.TryGetMenuOption(out int choice) && choice != 0)
            {
                Console.Write("Enter a name for your pet: ");
                string name = Console.ReadLine()?.Trim();
                
                if (!string.IsNullOrEmpty(name))
                {
                    var petType = (PetType)(choice - 1);
                    var pet = _petManager.AdoptPet(name, petType);
                    Console.WriteLine($"Congratulations! You've adopted a {petType} named {name}!");
                }
                else
                {
                    Console.WriteLine("Invalid name. Adoption cancelled.");
                }
            }
            await Task.CompletedTask;
        }

        private void DisplayPetStats()
        {
            var pets = _petManager.GetAllPets().ToList();
            if (pets.Any())
            {
                Console.WriteLine("\nYour Pets:");
                foreach (var pet in pets)
                {
                    Console.WriteLine($"\n{pet.Name} the {pet.Type}:");
                    Console.WriteLine($"Hunger: {pet.GetStat(PetStat.Hunger)}");
                    Console.WriteLine($"Sleep: {pet.GetStat(PetStat.Sleep)}");
                    Console.WriteLine($"Fun: {pet.GetStat(PetStat.Fun)}");
                }
            }
            else
            {
                Console.WriteLine("\nYou don't have any pets yet. Try adopting one!");
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task HandleItemUsage()
        {
            var pets = _petManager.GetAllPets().ToList();
            if (!pets.Any())
            {
                Console.WriteLine("You need to adopt a pet first!");
                return;
            }

            _itemMenu.Display();
            if (_itemMenu.TryGetMenuOption(out int choice) && choice != 0)
            {
                var itemType = (ItemType)(choice - 1);
                var items = _petManager.GetAvailableItems(itemType).ToList();

                Console.WriteLine("\nAvailable Items:");
                for (int i = 0; i < items.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {items[i]}");
                }

                Console.Write("\nSelect an item number: ");
                if (int.TryParse(Console.ReadLine(), out int itemChoice) && 
                    itemChoice > 0 && itemChoice <= items.Count)
                {
                    var selectedItem = items[itemChoice - 1];

                    Console.WriteLine("\nSelect a pet:");
                    for (int i = 0; i < pets.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {pets[i].Name} the {pets[i].Type}");
                    }

                    Console.Write("\nSelect a pet number: ");
                    if (int.TryParse(Console.ReadLine(), out int petChoice) && 
                        petChoice > 0 && petChoice <= pets.Count)
                    {
                        var selectedPet = pets[petChoice - 1];
                        await _petManager.UseItemAsync(selectedItem, selectedPet);
                    }
                }
            }
        }

        private void DisplayCreatorInfo()
        {
            Console.WriteLine("\nCreator Information");
            Console.WriteLine("-----------------");
            Console.WriteLine("Name: Omer Ali Taylan");
            Console.WriteLine("Student Number: [Your Student Number]");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        private async Task HandlePetTraining()
        {
            var pets = _petManager.GetAllPets().ToList();
            if (!pets.Any())
            {
                Console.WriteLine("You need to adopt a pet first!");
                return;
            }

            Console.WriteLine("\nSelect a pet to train:");
            for (int i = 0; i < pets.Count; i++)
            {
                var pet = pets[i];
                var progress = _trainingManager.GetTrainingProgress(pet);
                Console.WriteLine($"{i + 1}. {pet.Name} the {pet.Type} (Training Progress: {progress}%)");
            }

            Console.Write("\nSelect a pet number: ");
            if (int.TryParse(Console.ReadLine(), out int petChoice) && 
                petChoice > 0 && petChoice <= pets.Count)
            {
                var selectedPet = pets[petChoice - 1];
                var activities = _trainingManager.GetAvailableActivities(selectedPet).ToList();

                if (!activities.Any())
                {
                    Console.WriteLine("No training activities available for this pet type.");
                    return;
                }

                Console.WriteLine("\nAvailable Training Activities:");
                for (int i = 0; i < activities.Count; i++)
                {
                    var activity = activities[i];
                    Console.WriteLine($"{i + 1}. {activity.Name} - {activity.Description}");
                    Console.WriteLine($"   Duration: {activity.Duration} seconds");
                    Console.WriteLine($"   Affects: {string.Join(", ", activity.AffectedStats)}");
                }

                Console.Write("\nSelect an activity number: ");
                if (int.TryParse(Console.ReadLine(), out int activityChoice) && 
                    activityChoice > 0 && activityChoice <= activities.Count)
                {
                    var selectedActivity = activities[activityChoice - 1];
                    await _trainingManager.TrainPetAsync(selectedPet, selectedActivity);
                }
            }
        }
    }
} 