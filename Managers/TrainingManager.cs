using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGD208_Spring2025_OmerAliTaylan.Models;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.Managers
{
    public class TrainingManager
    {
        private readonly List<TrainingActivity> _activities;
        private readonly Dictionary<Pet, int> _trainingProgress;

        public TrainingManager()
        {
            _activities = new List<TrainingActivity>();
            _trainingProgress = new Dictionary<Pet, int>();
            InitializeActivities();
        }

        private void InitializeActivities()
        {
            // Dog activities
            _activities.Add(new TrainingActivity(
                "Basic Obedience",
                "Teach your dog basic commands like sit and stay",
                new[] { PetType.Dog },
                new[] { PetStat.Fun, PetStat.Hunger },
                new[] { 15, -5 },
                5,
                new[] { "Your dog learned to sit!", "Your dog mastered the stay command!" },
                new[] { "Your dog got distracted...", "Your dog needs more practice." }
            ));

            // Cat activities
            _activities.Add(new TrainingActivity(
                "Agility Training",
                "Train your cat to navigate through obstacles",
                new[] { PetType.Cat },
                new[] { PetStat.Fun, PetStat.Sleep },
                new[] { 20, -10 },
                4,
                new[] { "Your cat gracefully completed the course!", "Your cat showed amazing agility!" },
                new[] { "Your cat got tired and took a nap...", "Your cat wasn't in the mood for training." }
            ));

            // Bird activities
            _activities.Add(new TrainingActivity(
                "Vocal Training",
                "Teach your bird new sounds and words",
                new[] { PetType.Bird },
                new[] { PetStat.Fun, PetStat.Hunger },
                new[] { 15, -5 },
                3,
                new[] { "Your bird learned a new sound!", "Your bird is getting better at mimicking!" },
                new[] { "Your bird seems shy today...", "Your bird needs more practice." }
            ));

            // Fish activities
            _activities.Add(new TrainingActivity(
                "Swimming Patterns",
                "Train your fish to follow specific swimming patterns",
                new[] { PetType.Fish },
                new[] { PetStat.Fun, PetStat.Sleep },
                new[] { 10, -5 },
                2,
                new[] { "Your fish mastered the pattern!", "Your fish is swimming beautifully!" },
                new[] { "Your fish seems tired...", "Your fish needs more practice." }
            ));

            // Hamster activities
            _activities.Add(new TrainingActivity(
                "Maze Training",
                "Train your hamster to navigate through a maze",
                new[] { PetType.Hamster },
                new[] { PetStat.Fun, PetStat.Hunger },
                new[] { 15, -8 },
                4,
                new[] { "Your hamster found the way out!", "Your hamster is getting faster!" },
                new[] { "Your hamster got lost...", "Your hamster needs more practice." }
            ));
        }

        public IEnumerable<TrainingActivity> GetAvailableActivities(Pet pet)
        {
            return _activities.Where(a => a.IsCompatibleWith(pet.Type));
        }

        public async Task<bool> TrainPetAsync(Pet pet, TrainingActivity activity)
        {
            if (pet == null || activity == null || pet.IsDead)
                return false;

            if (!activity.IsCompatibleWith(pet.Type))
                return false;

            // Calculate success chance based on pet's personality and current stats
            var successChance = CalculateSuccessChance(pet, activity);
            var random = new Random();
            var isSuccess = random.NextDouble() < successChance;

            // Show training progress
            Console.WriteLine($"\nTraining {pet.Name} in {activity.Name}...");
            for (int i = 0; i < activity.Duration; i++)
            {
                Console.Write(".");
                await Task.Delay(1000);
            }
            Console.WriteLine();

            // Apply results
            if (isSuccess)
            {
                Console.WriteLine(activity.GetRandomSuccessMessage());
                for (int i = 0; i < activity.AffectedStats.Length; i++)
                {
                    pet.UpdateStat(activity.AffectedStats[i], activity.StatChanges[i]);
                }
                UpdateTrainingProgress(pet);
            }
            else
            {
                Console.WriteLine(activity.GetRandomFailureMessage());
                // Still apply stat changes but with reduced effect
                for (int i = 0; i < activity.AffectedStats.Length; i++)
                {
                    pet.UpdateStat(activity.AffectedStats[i], activity.StatChanges[i] / 2);
                }
            }

            return isSuccess;
        }

        private double CalculateSuccessChance(Pet pet, TrainingActivity activity)
        {
            double baseChance = 0.5; // 50% base chance

            // Adjust based on pet's personality
            switch (pet.Personality.ToLower())
            {
                case "playful":
                case "energetic":
                case "adventurous":
                    baseChance += 0.2;
                    break;
                case "lazy":
                case "timid":
                case "shy":
                    baseChance -= 0.1;
                    break;
            }

            // Adjust based on current stats
            var avgStat = (pet.GetStat(PetStat.Hunger) + pet.GetStat(PetStat.Sleep) + pet.GetStat(PetStat.Fun)) / 300.0;
            baseChance += (avgStat - 0.5) * 0.2; // Up to ±10% based on average stats

            // Adjust based on training progress
            if (_trainingProgress.TryGetValue(pet, out int progress))
            {
                baseChance += (progress / 100.0) * 0.1; // Up to 10% bonus based on progress
            }

            return Math.Clamp(baseChance, 0.1, 0.9); // Keep between 10% and 90%
        }

        private void UpdateTrainingProgress(Pet pet)
        {
            if (!_trainingProgress.ContainsKey(pet))
            {
                _trainingProgress[pet] = 0;
            }
            _trainingProgress[pet] = Math.Min(_trainingProgress[pet] + 5, 100);
        }

        public int GetTrainingProgress(Pet pet)
        {
            return _trainingProgress.TryGetValue(pet, out int progress) ? progress : 0;
        }
    }
} 