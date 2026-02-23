namespace RadioApp.Views;

public partial class IntroPage : ContentPage
{
	public IntroPage()
	{
		InitializeComponent();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await Task.Delay(50);

		var screenHeight = this.Height;

		// Start logo above screen
		Logo.TranslationY = -screenHeight * 0.4;
		Logo.Scale = 0.5;
		Logo.Opacity = 1;

		// Gradient animation
		var gradientAnim = new Animation(v =>
{
	BgGradient.StartPoint = new Point(0, v * 0.2);
	BgGradient.EndPoint = new Point(1, 1 - v * 0.2);
}, 0, 1);

		gradientAnim.Commit(this, "GradientShift", 16, 1200, Easing.SinInOut);

		var colorAnim = new Animation(t =>
{
	Stop1.Color = AdjustBrightness(Color.FromArgb("#141E30"), 0.15 * t);
	Stop2.Color = AdjustBrightness(Color.FromArgb("#243B55"), 0.10 * t);
	Stop3.Color = AdjustBrightness(Color.FromArgb("#DD5E3B"), 0.08 * t);
}, 0, 1);

		colorAnim.Commit(this, "ColorShift", 16, 1500, Easing.SinInOut);

		// Logo animation
		var logoAnim = new Animation();

		logoAnim.Add(0, 1, new Animation(v =>
		{
			Logo.TranslationY = -screenHeight * 0.4 * (1 - v);
			Logo.Scale = 0.5 + (0.6 * v);
		}));

		// Run both simultaneously
		var parent = new Animation();
		parent.Add(0, 1, gradientAnim);
		parent.Add(0, 1, logoAnim);

		parent.Commit(this, "IntroCombined", 16, 1200, Easing.CubicOut);

		await Task.Delay(1300);

		await Logo.ScaleTo(1.0, 150, Easing.CubicInOut);

		await Task.Delay(600);

		await this.FadeTo(0, 250);

		var shell = new AppShell();
		shell.Opacity = 0;
		Application.Current.MainPage = shell;

		await Task.Delay(30);
		await shell.FadeTo(1, 250);
	}

	private Color LerpColor(Color from, Color to, double t)
	{
		return new Color(
			Convert.ToSingle(from.Red + (to.Red - from.Red) * t),
			Convert.ToSingle(from.Green + (to.Green - from.Green) * t),
			Convert.ToSingle(from.Blue + (to.Blue - from.Blue) * t)
		);
	}

	private Color AdjustBrightness(Color color, double amount)
	{
		return new Color(
			Convert.ToSingle(Math.Min(color.Red + amount, 1)),
			Convert.ToSingle(Math.Min(color.Green + amount, 1)),
			Convert.ToSingle(Math.Min(color.Blue + amount, 1))
		);
	}
}