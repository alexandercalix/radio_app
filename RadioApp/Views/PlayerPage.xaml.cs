using RadioApp.Core.Enums;
using RadioApp.Core.Interfaces;
using RadioApp.ViewModels;
using CommunityToolkit.Maui.Views;

namespace RadioApp.Views;

public partial class PlayerPage : ContentPage
{
	private bool _isAnimating;
	private MediaElement _hiddenPlayer;
	private readonly IAudioService _audioService;

	public PlayerPage(PlayerViewModel vm, IAudioService audioService)
	{
		InitializeComponent();
		BindingContext = vm;
		vm.SetCurrentRoute("PlayerPage");

		_audioService = audioService;

		_audioService.StateChanged += state =>
		{
			MainThread.BeginInvokeOnMainThread(() =>
			{
				if (state == PlayerState.Playing)
					_ = StartBounce();
				else
					StopBounce();
			});
		};

		this.Loaded += OnPageLoaded;
	}

	// MAKE SURE THIS IS ASYNC
	private async void OnPageLoaded(object sender, EventArgs e)
	{

	}

	private async Task StartBounce()
	{
	}

	private void StopBounce()
	{

	}
}