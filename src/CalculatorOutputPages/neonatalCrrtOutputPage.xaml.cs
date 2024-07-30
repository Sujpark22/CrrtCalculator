using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;

namespace CRRT_Calculator
{
    public partial class NeonatalCrrtOutputPage : ContentPage
    {
        public NeonatalCrrtOutputPage(ViewModels.NeoCalculatorInput input)
        {
            InitializeComponent();

            //prescription details
            //patient mrn
            mrnLabel.Text = $"Patient mrn: {input.Mrn}";

            //access
            String access;
            double weightD = double.Parse(input.Weight);
            if (weightD >= 1.8 && weightD < 10)
            {
                access = "7-8FR";
            }
            else if (weightD >= 10 && weightD < 20)
            {
                access = "8-9FR";
            }
            else if (weightD >= 20)
            {
                access = "11.5FR or larger";
            }
            else
            {
                access = "Weight is below 1.8";
            }
            accessLabel.Text = $"Access: {access}";

            //Filter
            String filter;
            if (weightD <= 12)
            {
                filter = "HF20";
            }
            else if (weightD > 12 && weightD < 20)
            {
                filter = "ST60";
            }
            else if (weightD >= 20)
            {
                filter = "HF1000";
            }
            else
            {
                filter = "Invalid value";
            }
            filterLabel.Text = $"Filter: {filter}";

            //Blood Flow Rate
            bfrLabel.Text = $"Blood FLow Rate: {input.BFR}";

            //Dialysate (ml/hr)
            Double heightD = double.Parse(input.Height);
            Double bsa = 0.007184 * Math.Pow(weightD, 0.425) * Math.Pow(heightD, 0.725);
            Double crrtDoseUnder12 = weightD <= 12 ? 120 * weightD : 0;
            Double crrtDoseOver12 = (weightD > 12) ? (8000 * bsa / 1.73) : 0;
            Double crrtDoseChosen = (weightD <= 12) ? crrtDoseUnder12 : crrtDoseOver12;
            int diaRate;
            if (filter == "HF20")
            {
                diaRate = (int)crrtDoseUnder12;
            }
            else
            {
                diaRate = (int)crrtDoseChosen / 2;
            }
            double dia = Math.Ceiling((double)diaRate / 10) * 10;
            diaLabel.Text = $"Dialysate (ml/hr): {dia}";

            //PBP reinfusion rate (ml/hr)
            int post = (filter == "HF20") ? 40 : 100;
            int pbp1 = (filter == "HF20") ? 0 : (int)((crrtDoseChosen / 2) - post);
            double pbp2 = Math.Ceiling((double)pbp1 / 10) * 10;
            pbpLabel.Text = $"PBP reinfusion rate (ml/hr): {pbp2}";

            //Post filter reinfusion rate (ml/hr)
            postLabel.Text = $"Post filter reinfusion rate (ml/hr): {post}";

            //Calcium gtt
            double citDose;
            int years = DateTime.Today.Year - input.Dob.Year;
            if (DateTime.Today < input.Dob.AddYears(years))
            {
                years--;
            }
            bool isLessThanOneYear = (years < 1);
            Double bfr = double.Parse(input.BFR);

            if (input.LivDys == "Yes" && input.Citrate == "Yes")
            {
                citDose = 0.75 * bfr;
            }
            else if (input.Citrate == "Yes" && isLessThanOneYear)
            {
                citDose = 0.75 * bfr;
            }
            else if (input.Citrate == "Yes" && input.LivDys == "No" && !isLessThanOneYear)
            {
                citDose = 1.5 * bfr;
            }
            else
            {
                citDose = 0;
            }

            double calcDrip1 = (input.Citrate == "Yes") ? 0.4 * citDose : 0;
            double calcDrip2 = (input.Citrate == "No") ? 1 * weightD : 0;
            double calcGTT = (input.Citrate == "Yes") ? calcDrip1 : calcDrip2;
            calcGTTLabel.Text = $"Calcium GTT: {calcGTT}";

            //Anticoagulation
            String anti = "";
            if (input.Heparin == "Yes")
            {
                anti = "Heparin";
            }
            if (input.Heparin == "No" && input.Citrate == "Yes")
            {
                anti = "Citrate";
            }
            if (input.Heparin == "No" && input.Citrate == "No")
            {
                anti = "None or Epoprostenol";
            }

            anticoLabel.Text = $"Anticoagulation: {anti}";

            //Citrate
            citLabel.Text = $"Citrate: {citDose}";

            //Heparin bolus (units)
            double hB = input.Heparin == "Yes" ? double.Parse(input.HepBol) : 0;
            hepBolusLabel.Text = $"Heparin bolus: {hB}";

            //Heparin gtt (units/kg/hr)
            double hG = input.Heparin == "Yes" ? double.Parse(input.HepDrip) / weightD : 0;
            hepGTTLabel.Text = $"Heparin gtt (units/kg/hr): {hG}";

            //Blood Prime (if indicated)
            String bp;
            if (filter == "HF20")
            {
                bp = "60ml pRBC + 30ml bicarbonate";
            }
            else if (filter == "ST60")
            {
                bp = "80ml pRBC + 40ml bicarbonate";
            }
            else if (filter == "HF1000")
            {
                bp = "120ml pRBC + 60ml bicarbonate";
            }
            else
            {
                bp = "Unknown";
            }
            bloodPrimeLabel.Text = $"Blood Prime (if indicated): {bp}";
        }
    }

}