using System;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.Logging;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace CRRT_Calculator
{
    public partial class EcmoCalcOutputPage : ContentPage
    {
        public EcmoCalcOutputPage(string mrn, DateTime dob, string weight, string height, string clear, string antiEC, string citrate)
        {
            InitializeComponent();

            //patient mrn
            mrnLabel2.Text = $"Patient mrn: {mrn}";

            //filter
            double weightD = double.Parse(weight);
            double heightD = double.Parse(height);
            string filter;
            filter = (weightD <= 12 && clear == "No") ? "CVVHD via HemoCor" :
                (weightD <= 12 && clear == "Yes") ? "ST60" :
                (weightD > 12 && weightD < 20) ? "ST60" :
                (weightD >= 20) ? "HF1000" : "";
            filterLabel.Text = $"Filter: {filter}";

            //blood flow rate
            int ageYears = DateTime.Today.Year - dob.Year;
            if (dob > DateTime.Today.AddYears(-ageYears)) ageYears--;

            double bfr1 = (ageYears < 1) ? 10 * weightD :
                          (ageYears >= 1 && ageYears < 6) ? 8 * weightD :
                          (ageYears >= 6) ? 6 * weightD : 0;

            double bfr2 = (bfr1 > 150) ? 150 : bfr1;
            double bfr3 = Math.Ceiling(bfr2 / 5) * 5;
            bfrLabel.Text = $"Blood Flow Rate: {bfr3}";

            //dialysate
            string hemoCor = (weightD <= 12 && clear == "No") ? (24 * weightD).ToString() : "CRRT";
            double bsa = 0.007184 * Math.Pow(heightD, 0.725) * Math.Pow(weightD, 0.425);
            double crrtDose = (clear == "Yes" && weightD <= 12) ? 30 * weightD :
                              (weightD > 12) ? 2000 * bsa / 1.73 : 0;
            string dia1 = (filter == "CVVHD via HemoCor") ? hemoCor : (crrtDose / 2).ToString();
            double dia2 = (double.TryParse(dia1, out double numericValue)) ? Math.Ceiling(numericValue / 10) * 10 : 0;
            diaLabel.Text = $"Dialysate (ml/hr): {dia2}";

            //pre-blood pump reinfusion
            double pbp = (filter == "CVVHD via HemoCor") ? 0 : (crrtDose / 2) - 100;
            pbpLabel.Text = $"Pre-blood pump reinfusion (ml/hr): {Math.Ceiling(pbp/10)*10}";

            //post filter reinfusion
            double pfr = (filter == "CVVHD via HemoCor") ? 0 : 100;
            pfrLabel.Text = $"Post-filter reinfusion (ml/hr): {pfr}";

            //citrate
            double citDose = (citrate == "Yes") ? 1.5 * bfr3 : 0;
            citLabel.Text = $"Citrate: {Math.Ceiling(citDose)}";

            //citrate Warning
            citWarning.Text = "If Liver dysfuction present, or infant, may need to cut dose";

            //calcium
            double calc = (citrate == "Yes") ? 0.4 * citDose : weightD;
            calcLabel.Text = $"Calcium: {Math.Ceiling(calc)}";

            //calcium Warning
            calcWarning.Text = "Rate to be adjusted if changes made to citrate rate";

        }
    }

}