using System;
using System.Threading.Tasks;

namespace DGD208_Spring2025_OmerAliTaylan
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var game = new Game();
                await game.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
} 