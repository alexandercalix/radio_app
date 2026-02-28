namespace RadioApp.Views;

public partial class IntroPage : ContentPage
{
	private bool _started;
	private CancellationTokenSource _animationCts;

	public IntroPage()
	{
		InitializeComponent();
		_animationCts = new CancellationTokenSource();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (_started) return;
		_started = true;

		try
		{
			// Small delay to ensure layout is calculated
			await Task.Delay(50, _animationCts.Token);

			var screenHeight = this.Height;
			if (screenHeight <= 0)
				screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;

			// Start logo above screen
			Logo.TranslationY = -screenHeight * 0.4;
			Logo.Scale = 0.5;
			Logo.Opacity = 1;

			// Gradient animation
			var gradientAnim = new Animation(v =>
			{
				if (!_animationCts.IsCancellationRequested)
				{
					BgGradient.StartPoint = new Point(0, v * 0.2);
					BgGradient.EndPoint = new Point(1, 1 - v * 0.2);
				}
			}, 0, 1);

			gradientAnim.Commit(this, "GradientShift", 16, 1200, Easing.SinInOut);

			var colorAnim = new Animation(t =>
			{
				if (!_animationCts.IsCancellationRequested)
				{
					// Ensure you are using Color.FromRgba here to avoid iOS color parsing crashes
					Stop1.Color = AdjustBrightness(Color.FromRgba(20, 30, 48, 255), 0.15 * t); // #141E30
					Stop2.Color = AdjustBrightness(Color.FromRgba(36, 59, 85, 255), 0.10 * t); // #243B55
					Stop3.Color = AdjustBrightness(Color.FromRgba(221, 94, 59, 255), 0.08 * t); // #DD5E3B
				}
			}, 0, 1);

			colorAnim.Commit(this, "ColorShift", 16, 1500, Easing.SinInOut);

			// Logo animation
			var logoAnim = new Animation(v =>
			{
				if (!_animationCts.IsCancellationRequested)
				{
					Logo.TranslationY = -screenHeight * 0.4 * (1 - v);
					Logo.Scale = 0.5 + (0.6 * v);
				}
			}, 0, 1);

			logoAnim.Commit(this, "LogoAnim", 16, 1200, Easing.CubicOut);

			// Wait for animations
			await Task.Delay(1300, _animationCts.Token);
			if (_animationCts.IsCancellationRequested) return;

			await Logo.ScaleToAsync(1.0, 150, Easing.CubicInOut);
			if (_animationCts.IsCancellationRequested) return;

			await Task.Delay(600, _animationCts.Token);
			if (_animationCts.IsCancellationRequested) return;

			// Fade the page out
			await this.FadeToAsync(0, 250);

			// THE MAGIC FIX: Give the iOS animation engine a fraction of a second to fully release the view
			await Task.Delay(100, _animationCts.Token);
		}
		catch (TaskCanceledException)
		{
			// Animation was cancelled, exit gracefully
			return;
		}
		finally
		{
			// Clean up animations FIRST
			this.AbortAnimation("GradientShift");
			this.AbortAnimation("ColorShift");
			this.AbortAnimation("LogoAnim");

			if (!_animationCts.IsCancellationRequested)
			{
				// Dispatcher completely separates this action from the animation lifecycle
				Dispatcher.Dispatch(() =>
				{
					Application.Current.MainPage = new AppShell();
				});
			}

			_animationCts?.Dispose();
			_animationCts = null;
		}
	}

	private Color AdjustBrightness(Color color, double amount)
	{
		return new Color(
			Convert.ToSingle(Math.Min(color.Red + amount, 1)),
			Convert.ToSingle(Math.Min(color.Green + amount, 1)),
			Convert.ToSingle(Math.Min(color.Blue + amount, 1))
		);
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		// Cancel any ongoing animations
		_animationCts?.Cancel();

		this.AbortAnimation("GradientShift");
		this.AbortAnimation("ColorShift");
		this.AbortAnimation("LogoAnim");
	}
}