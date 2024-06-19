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
            Navigation.PushAsync(new crrtCalcInputPage());
        }

        private void Button2_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new aquaCalcInputPage());
        }

        private void Button3_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page3());
        }

        private void Button4_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page4());
        }

    }
}
