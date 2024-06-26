using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;


namespace CRRT_Calculator
{
    public partial class HeparinDose : ContentPage
    {
        public HeparinDose(string heparin, string weight)
        {
            InitializeComponent();

			_model = new ViewModels.HeparinDose(heparin, weight);
            BindingContext = _model;
        }

        readonly ViewModels.HeparinDose _model;

        private void OnCalculateClicked(object sender, EventArgs e)
        {
            _model.Calculate();
        }
    }
}