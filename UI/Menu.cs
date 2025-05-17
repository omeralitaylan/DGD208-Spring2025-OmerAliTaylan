using System;
using System.Collections.Generic;

namespace DGD208_Spring2025_OmerAliTaylan.UI
{
    public class Menu
    {
        private readonly Dictionary<int, string> _menuItems;
        private readonly string _title;

        public Menu(string title)
        {
            _title = title;
            _menuItems = new Dictionary<int, string>();
        }

        public void AddMenuItem(int key, string description)
        {
            _menuItems[key] = description;
        }

        public void Display()
        {
            Console.WriteLine($"\n{_title}");
            Console.WriteLine(new string('-', _title.Length));

            foreach (var item in _menuItems)
            {
                Console.WriteLine($"{item.Key}. {item.Value}");
            }
            Console.WriteLine("\nEnter your choice: ");
        }

        public bool TryGetMenuOption(out int choice)
        {
            return int.TryParse(Console.ReadLine(), out choice) && _menuItems.ContainsKey(choice);
        }
    }
} 