using System;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.UI
{
    public static class StatusDisplay
    {
        private static readonly Dictionary<PetStatus, (ConsoleColor color, string[] asciiArt)> StatusVisuals = new()
        {
            { PetStatus.Normal, (ConsoleColor.White, new[] {
                "  ^_^  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Happy, (ConsoleColor.Green, new[] {
                "  ^_^  ",
                " (^_^) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Hungry, (ConsoleColor.Yellow, new[] {
                "  >_<  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Tired, (ConsoleColor.Blue, new[] {
                "  -_-  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Bored, (ConsoleColor.Gray, new[] {
                "  -_-  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Excited, (ConsoleColor.Magenta, new[] {
                "  ^_^  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })},
            { PetStatus.Sick, (ConsoleColor.Red, new[] {
                "  >_<  ",
                " (o o) ",
                " ( - ) ",
                "-------"
            })}
        };

        public static void DisplayStatus(string petName, PetStatus status, int hunger, int sleep, int fun)
        {
            var (color, asciiArt) = StatusVisuals[status];
            var originalColor = Console.ForegroundColor;
            
            Console.ForegroundColor = color;
            Console.WriteLine($"\n{petName}'s Status: {status}");
            Console.WriteLine("-------------------");
            
            foreach (var line in asciiArt)
            {
                Console.WriteLine(line);
            }
            
            Console.WriteLine("\nStats:");
            DisplayStatBar("Hunger", hunger);
            DisplayStatBar("Sleep", sleep);
            DisplayStatBar("Fun", fun);
            
            Console.ForegroundColor = originalColor;
            Console.WriteLine("-------------------\n");
        }

        private static void DisplayStatBar(string statName, int value)
        {
            Console.Write($"{statName}: ");
            var barLength = value / 5;
            Console.Write("[");
            Console.Write(new string('█', barLength));
            Console.Write(new string(' ', 20 - barLength));
            Console.WriteLine($"] {value}%");
        }

        public static string GetStatusMessage(PetStatus status, string petName)
        {
            return status switch
            {
                PetStatus.Happy => $"{petName} is feeling great and very happy!",
                PetStatus.Hungry => $"{petName} is getting hungry and needs food!",
                PetStatus.Tired => $"{petName} is feeling tired and needs rest!",
                PetStatus.Bored => $"{petName} is bored and wants to play!",
                PetStatus.Excited => $"{petName} is super excited and full of energy!",
                PetStatus.Sick => $"{petName} is not feeling well and needs attention!",
                _ => $"{petName} is doing okay."
            };
        }
    }
} 