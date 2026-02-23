using RadioApp.Views;

namespace RadioApp;

public partial class AppShell : Shell
{

	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
		Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));

	}
}
