﻿using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;


namespace CRRT_Calculator
{
    public partial class Page4 : ContentPage
    {
        public Page4()
        {
            InitializeComponent();
        }

        private void OnDobPickerDateSelected(object sender, DateChangedEventArgs e)
        {
            CalculateBloodFlowRates();
        }

        private void OnWeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateBloodFlowRates();
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
        }

        private void OnHepPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            bool isHeparinNo = hepPicker.SelectedItem.ToString() == "No";
            bool isHeparinYes = hepPicker.SelectedItem.ToString() == "Yes";

            if (isHeparinNo)
            {
                hepBolLabel.IsVisible = false;
                hepBolusEntry.IsVisible = false;
                hepDripLabel.IsVisible = false;
                hepDripEntry.IsVisible = false;

            }
            else if (isHeparinYes)
            {
                hepBolLabel.IsVisible = true;
                hepBolusEntry.IsVisible = true;
                hepDripLabel.IsVisible = true;
                hepDripEntry.IsVisible = true;

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
            string weight = weightEntry.Text;
            string height = heightEntry.Text;
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
                string.IsNullOrWhiteSpace(citrate) ||
                string.IsNullOrWhiteSpace(liverDysfunction) ||
                string.IsNullOrWhiteSpace(heparinBolus) ||
                (heparin == "Yes" && (string.IsNullOrWhiteSpace(heparinBolus) || string.IsNullOrWhiteSpace(heparinDrip))))
            {
                await DisplayAlert("Error", "Please fill out all required fields.", "OK");
                return;
            }
            await Navigation.PushAsync(new Page4Display(mrn, dob, weight, height, bloodFlowRate, heparin, citrate, liverDysfunction, heparinBolus, heparinDrip));
        }
    }
}