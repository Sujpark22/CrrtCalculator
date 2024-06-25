namespace CRRT_Calculator
{
    public partial class Calculators : ContentPage
    {
        public Calculators()
        {
            InitializeComponent();
        }

        private void Button1_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CrrtCalcInputPage());
        }

        private void Button2_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AquaCalcInputPage());
        }

        private void Button3_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new EcmoCalcInputPage());
        }

        private void Button4_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NeonatalCrrtInputPage());
        }

    }
}
