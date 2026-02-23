using RadioApp.Core.Enums;
using RadioApp.Core.Interfaces;
using RadioApp.ViewModels;

namespace RadioApp.Views;

public partial class PlayerPage : ContentPage
{
	private bool _isAnimating;


	public PlayerPage(PlayerViewModel vm, IAudioService audioService)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.SetCurrentRoute("PlayerPage");
		audioService.AttachPlayer(HiddenPlayer);

		audioService.StateChanged += state =>
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (state == PlayerState.Playing)
					_ = StartBounce();
				else
					StopBounce();
			});
		};
	}

	private async Task StartBounce()
	{
		if (_isAnimating)
			return;

		_isAnimating = true;

		while (_isAnimating)
		{
			await LogoImage.ScaleTo(1.1, 400, Easing.SinInOut);
			await LogoImage.ScaleTo(1.0, 400, Easing.SinInOut);
		}
	}

	private void StopBounce()
	{
		_isAnimating = false;
		LogoImage.ScaleTo(1.0, 200);
	}
}