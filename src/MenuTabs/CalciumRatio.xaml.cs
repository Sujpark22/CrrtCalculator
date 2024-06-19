namespace CRRT_Calculator
{
    public partial class CalciumRatio : ContentPage
    {
        public CalciumRatio()
        {
            InitializeComponent();
        }

        private void RatioTextChanged(object sender, TextChangedEventArgs e)
        {
            warningLabel.IsVisible = false;

            if (!string.IsNullOrEmpty(totLabel.Text) &&
                !string.IsNullOrEmpty(serLabel.Text) &&
                !string.IsNullOrEmpty(patLabel.Text))
            {
                double totCalcium, serumAlbumin, patientICal;
                bool isTotCalciumValid = double.TryParse(totLabel.Text, out totCalcium);
                bool isSerumAlbuminValid = double.TryParse(serLabel.Text, out serumAlbumin);
                bool isPatientICalValid = double.TryParse(patLabel.Text, out patientICal);

                if (isTotCalciumValid && isSerumAlbuminValid && isPatientICalValid)
                {
                    double correctedCalc = (totCalcium + 0.8 * (4 - serumAlbumin));
                    double calciumRatio = correctedCalc * 0.25 / patientICal;
                    calcRatLabel.Text = $"Calcium Ratio: {calciumRatio:F2}";

                    if (calciumRatio <= 2.2)
                    {
                        calcRatLabel.TextColor = Colors.Green;
                    }
                    else if (calciumRatio > 2.2 || calciumRatio < 2.5)
                    {
                        calcRatLabel.TextColor = Colors.Yellow;
                    }
                    else if (calciumRatio >= 2.5)
                    {
                        calcRatLabel.TextColor = Colors.Red;
                    }
                }
                else
                {
                    calcRatLabel.Text = "Calcium Ratio: -";
                    calcRatLabel.TextColor = Colors.LightBlue;
                    warningLabel.Text = "Please enter valid numbers in all fields.";
                    warningLabel.IsVisible = true;
                }
            }
            else
            {
                calcRatLabel.Text = "Calcium Ratio: -";
                calcRatLabel.TextColor = Colors.LightBlue; // Reset to default color
                warningLabel.Text = "Please fill in all fields.";
                warningLabel.IsVisible = true;
            }
        }
    }
}
