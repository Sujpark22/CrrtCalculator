using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using CRRT_Calculator.ViewModels;
using System.Diagnostics;


namespace CRRT_Calculator
{
    public partial class NeonatalCrrtInputPage : ContentPage
    {
        public NeonatalCrrtInputPage()
        {
            InitializeComponent();

            BindingContext = _model = new NeoCalculatorInput();
            _model.NavigateToHeparinDose += OnNavigateToHeparinDose;
        }

        readonly NeoCalculatorInput _model;


        private void WeightEntry_Unfocused(object sender, FocusEventArgs e)
        {
            if (BindingContext is NeoCalculatorInput viewModel)
            {
                viewModel.UpdateHeparinFields();
                viewModel.CalculateBFR();
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

            await Navigation.PushAsync(new NeonatalCrrtOutputPage(_model));
        }
    }
}