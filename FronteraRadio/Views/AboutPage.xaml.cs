using FronteraRadio.ViewModels;

namespace FronteraRadio.Views;

public partial class AboutPage : ContentPage
{
	// Usamos el ViewModel que ya tiene los datos cargados
	public AboutPage(AboutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		// Opcional: Esto asegura que el scroll se resetee al entrar

		// Forzamos a que Android re-evalue los Bindings al entrar a la página
		if (BindingContext is AboutViewModel vm)
		{
			OnPropertyChanged(nameof(BindingContext));
		}
	}
}