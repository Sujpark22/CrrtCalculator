using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRRT_Calculator.ViewModels
{
    public partial class AquaCalculatorInput : ObservableObject
    {
        [ObservableProperty]
        private string? _Mrn, _Clear, _Cand, _Mod, _Ecmo, _Weight, _Warning, _Height, _Fluid, _Citrate, _LivDys, _Heparin, _hepBol, _hepGTT, _Epop;

        [ObservableProperty]
        private DateTime _dob;

        [ObservableProperty]
        private bool _isHepBolLabelVisible, _isHepBolusEntryVisible, _isHepGTTLabelVisible, _isHepGTTEntryVisible;

        public event Action<string, string> NavigateToHeparinDose;

        private async Task EvaluateWeight(double weight)
        {
            if (weight > 12)
            {
                Warning = "*'Modified' Aquapheresis is not indicated, use CRRT for clearance and aquapheresis for fluid removal only";
            }
            else
            {
                Warning = string.Empty;
            }
        }

        public async void OnWeightEntryUnfocused()
        {
            if (double.TryParse(Weight, out double weight))
            {
                await EvaluateWeight(weight);
            }
            else
            {
                Warning = string.Empty;
            }
            UpdateHeparinFields();
        }

        partial void OnHeparinChanged(string? value)
        {
            UpdateHeparinFields();
        }

        public void UpdateHeparinFields()
        {
            if (Heparin == null || Heparin == "No" || string.IsNullOrEmpty(Weight) || !double.TryParse(Weight, out double weightD))
            {
                IsHepBolLabelVisible = false;
                IsHepBolusEntryVisible = false;
                IsHepGTTLabelVisible = false;
                IsHepGTTEntryVisible = false;
                return;
            }

            else
            {
                IsHepBolLabelVisible = true;
                IsHepBolusEntryVisible = true;
                IsHepGTTLabelVisible = true;
                IsHepGTTEntryVisible = true;

                NavigateToHeparinDose?.Invoke(Heparin, Weight);
            }
        }

        public void Validate(out List<string> errors)
        {
            errors = [];

            check(Mrn, "MRN", errors);
            //check(Clear, "Rapid clearance", errors);
            //check(Cand, "Candidate for aquapheresis", errors);
            check(Mod, "'Modified' aquapheresis with CVVH?", errors);
            //check(Ecmo, "ECMO", errors);
            checkNumber(Weight, "Weight", errors);
            checkNumber(Height, "Height", errors);
            check(Fluid, "Aquapheresis", errors);
            check(Epop, "Epoprostenol", errors);
            check(Heparin, "Heparin", errors);

            if (Heparin == "Yes")
            {
                checkNumber(HepBol, "Heparin Bolus", errors);
                checkNumber(HepGTT, "Heparin Drip", errors);
            }

            static void check(string? value, string caption, ICollection<string> errors)
            {
                if (string.IsNullOrWhiteSpace(value))
                    errors.Add($"{caption} is empty");
            }

            static void checkNumber(string? number, string caption, ICollection<string> errors)
            {
                if (!double.TryParse(number, out _))
                    errors.Add($"{caption} is invalid number");
            }
        }
    }
}
