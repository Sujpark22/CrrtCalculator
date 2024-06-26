using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CRRT_Calculator.ViewModels;

class HeparinDose : ObservableObject
{
	public HeparinDose(string? heparin, string weight)
	{
		if (heparin != null)
		{
			double weightD = double.Parse(weight);
			//heparin 10
			hep10 = (heparin == "Yes") ? 10 * weightD : 0;
			hep10Label = $"Heparin 10 units/kg: {hep10}";

			//heparin 20
			hep20 = (heparin == "Yes") ? 20 * weightD : 0;
			hep20Label = $"Heparin 20 units/kg: {hep20}";

			//heparin30
			hep30 = (heparin == "Yes") ? 30 * weightD : 0;
			hep30Label = $"Heparin 30 units/kg: {hep30}";
		}
	}

	readonly double hep10, hep20, hep30;

	string _bolusACT, _bolusPTT;

	public string hep10Label { get; }
	public string hep20Label { get; }
	public string hep30Label { get; }

	public string preACT { get; set; }
	public string prePTT { get; set; }

	public string bolusACT
	{
		get => _bolusACT;
		private set => SetProperty(ref _bolusACT, value);
	}

	public string bolusPTT
	{
		get => _bolusPTT;
		private set => SetProperty(ref _bolusPTT, value);
	}

	public void Calculate()
	{
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
		this.bolusACT = $"Suggested heparin bolus (based on ACT):     {bolusACT}";


		//Suggested heparin bolus (based on PTT)
		string bolusPTT;
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
		this.bolusPTT = $"Suggested heparin bolus (based on PTT):     {bolusPTT}";
	}
}
