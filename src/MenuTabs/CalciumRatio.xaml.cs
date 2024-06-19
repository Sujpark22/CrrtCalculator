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
            if (!string.IsNullOrEmpty(totLabel.Text) &&
                !string.IsNullOrEmpty(serLabel.Text) &&
                !string.IsNullOrEmpty(patLabel.Text))
            {
                double totCalcium = double.Parse(totLabel.Text);
                double serumAlbumin = double.Parse(serLabel.Text);
                double patientICal = double.Parse(patLabel.Text);

                double correctedCalc = (totCalcium + 0.8 * (4 - serumAlbumin));
                double calciumRatio = correctedCalc * 0.25 / patientICal;
                calcRatLabel.Text = $"Calcium Ratio: {calciumRatio}";
            }
            else
            {
                calcRatLabel.Text = "Calcium Ratio: -";
            }
        }
    }
}
