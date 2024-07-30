using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using CRRT_Calculator.ViewModels;



namespace CRRT_Calculator
{
    public partial class AquaCalcInputPage : ContentPage
    {
        public AquaCalcInputPage()
        {
            InitializeComponent();
            _model = new AquaCalculatorInput(); 
            BindingContext = _model;
        }

        readonly ViewModels.AquaCalculatorInput _model;

        private async void OnClearChanged(object sender, EventArgs e)
        {
            if (clearPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new CrrtCalcInputPage());
                }
            }
        }

        private async void OnCandChanged(object sender, EventArgs e)
        {
            if (candPicker.SelectedItem.ToString() == "No")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new CrrtCalcInputPage());
                }
            }
        }

        private async void OnEcmoChanged(object sender, EventArgs e)
        {
            if (ecmoPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT on ECMO Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new EcmoCalcInputPage());
                }
            }
        }

        private async void WeightEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (BindingContext is CrrtCalculatorInput viewModel)
            {
                viewModel.UpdateHeparinFields();
                viewModel.CalculateBFR();
                viewModel.OnWeightEntryUnfocused();
            }
        }

        private void OnNavigateToHeparinDose(string heparin, string weight)
        {
            var hepWS = new Window(new HeparinDose(heparin, weight));

            if (DeviceInfo.Platform == DevicePlatform.WinUI || DeviceInfo.Platform == DevicePlatform.macOS)
            {
                Application.Current.OpenWindow(hepWS);
            }
            else if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Navigation.PushAsync(new HeparinDose(heparin, weight));
            }
        }

        private async void Submit_Clicked(object sender, EventArgs e)
        {
            _model.Validate(out var errors);

            if (errors.Any())
            {
                await DisplayAlert("Error", string.Join("\n", errors), "OK");
                return;
            }

            await Navigation.PushAsync(new AquaCalcOutputPage(_model));
        }

        /*private void OnWeightEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (double.TryParse(weightEntry.Text, out double weight))
            {
                if (weight > 12)
                {
                    warningLabel.Text = "*'Modified' Aquapheresis is not indicated, use CRRT for clearance and aquapheresis for fluid removal only";
                }
                else
                {
                    warningLabel.Text = "";
                }
            }
            else
            {
                warningLabel.Text = "";
            }
            UpdateHeparinFields();
        }

        private void OnHepPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHeparinFields();
        }

        private void UpdateHeparinFields()
        {
            if (hepPicker.SelectedItem == null || hepPicker.SelectedItem.ToString() == "No" || string.IsNullOrEmpty(weightEntry.Text) || !double.TryParse(weightEntry.Text, out double weightD))
            {
                hepBolLabel.IsVisible = false;
                hepBolusEntry.IsVisible = false;
                hepGTTLabel.IsVisible = false;
                hepGTTEntry.IsVisible = false;
                return;
            }

            else if(hepPicker.SelectedItem.ToString() == "Yes")
            {
                hepBolLabel.IsVisible = true;
                hepBolusEntry.IsVisible = true;
                hepGTTLabel.IsVisible = true;
                hepGTTEntry.IsVisible = true;
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
            string clear = clearPicker.SelectedItem?.ToString();
            string cand = candPicker.SelectedItem?.ToString();
            string mod = modPicker.SelectedItem?.ToString();
            string ecmo = ecmoPicker.SelectedItem?.ToString();
            string weight = weightEntry.Text;
            string height = heightEntry.Text;
            string reinfusion = fluidPicker.SelectedItem?.ToString();
            string heparin = hepPicker.SelectedItem?.ToString();
            string heparinBolus = hepBolusEntry.Text;
            string heparinGTT = hepGTTEntry.Text;
            string epop = epPicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(mrn) ||
                string.IsNullOrWhiteSpace(clear) ||
                string.IsNullOrWhiteSpace(cand) ||
                string.IsNullOrWhiteSpace(mod) ||
                string.IsNullOrWhiteSpace(ecmo) ||
                string.IsNullOrWhiteSpace(weight) ||
                string.IsNullOrWhiteSpace(height) ||
                string.IsNullOrWhiteSpace(reinfusion) ||
                string.IsNullOrWhiteSpace(heparin) ||
                string.IsNullOrWhiteSpace(epop) ||
                (heparin == "Yes" && (string.IsNullOrWhiteSpace(heparinBolus) || string.IsNullOrWhiteSpace(heparinGTT))))
            {
                await DisplayAlert("Error", "Please fill out all fields.", "OK");
                return;
            }

            await Navigation.PushAsync(new AquaCalcOutputPage(mrn, dob, clear, cand, mod, ecmo, weight, height, reinfusion, heparin, heparinBolus, heparinGTT, epop));
        }*/
    }
}
