using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CRRT_Calculator
{
    public partial class AquaCalcOutputPage : ContentPage
    {
        public AquaCalcOutputPage(ViewModels.AquaCalculatorInput input)
        {
            InitializeComponent();

            //patient mrn
            mrnLabel2.Text = $"Patient mrn: {input.Mrn}";

            //access
            double weightD = double.Parse(input.Weight);
            string access;
            if (weightD < 3 && weightD >= 1.8)
            {
                access = "5Fr PICC no longer than 10-12cm";
            }
            else if (weightD >= 3 && weightD < 8)
            {
                access = "6Fr PICC no longer than 10-12cm";
            }
            else if (weightD >= 8)
            {
                access = "7Fr Dialysis catheter";
            }
            else
            {
                access = "N/A";
            }
            accessLabel.Text = $"Access: {access}";

            //Blood flow rate
            if (weightD < 3 && weightD >= 1.8)
            {
                bfrLabel.Text = $"Blood flow rate: 20";
            }
            else if (weightD >= 3 && weightD < 8)
            {
                bfrLabel.Text = $"Blood flow rate: 30";
            }
            else if (weightD >= 8)
            {
                bfrLabel.Text = $"Blood flow rate: 40";
            }
            else
            {
                bfrLabel.Text = $"Blood flow rate: N/A";
            }

            //reinfusion fluid
            reLabel.Text = $"Reinfusion fluid: {input.Fluid}";

            //reinfusion fluid rate
            double modAquaDose = input.Mod == "Yes" ? 24 * weightD : 0;
            rfrLabel.Text = $"Reinfusion fluid rate (ml/hr): {Math.Ceiling(modAquaDose / 5) * 5}";

            //Calcium gtt
            double calcGTT = input.Fluid == "Phoxillium 4/2.5" ? 0.5 * weightD : weightD;
            calcGTTLabel.Text = $"Calcium gtt (ml/hr): {calcGTT}";

            //Anticoagulation
            string anti = input.Heparin == "Yes" ? "Heparin" : input.Epop == "Yes" ? "Epoprostenol" : "None";
            anticoLabel.Text = $"Anticoagulation: {anti}";

            //heparin bolus dose
            double hepDose = input.Heparin == "Yes" ? double.Parse(input.HepBol) : 0;
            hepBolusLabel.Text = $"Heparin bolus dose (units): {hepDose}";

            //Heparin gtt rate (units/kg/hr)
            double gttRate = input.Heparin == "Yes" ? double.Parse(input.HepGTT) / weightD : 0;
            hepGTTLabel.Text = $"Heparin gtt rate (units/kg/hr): {gttRate}";

            //Epoprostenol
            string epDose = input.Epop == "Yes" ? "Start at 2 ng/kg/min and increase by 2 ng/kg/min to a max of 8 ng/kg/min" : "N/A";
            string ep = input.Epop == "Yes" ? epDose : "0";
            epDoseLabel.Text = $"Epoprostenol: {ep}";

        }
    }

}