using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;


namespace CRRT_Calculator
{
    public partial class HeparinDose : ContentPage
    {
        private double weightD;
        private double hep10;
        private double hep20;
        private double hep30;
        public HeparinDose(string heparin, string weight)
        {
            InitializeComponent();

            weightD = double.Parse(weight);
            //heparin 10
            hep10 = (heparin == "Yes") ? 10 * weightD : 0;
            hep10Label.Text = $"Heparin 10 units/kg: {hep10}";

            //heparin 20
            hep20 = (heparin == "Yes") ? 20 * weightD : 0;
            hep20Label.Text = $"Heparin 20 units/kg: {hep20}";

            //heparin30
            hep30 = (heparin == "Yes") ? 30 * weightD : 0;
            hep30Label.Text = $"Heparin 30 units/kg: {hep30}";

        }

        private void OnCalculateClicked(object sender, EventArgs e)
        {

            string preACT = preACTEntry.Text;
            string prePTT = prePTTEntry.Text;

            
            //Suggested heparin bolus (based on ACT)
            string bolusACT;
            double preACTD = double.Parse(preACT);
            if (preACTD <= 120)
            {
                bolusACT = hep30.ToString();
            }
            else if (preACTD > 120 && preACTD < 140)
            {
                bolusACT = hep20.ToString();
            }
            else if (preACTD >= 140 && preACTD < 160)
            {
                bolusACT = hep10.ToString();
            }
            else if (preACTD >= 160)
            {
                bolusACT = "HOLD BOLUS i.e. 0";
            }
            else
            {
                bolusACT = "Unknown";
            }
            bolusACTLabel.Text = $"Suggested heparin bolus (based on ACT): {bolusACT}";


            //Suggested heparin bolus (based on PTT)
            string bolusPTT = "";
            double prePTTD = double.Parse(prePTT);
            if (prePTTD <= 15)
            {
                bolusPTT = hep30.ToString();
            }
            else if (prePTTD > 15 && prePTTD <= 30)
            {
                bolusPTT = hep20.ToString();
            }
            else if (prePTTD > 30 && prePTTD < 40)
            {
                bolusPTT = hep10.ToString();
            }
            else if (prePTTD >= 40)
            {
                bolusPTT = "HOLD BOLUS i.e. 0";
            }
            else
            {
                bolusPTT = "Unknown";
            }
            bolusPTTLabel.Text = $"Suggested heparin bolus (based on PTT): {bolusPTT}";
        }
    }
}