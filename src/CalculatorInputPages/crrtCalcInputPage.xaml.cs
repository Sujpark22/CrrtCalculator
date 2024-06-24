using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;


namespace CRRT_Calculator
{
    public partial class crrtCalcInputPage : ContentPage
    {
        public crrtCalcInputPage()
        {
            InitializeComponent();
        }

        private async void OnEcmoPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ecmoPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT on ECMO Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new ecmoCalcInputPage());
                }
            }
        }

        private void OnDobPickerDateSelected(object sender, DateChangedEventArgs e)
        {
            CalculateBloodFlowRates();
        }
        private async void OnAquaPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (aquaPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'Aquapheresis Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new aquaCalcInputPage());
                }
            }
        }

        private async Task ShowWarning(string message)
        {
            warningLabel.Text = message;
        }

        private async Task ClearWarning()
        {
            warningLabel.Text = "";
        }

        private async Task EvaluateWeight(double weight)
        {
            if (weight <= 8 && weight >= 1.8)
            {
                await ShowWarning("*Should Consider Aquapheresis First");
            }
            else if (weight <= 12 && weight > 8)
            {
                await ShowWarning("*May consider Aquapheresis if indicated");
            }
            else if (weight < 1.8)
            {
                await ShowWarning("*Reconsider RRT");
            }
            else
            {
                await ClearWarning();
            }
        }

        private async void OnWeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(e.NewTextValue, out double weight))
            {
                await EvaluateWeight(weight);
                CalculateBloodFlowRates();
            }
            else
            {
                await ClearWarning();
            }
        }

        private void CalculateBloodFlowRates()
        {
            // Get the inputted weight and date of birth
            if (double.TryParse(weightEntry.Text, out double weight))
            {
                DateTime dob = dobPicker.Date;
                DateTime today = DateTime.Today;
                int age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--;

                double minBFR = age < 1 ? 8 * weight : (age < 6 ? 6 * weight : 3 * weight);
                double maxBFR = age < 1 ? 10 * weight : (age < 6 ? 8 * weight : 6 * weight);

                minBFRLabel.Text = $"Minimum Blood Flow Rate: {minBFR} ml/min";
                maxBFRLabel.Text = $"Maximum Blood Flow Rate: {maxBFR} ml/min";
            }
            else
            {
                minBFRLabel.Text = "Minimum Blood Flow Rate: -";
                maxBFRLabel.Text = "Maximum Blood Flow Rate: -";
            }
        }

        private void OnBFEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(e.NewTextValue, out double bloodFlowRate))
            {
                if (bloodFlowRate > 150)
                {
                    BFEntry.Text = "YOU MAY HAVE EXCEEDED MAX BLOOD FLOW RATE";
                }

            }
            UpdateHeparinFields();
        }

        private void OnHepPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHeparinFields();
        }

        private void UpdateHeparinFields()
        {
            if (hepPicker.SelectedItem == null || string.IsNullOrEmpty(weightEntry.Text) || !double.TryParse(weightEntry.Text, out double weightD))
            {
                hepBolLabel.IsVisible = false;
                hepBolusEntry.IsVisible = false;
                hepDripLabel.IsVisible = false;
                hepDripEntry.IsVisible = false;
                return;
            }

            else if (hepPicker.SelectedItem.ToString() == "Yes")
            {
                string heparin = hepPicker.SelectedItem.ToString();
                string weight = weightEntry.Text;
                var hepWS = new Window(new HeparinDose(heparin, weight));

                if (DeviceInfo.Platform == DevicePlatform.WinUI)
                {
                    Application.Current.OpenWindow(hepWS);
                }
                else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    Navigation.PushAsync(new HeparinDose(heparin, weight));
                }
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            string mrn = mrnEntry.Text;
            DateTime dob = dobPicker.Date;
            string ecmo = ecmoPicker.SelectedItem?.ToString();
            string weight = weightEntry.Text;
            string height = heightEntry.Text;
            string aqua = aquaPicker.SelectedItem?.ToString();
            string bloodFlowRate = BFEntry.Text;
            string heparin = hepPicker.SelectedItem?.ToString();
            string citrate = citPicker.SelectedItem?.ToString();
            string liverDysfunction = livPicker.SelectedItem?.ToString();
            string heparinBolus = hepBolusEntry.Text;
            string heparinDrip = hepDripEntry.Text;

            if (string.IsNullOrWhiteSpace(mrn) ||
                string.IsNullOrWhiteSpace(weight) ||
                string.IsNullOrWhiteSpace(height) ||
                string.IsNullOrWhiteSpace(bloodFlowRate) ||
                string.IsNullOrWhiteSpace(heparin) ||
                string.IsNullOrWhiteSpace(citrate) ||
                string.IsNullOrWhiteSpace(liverDysfunction) ||
                (heparin == "Yes" && (string.IsNullOrWhiteSpace(heparinBolus) || string.IsNullOrWhiteSpace(heparinDrip))))
            {
                await DisplayAlert("Error", "Please fill out all required fields.", "OK");
                return;
            }

            await Navigation.PushAsync(new crrtCalcOutputPage(mrn, dob, ecmo, weight, height, aqua, bloodFlowRate, heparin, citrate, liverDysfunction, heparinBolus, heparinDrip));
        }
    }
}