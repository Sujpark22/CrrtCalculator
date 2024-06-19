using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using static Microsoft.Maui.ApplicationModel.Permissions;


namespace CRRT_Calculator
{
    public partial class Page3 : ContentPage
    {
        public Page3()
        {
            InitializeComponent();
        }

       
        private async void Submit_Clicked(object sender, EventArgs e)
        {
            string mrn = mrnEntry.Text;
            DateTime dob = dobPicker.Date;
            string weight = weightEntry.Text;
            string height = heightEntry.Text;
            string clear = clearPicker.SelectedItem?.ToString();
            string antiEC = antiPicker.SelectedItem?.ToString();
            string citrate = citPicker.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(mrn) ||
                string.IsNullOrWhiteSpace(weight) ||
                string.IsNullOrWhiteSpace(height) ||
                string.IsNullOrWhiteSpace(clear) ||
                string.IsNullOrWhiteSpace(antiEC) ||
                string.IsNullOrWhiteSpace(citrate))
            {
                await DisplayAlert("Error", "Please fill out all required fields.", "OK");
                return;
            }

            await Navigation.PushAsync(new Page3Display(mrn, dob, weight, height, clear, antiEC, citrate));
        }
    }
}
