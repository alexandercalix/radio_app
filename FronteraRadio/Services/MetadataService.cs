using FronteraRadio.Core.Interfaces;

#if ANDROID
using Android.Graphics;
using Android.Media;
using Microsoft.Maui.ApplicationModel;
#endif

#if IOS
using MediaPlayer;
using UIKit;
#endif

namespace FronteraRadio.Services;

public class MetadataService : IMetadataService
{
    public void UpdateMetadata(string title, string artist, string unusedName)
    {
#if ANDROID
    var context = Microsoft.Maui.ApplicationModel.Platform.AppContext;
    
    // Android usa el BMP (androiddash) [cite: 2026-03-02]
    int resId = context.Resources.GetIdentifier("androiddash", "drawable", context.PackageName);
    
    if (resId != 0)
    {
        var bitmap = Android.Graphics.BitmapFactory.DecodeResource(context.Resources, resId);
        var metadata = new Android.Media.MediaMetadata.Builder()
            .PutString(Android.Media.MediaMetadata.MetadataKeyTitle, title)
            .PutString(Android.Media.MediaMetadata.MetadataKeyArtist, artist)
            .PutBitmap(Android.Media.MediaMetadata.MetadataKeyAlbumArt, bitmap)
            .Build();
        // Aquí conectas con tu MediaSession de Android [cite: 2026-03-02]
    }
#endif

#if IOS
        // iOS usa el PNG cuadrado (iosartwork) [cite: 2026-03-02]
        var image = UIKit.UIImage.FromBundle("iosartwork");
        if (image != null)
        {
            var artwork = new MediaPlayer.MPMediaItemArtwork(image);
            var info = new MediaPlayer.MPNowPlayingInfo
            {
                Title = title,
                Artist = artist,
                Artwork = artwork,
                IsLiveStream = true
            };
            MediaPlayer.MPNowPlayingInfoCenter.DefaultCenter.NowPlaying = info;
        }
#endif
    }
}