using FronteraRadio.Core.Interfaces;
using FronteraRadio.ViewModels;


namespace FronteraRadio.Views;

public partial class AboutPage : ContentPage
{
	private readonly IFirebaseService _firebaseService;

	// Usamos el ViewModel que ya tiene los datos cargados
	public AboutPage(AboutViewModel vm, IFirebaseService firebaseService)
	{
		InitializeComponent();
		BindingContext = vm;
		_firebaseService = firebaseService;
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

		_firebaseService.LogEvent("about_view", new Dictionary<string, object>
		{
			{ "screen_name", "about_page" }

		});

		// Esto aparecerá en tu terminal de VS Code aunque Firebase falle [cite: 2026-03-05]
		Console.WriteLine(">>>> FIREBASE_CHECK: Enviando evento 'about_viewed'...");

		_firebaseService.LogEvent("about_viewed", new Dictionary<string, object>
		{
			{ "user_platform", "iOS_Simulator" }
		});
	}
}