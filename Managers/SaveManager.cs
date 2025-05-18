using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using DGD208_Spring2025_OmerAliTaylan.Models;

namespace DGD208_Spring2025_OmerAliTaylan.Managers
{
    public class SaveManager
    {
        private readonly string _savePath;
        private readonly PetManager _petManager;

        public SaveManager(PetManager petManager)
        {
            _petManager = petManager;
            _savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "savedata.json");
        }

        public void SaveGame()
        {
            var saveData = new Dictionary<string, object>();
            var petList = new List<Dictionary<string, object>>();

            foreach (var pet in _petManager.GetAllPets())
            {
                petList.Add(pet.ToSaveData());
            }

            saveData["Pets"] = petList;
            
            string jsonString = JsonSerializer.Serialize(saveData);
            File.WriteAllText(_savePath, jsonString);
        }

        public void LoadGame()
        {
            if (!File.Exists(_savePath))
            {
                return;
            }

            try
            {
                string jsonString = File.ReadAllText(_savePath);
                var saveData = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonString);

                if (saveData.ContainsKey("Pets"))
                {
                    var petList = (JsonElement)saveData["Pets"];
                    foreach (var petData in petList.EnumerateArray())
                    {
                        var petDict = JsonSerializer.Deserialize<Dictionary<string, object>>(petData.ToString());
                        var pet = Pet.FromSaveData(petDict);
                        _petManager.AddExistingPet(pet);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading save data: {ex.Message}");
            }
        }
    }
} 