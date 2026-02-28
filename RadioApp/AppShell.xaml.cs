using RadioApp.Views;

namespace RadioApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Only register routes for pages you navigate TO from the Player
		Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
		Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));
	}

}