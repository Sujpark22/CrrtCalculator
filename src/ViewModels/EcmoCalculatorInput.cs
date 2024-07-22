using CommunityToolkit.Mvvm.ComponentModel;

namespace CRRT_Calculator.ViewModels;

public partial class EcmoCalculatorInput : ObservableObject
{
	[ObservableProperty]
	string?
		_mrn,
		_weight,
		_height,
		_clear,
		_antiEC,
		_citrate;

	[ObservableProperty]
	DateTime _dob;

	//public bool IsInvalid()
	//{
	//	return string.IsNullOrWhiteSpace(Mrn) ||
	//		isInvalidNumber(Weight) ||
	//		isInvalidNumber(Height) ||
	//		string.IsNullOrWhiteSpace(Clear) ||
	//		string.IsNullOrWhiteSpace(AntiEC) ||
	//		string.IsNullOrWhiteSpace(Citrate);

	//	static bool isInvalidNumber(string? number) => !double.TryParse(number, out _);
	//}

	public void Validate(out List<string> errors)
	{
		errors = [];

		check(Mrn, "MRN", errors);
		checkNumber(Weight, "Weight", errors);
		checkNumber(Height, "Height", errors);
		check(Clear, "Clear", errors);
		check(AntiEC, "AntiEC", errors);
		check(Citrate, "Citrate", errors);

		static void check(string? value, string caption, ICollection<string> errors)
		{
			if (string.IsNullOrWhiteSpace(value))
				errors.Add($"{caption} is empty");
		}

		static void checkNumber(string? number, string caption, ICollection<string> errors)
		{
			if (!double.TryParse(number, out _))
				errors.Add($"{caption} is invalid number");
		}
	}
}
