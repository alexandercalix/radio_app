import { useState } from "react";
import { Audio } from "expo-av";
import { RADIO_CONFIG } from "../config/radio.config";
import { radioStore } from "../audio/radioStore";
import { Platform } from "react-native";

export function useRadio() {
  const [, forceUpdate] = useState(0);
  const sync = () => forceUpdate(v => v + 1);

  const play = async () => {
  if (radioStore.loading) return;

  radioStore.loading = true;
  sync();

  try {
    if (!radioStore.sound) {
      await createSound();
    } else {
      try {
        await radioStore.sound.playAsync();
      } catch (e) {
        console.log("Sound dead, recreating…");
        await createSound(); // 🔴 CLAVE
      }
    }
  } catch (e) {
    console.log("Radio play error:", e);
    radioStore.playing = false;
  } finally {
    radioStore.loading = false;
    sync();
  }
};

const createSound = async () => {
  if (radioStore.sound) {
    try {
      await radioStore.sound.unloadAsync();
    } catch {}
    radioStore.sound = null;
  }

  const source =
    Platform.OS === "android"
      ? {
          uri: RADIO_CONFIG.streams[0].url,
          headers: {
            "Icy-MetaData": "1",
            "User-Agent": "Mozilla/5.0",
          },
        }
      : {
          uri: RADIO_CONFIG.streams[0].url,
        };

  const options =
    Platform.OS === "android"
      ? {
          shouldPlay: true,
          androidImplementation: "MediaPlayer" as const,
        }
      : {
          shouldPlay: true,
        };

  const { sound } = await Audio.Sound.createAsync(source, options);

  sound.setOnPlaybackStatusUpdate(status => {
    if (!status.isLoaded) {
      radioStore.playing = false;
      sync();
      return;
    }

    radioStore.playing =
      status.isPlaying && !status.isBuffering;
    sync();
  });

  radioStore.sound = sound;
};

  const stop = async () => {
    if (!radioStore.sound) return;

    try {
      await radioStore.sound.stopAsync();
      // ⛔ NO setPlaying aquí
      // esperamos al status update
    } catch (e) {
      console.log("Radio stop error:", e);
    }
  };

  return {
    play,
    stop,
    playing: radioStore.playing,
    loading: radioStore.loading,
    error: null,
  };
}