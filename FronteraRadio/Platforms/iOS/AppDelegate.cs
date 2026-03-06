using AVFoundation;
using Foundation;
using UIKit;

namespace FronteraRadio;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	public override bool FinishedLaunching(UIApplication app, NSDictionary options)
	{
		var session = AVFoundation.AVAudioSession.SharedInstance();
		session.SetCategory(AVAudioSessionCategory.Playback, AVAudioSessionCategoryOptions.DefaultToSpeaker);
		session.SetActive(true);

		Firebase.Core.App.Configure();

		return base.FinishedLaunching(app, options);
	}
}
