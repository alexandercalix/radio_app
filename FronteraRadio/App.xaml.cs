using FronteraRadio.Core.Interfaces;

namespace FronteraRadio;

public partial class App : Application
{
	private readonly IFirebaseService _firebaseService;

	public App(IFirebaseService firebaseService)
	{
		InitializeComponent();
		_firebaseService = firebaseService;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override void OnStart()
	{

	}
}