using System;
using DGD208_Spring2025_OmerAliTaylan.Enums;

namespace DGD208_Spring2025_OmerAliTaylan.Models
{
    public class TrainingActivity
    {
        public string Name { get; }
        public string Description { get; }
        public PetType[] CompatiblePetTypes { get; }
        public PetStat[] AffectedStats { get; }
        public int[] StatChanges { get; }
        public int Duration { get; } // in seconds
        public string[] SuccessMessages { get; }
        public string[] FailureMessages { get; }

        public TrainingActivity(
            string name,
            string description,
            PetType[] compatiblePetTypes,
            PetStat[] affectedStats,
            int[] statChanges,
            int duration,
            string[] successMessages,
            string[] failureMessages)
        {
            Name = name;
            Description = description;
            CompatiblePetTypes = compatiblePetTypes;
            AffectedStats = affectedStats;
            StatChanges = statChanges;
            Duration = duration;
            SuccessMessages = successMessages;
            FailureMessages = failureMessages;
        }

        public bool IsCompatibleWith(PetType petType)
        {
            return Array.Exists(CompatiblePetTypes, type => type == petType);
        }

        public string GetRandomSuccessMessage()
        {
            var random = new Random();
            return SuccessMessages[random.Next(SuccessMessages.Length)];
        }

        public string GetRandomFailureMessage()
        {
            var random = new Random();
            return FailureMessages[random.Next(FailureMessages.Length)];
        }
    }
} 