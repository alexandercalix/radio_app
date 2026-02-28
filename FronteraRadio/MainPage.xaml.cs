using FronteraRadio.Views;
namespace FronteraRadio;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		// Esperamos a que la página esté totalmente dibujada
		this.Loaded += OnMainPageLoaded;
	}

	private async void OnMainPageLoaded(object sender, EventArgs e)
	{
		// Le damos un respiro extra al sistema
		await Task.Delay(1000);

		// Navegamos al Player de forma asíncrona y segura
		await Shell.Current.GoToAsync("//PlayerPage");
	}
}