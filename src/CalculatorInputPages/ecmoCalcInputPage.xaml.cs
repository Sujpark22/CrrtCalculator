
namespace CRRT_Calculator;

using ViewModels;

public partial class EcmoCalcInputPage : ContentPage
{
	public EcmoCalcInputPage()
	{
		InitializeComponent();

		BindingContext = _model = new EcmoCalculatorInput();
	}

	readonly EcmoCalculatorInput _model;
       
	private async void Submit_Clicked(object sender, EventArgs e)
	{
		_model.Validate(out var errors);

		if (errors.Any())
		{
			await DisplayAlert("Error", string.Join("\n", errors) , "OK");
			return;
		}

		await Navigation.PushAsync(new EcmoCalcOutputPage(_model));
	}
}
