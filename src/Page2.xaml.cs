using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using static Microsoft.Maui.ApplicationModel.Permissions;


namespace CRRT_Calculator
{
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();
        }

        private async void OnClearPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (clearPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new Page1());
                }
            }
        }

        private async void OnCandPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (candPicker.SelectedItem.ToString() == "No")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new Page1());
                }
            }
        }

        private async void OnEcmoPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ecmoPicker.SelectedItem.ToString() == "Yes")
            {
                bool answer = await DisplayAlert("", "Use the 'CRRT on ECMO Calculator'", "Go", "Cancel");
                if (answer)
                {
                    await Navigation.PushAsync(new Page3());
                }
            }
        }

        private void OnWeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(e.NewTextValue, out double weight))
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
        }

        private void OnHepPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            bool isHeparinNo = hepPicker.SelectedItem.ToString() == "No";
            bool isHeparinYes = hepPicker.SelectedItem.ToString() == "Yes";

            if (isHeparinNo)
            {
                hepBolLabel.IsVisible = false;
                hepBolusEntry.IsVisible = false;
                hepGTTLabel.IsVisible = false;
                hepGTTEntry.IsVisible = false;

            }
            else if (isHeparinYes)
            {
                hepBolLabel.IsVisible = true;
                hepBolusEntry.IsVisible = true;
                hepGTTLabel.IsVisible = true;
                hepGTTLabel.IsVisible = true;

                string heparin = hepPicker.SelectedItem?.ToString();
                string weight = weightEntry.Text;
                var hepWS = new Window(new HeparinDose(heparin, weight));
                Application.Current.OpenWindow(hepWS);
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
                string.IsNullOrWhiteSpace(heparinBolus) ||
                string.IsNullOrWhiteSpace(heparinGTT) ||
                string.IsNullOrWhiteSpace(epop) ||
                (heparin == "Yes" && (string.IsNullOrWhiteSpace(heparinBolus) || string.IsNullOrWhiteSpace(heparinGTT))))
            {
                await DisplayAlert("Error", "Please fill out all fields.", "OK");
                return;
            }

            await Navigation.PushAsync(new Page2Display(mrn, dob, clear, cand, mod, ecmo, weight, height, reinfusion, heparin, heparinBolus, heparinGTT, epop));
        }
    }
}
