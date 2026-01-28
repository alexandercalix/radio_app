import { useEffect, useState } from "react";
import { Audio } from "expo-av";
import * as Network from "expo-network";
import { RADIO_CONFIG } from "../config/radio.config";
import { Platform } from "react-native";

let sound: Audio.Sound | null = null;

export function useRadio() {
  const [playing, setPlaying] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  let wasPlayingBeforeDisconnect = false;

  // 🔌 Network listener: fuerza stop si se cae internet
useEffect(() => {
  const sub = Network.addNetworkStateListener(state => {
    if (!state.isConnected) {
      if (playing) {
        wasPlayingBeforeDisconnect = true;
        forceStop("Sin conexión a internet");
      }
    } else {
      // 🔁 Internet volvió
      if (wasPlayingBeforeDisconnect) {
        wasPlayingBeforeDisconnect = false;
        play(); // 👈 intento ÚNICO de reconexión
      }
    }
  });

  return () => sub.remove();
}, [playing]);

  const createAndPlay = async () => {
    // Limpieza defensiva
    if (sound) {
      try {
        await sound.unloadAsync();
      } catch {}
      sound = null;
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
        : { uri: RADIO_CONFIG.streams[0].url };

    const options =
      Platform.OS === "android"
        ? {
            shouldPlay: true,
            androidImplementation: "MediaPlayer" as const,
          }
        : { shouldPlay: true };

    const result = await Audio.Sound.createAsync(source, options);
    sound = result.sound;

    sound.setOnPlaybackStatusUpdate(status => {
      if (!status.isLoaded) {
        setPlaying(false);
        return;
      }

      // Android puede decir "isPlaying=true" aunque no suene aún
      setPlaying(status.isPlaying && !status.isBuffering);
    });
  };

  const play = async () => {
    if (loading || playing) return;

    setLoading(true);
    setError(null);

    const net = await Network.getNetworkStateAsync();
    if (!net.isConnected) {
      setError("Sin conexión a internet");
      setLoading(false);
      return;
    }

    try {
      if (!sound) {
        await createAndPlay();
      } else {
        try {
          await sound.playAsync();
        } catch {
          // Android a veces mata el player internamente
          await createAndPlay();
        }
      }
    } catch (e) {
      console.log("Radio play error:", e);
      setError("No se pudo conectar a la radio.");
      setPlaying(false);
    } finally {
      setLoading(false);
    }
  };

  const forceStop = async (msg?: string) => {
    console.log("Force stopping radio", msg ? `with message: ${msg}` : "");
    try {
      if (sound) {
        try {
          await sound.stopAsync();
          await sound.unloadAsync();
        } catch {}
        sound = null;
      }
    } finally {
      setPlaying(false);
      setLoading(false);
      if (msg) setError(msg);
    }
  };

const stop = async () => {
  wasPlayingBeforeDisconnect = false; // ⛔ corta reconexión
  await forceStop();
};

  return {
    play,
    stop,
    playing,
    loading,
    error,
  };
}