using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace FronteraRadio.Views;

// Heredamos usando el nombre largo para evitar confusiones con MAUI interno
public partial class UpdatePopup : CommunityToolkit.Maui.Views.Popup
{
	private readonly string _url;

	public string UpdateMessage { get; set; }

	public UpdatePopup(string message, string url)
	{
		InitializeComponent();

		// Al heredar explícitamente, esta línea YA NO debe dar error
		// this.CanBeDismissedByTappingOutside = false;
		// this.Color = Colors.Transparent;

		this.UpdateMessage = message;
		this._url = url;
		this.BindingContext = this;
	}

	[RelayCommand]
	private async Task Update()
	{
		if (!string.IsNullOrWhiteSpace(_url))
		{
			await Browser.Default.OpenAsync(_url, BrowserLaunchMode.External);
		}
	}
}