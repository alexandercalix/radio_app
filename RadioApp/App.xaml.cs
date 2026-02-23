using Microsoft.Extensions.DependencyInjection;
using RadioApp.Views;
#if IOS
using UIKit;
#endif

namespace RadioApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

#if IOS
		UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
#endif

		MainPage = new IntroPage();
	}


}