
using RadioApp.ViewModels;

namespace RadioApp.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.SetCurrentRoute("AboutPage");
	}
}