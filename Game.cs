using System;
using System.Threading.Tasks;
using DGD208_Spring2025_OmerAliTaylan.UI;

namespace DGD208_Spring2025_OmerAliTaylan
{
    public class Game
    {
        private bool _isRunning;
        private readonly Menu _mainMenu;

        public Game()
        {
            _mainMenu = new Menu("Pet Simulator Main Menu");
            InitializeMainMenu();
        }

        private void InitializeMainMenu()
        {
            _mainMenu.AddMenuItem(1, "Display Creator Info");
            _mainMenu.AddMenuItem(2, "Adopt a Pet");
            _mainMenu.AddMenuItem(3, "View Pet Stats");
            _mainMenu.AddMenuItem(4, "Use Item");
            _mainMenu.AddMenuItem(0, "Exit Game");
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
                    Console.WriteLine("Adopt a Pet feature coming soon...");
                    break;
                case 3:
                    Console.WriteLine("View Pet Stats feature coming soon...");
                    break;
                case 4:
                    Console.WriteLine("Use Item feature coming soon...");
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
            await Task.CompletedTask;
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
    }
} 