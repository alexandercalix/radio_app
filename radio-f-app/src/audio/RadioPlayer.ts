import { Audio, AVPlaybackStatus } from "expo-av";

type StatusListener = (playing: boolean) => void;

export class StreamError extends Error {}

class RadioPlayer {
  private sound: Audio.Sound | null = null;
  private statusListener?: StatusListener;

  setStatusListener(listener: StatusListener) {
    this.statusListener = listener;
  }

  private notify(playing: boolean) {
    this.statusListener?.(playing);
  }

  async configureAudio() {
    await Audio.setAudioModeAsync({
      staysActiveInBackground: true,
      playsInSilentModeIOS: true,
      shouldDuckAndroid: true,
    });
  }

  private async tryPlay(url: string) {
    const { sound } = await Audio.Sound.createAsync(
      { uri: url },
      { shouldPlay: true },
      this.onStatusUpdate
    );

    this.sound = sound;
  }

  async playWithFallback(streams: string[], retryDelay: number, retries: number) {
    await this.configureAudio();

    for (const url of streams) {
      let attempts = 0;

      while (attempts <= retries) {
        try {
          await this.tryPlay(url);
          this.notify(true);
          return; // ✅ success
        } catch (e) {
          attempts++;
          await this.cleanup();
          if (attempts <= retries) {
            await new Promise(r => setTimeout(r, retryDelay));
          }
        }
      }
    }

    throw new StreamError("All radio streams failed.");
  }

  async stop() {
    await this.cleanup();
    this.notify(false);
  }

  private async cleanup() {
    if (this.sound) {
      try {
        await this.sound.stopAsync();
        await this.sound.unloadAsync();
      } catch {}
      this.sound = null;
    }
  }

  private onStatusUpdate = (status: AVPlaybackStatus) => {
    if (!status.isLoaded) {
      this.notify(false);
      return;
    }

    this.notify(status.isPlaying);
  };
}

export const radioPlayer = new RadioPlayer();