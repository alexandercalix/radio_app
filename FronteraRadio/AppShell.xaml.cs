using FronteraRadio.Views;

namespace FronteraRadio;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
	}
}
