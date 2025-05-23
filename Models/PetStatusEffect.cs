using System;
using DGD208_Spring2025_OmerAliTaylan.Enums;
using DGD208_Spring2025_OmerAliTaylan.UI;

namespace DGD208_Spring2025_OmerAliTaylan.Models
{
    public class PetStatusEffect
    {
        public event EventHandler<PetStatusEventArgs> StatusChanged;

        private PetStatus _currentStatus;
        private readonly Pet _pet;
        private int _statusDuration;

        public PetStatus CurrentStatus => _currentStatus;
        public int StatusDuration => _statusDuration;

        public PetStatusEffect(Pet pet)
        {
            _pet = pet;
            _currentStatus = PetStatus.Normal;
            _statusDuration = 0;
            _pet.StatChanged += OnPetStatChanged;
        }

        private void OnPetStatChanged(object sender, PetStatEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var hunger = _pet.GetStat(PetStat.Hunger);
            var sleep = _pet.GetStat(PetStat.Sleep);
            var fun = _pet.GetStat(PetStat.Fun);

            var oldStatus = _currentStatus;
            _currentStatus = DetermineStatus(hunger, sleep, fun);

            if (oldStatus != _currentStatus)
            {
                _statusDuration = 0;
                OnStatusChanged(new PetStatusEventArgs(oldStatus, _currentStatus));
                ApplyStatusEffects();
            }
            else
            {
                _statusDuration++;
            }
        }

        private void ApplyStatusEffects()
        {
            switch (_currentStatus)
            {
                case PetStatus.Happy:
                    // Happy pets recover stats slightly faster
                    _pet.UpdateStat(PetStat.Hunger, 1);
                    _pet.UpdateStat(PetStat.Sleep, 1);
                    _pet.UpdateStat(PetStat.Fun, 1);
                    break;
                case PetStatus.Sick:
                    // Sick pets lose stats faster
                    _pet.UpdateStat(PetStat.Hunger, -2);
                    _pet.UpdateStat(PetStat.Sleep, -2);
                    _pet.UpdateStat(PetStat.Fun, -2);
                    break;
                case PetStatus.Excited:
                    // Excited pets use more energy
                    _pet.UpdateStat(PetStat.Hunger, -1);
                    _pet.UpdateStat(PetStat.Sleep, -1);
                    break;
            }
        }

        private PetStatus DetermineStatus(int hunger, int sleep, int fun)
        {
            if (hunger < 20) return PetStatus.Hungry;
            if (sleep < 20) return PetStatus.Tired;
            if (fun < 20) return PetStatus.Bored;
            if (hunger < 40 || sleep < 40 || fun < 40) return PetStatus.Sick;
            if (hunger > 80 && sleep > 80 && fun > 80) return PetStatus.Happy;
            if (fun > 80) return PetStatus.Excited;
            return PetStatus.Normal;
        }

        protected virtual void OnStatusChanged(PetStatusEventArgs e)
        {
            StatusDisplay.DisplayStatus(_pet.Name, _currentStatus, 
                _pet.GetStat(PetStat.Hunger),
                _pet.GetStat(PetStat.Sleep),
                _pet.GetStat(PetStat.Fun));
            
            Console.WriteLine(StatusDisplay.GetStatusMessage(_currentStatus, _pet.Name));
            StatusChanged?.Invoke(this, e);
        }
    }

    public class PetStatusEventArgs : EventArgs
    {
        public PetStatus OldStatus { get; }
        public PetStatus NewStatus { get; }

        public PetStatusEventArgs(PetStatus oldStatus, PetStatus newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
} 