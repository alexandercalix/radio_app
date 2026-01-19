import { useEffect, useState } from "react";
import { radioPlayer, StreamError } from "../audio/RadioPlayer";
import { RADIO_CONFIG } from "../config/radio.config";

export function useRadio() {
  const [playing, setPlaying] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    radioPlayer.setStatusListener(setPlaying);
  }, []);

  const play = async () => {
    if (loading || playing) return;

    setLoading(true);
    setError(null);

    try {
      await radioPlayer.playWithFallback(
        RADIO_CONFIG.streams.map(s => s.url),
        RADIO_CONFIG.retryDelayMs,
        RADIO_CONFIG.maxRetriesPerStream
      );
    } catch (e) {
      if (e instanceof StreamError) {
        setError("Unable to connect to the radio stream.");
      } else {
        setError("Unexpected playback error.");
      }
    } finally {
      setLoading(false);
    }
  };

  const stop = async () => {
    await radioPlayer.stop();
  };

  return {
    play,
    stop,
    playing,
    loading,
    error,
  };
}