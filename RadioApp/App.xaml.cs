namespace RadioApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();


		// Just load the Shell immediately. The OS handles the splash screen before this!
		MainPage = new AppShell();
	}
}