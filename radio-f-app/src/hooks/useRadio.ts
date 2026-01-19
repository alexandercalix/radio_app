import { useEffect, useState } from "react";
import { radioPlayer, StreamError } from "../audio/RadioPlayer";
import { RADIO_CONFIG } from "../config/radio.config";
import * as Network from "expo-network";
export function useRadio() {
  const [playing, setPlaying] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
    const PLAY_TIMEOUT_MS = 8000;
  useEffect(() => {
    radioPlayer.setStatusListener(setPlaying);
  }, []);

const play = async () => {
  if (loading || playing) return;

  setLoading(true);
  setError(null);

  try {
    await withTimeout(
      radioPlayer.playWithFallback(
        RADIO_CONFIG.streams.map(s => s.url),
        RADIO_CONFIG.retryDelayMs,
        RADIO_CONFIG.maxRetriesPerStream
      ),
      PLAY_TIMEOUT_MS
    );

    setPlaying(true);
  } catch (e: any) {
    if (e.message === "STREAM_TIMEOUT") {
      setError("No se pudo conectar a la radio.");
    } else if (e instanceof StreamError) {
      setError("No se pudo conectar a la radio.");
    } else {
      setError("Error inesperado de reproducción.");
    }
  } finally {
    setLoading(false);
  }
};

const stop = async () => {
  try {
    setLoading(false);
    setPlaying(false);
    setError(null);
    await radioPlayer.stop();
  } catch {
    setLoading(false);
    setPlaying(false);
  }
};

function withTimeout<T>(promise: Promise<T>, ms: number): Promise<T> {
  return new Promise((resolve, reject) => {
    const timer = setTimeout(() => {
      reject(new Error("STREAM_TIMEOUT"));
    }, ms);

    promise
      .then(result => {
        clearTimeout(timer);
        resolve(result);
      })
      .catch(err => {
        clearTimeout(timer);
        reject(err);
      });
  });
}

  return {
    play,
    stop,
    playing,
    loading,
    error,
  };
}