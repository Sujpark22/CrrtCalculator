using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CRRT_Calculator.ViewModels {

    public partial class NeoCalculatorInput : ObservableObject
    {
        [ObservableProperty]
        private string? _Mrn, _Weight, _Height, _Citrate, _LivDys, _BFR, _minBFR, _maxBFR, _heparin, _hepBol, _hepDrip;

        [ObservableProperty]
        private DateTime _dob;

        [ObservableProperty]
        private bool _isHepBolLabelVisible, _isHepBolusEntryVisible, _isHepDripLabelVisible, _isHepDripEntryVisible;

        public event Action<string, string> NavigateToHeparinDose;


        partial void OnDobChanged(DateTime value)
        {
            CalculateBFR();
        }

        partial void OnHeparinChanged(string? value)
        {
            UpdateHeparinFields();
        }

        partial void OnBFRChanged(string? value) {
            if (double.TryParse(value, out double bloodFlowRate))
            {
                if (bloodFlowRate > 150)
                {
                    BFR = "YOU MAY HAVE EXCEEDED MAX BLOOD FLOW RATE";
                }
            }
        }

        public void CalculateBFR()
        {
            if (double.TryParse(Weight, out double weightValue) && weightValue > 0 && Dob != default)
            {
                DateTime today = DateTime.Today;
                int age = today.Year - Dob.Year;
                if (Dob.Date > today.AddYears(-age)) age--;

                double minBFR = age < 1 ? 8 * weightValue : (age < 6 ? 6 * weightValue : 3 * weightValue);
                double maxBFR = age < 1 ? 10 * weightValue : (age < 6 ? 8 * weightValue : 6 * weightValue);

                MinBFR = "Minimum Blood Flow Rate: " + minBFR.ToString();
                MaxBFR = "Maximum Blood Flow Rate: " + maxBFR.ToString();
            }
            else
            {
                MinBFR = "Minimum Blood Flow Rate: " + string.Empty;
                MaxBFR = "Maximum Blood Flow Rate: " + string.Empty;
            }
        }

        public void UpdateHeparinFields()
        {
            if (Heparin == null || Heparin == "No" || string.IsNullOrEmpty(Weight) || !double.TryParse(Weight, out double weightD))
            {
                IsHepBolLabelVisible = false;
                IsHepBolusEntryVisible = false;
                IsHepDripLabelVisible = false;
                IsHepDripEntryVisible = false;
                return;
            }

            else
            {
                IsHepBolLabelVisible = true;
                IsHepBolusEntryVisible = true;
                IsHepDripLabelVisible = true;
                IsHepDripEntryVisible = true;

                NavigateToHeparinDose?.Invoke(Heparin, Weight);
            }
        }

        public void Validate(out List<string> errors)
        {
            errors = [];

            check(Mrn, "MRN", errors);
            checkNumber(Weight, "Weight", errors);
            checkNumber(Height, "Height", errors);
            check(Citrate, "Citrate", errors);
            check(LivDys, "Liver Dysfuntion", errors);
            checkNumber(BFR, "Blood Flow Rate", errors);
            check(Heparin, "Heparin", errors);

            if (Heparin == "Yes") {
                checkNumber(HepBol, "Heparin Bolus", errors);
                checkNumber(HepDrip, "Heparin Drip", errors);
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
