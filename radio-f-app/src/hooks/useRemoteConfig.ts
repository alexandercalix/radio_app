import AsyncStorage from "@react-native-async-storage/async-storage";
import { useEffect, useState } from "react";
import type { RemoteRadioConfig } from "../models/remote-config.model";

const STORAGE_KEY = "REMOTE_CONFIG_CACHE";
const CACHE_TTL_MS = 7 * 24 * 60 * 60 * 1000;
const REMOTE_CONFIG_URL = "https://TU_URL/config.json";

export function useRemoteConfig() {
  const [config, setConfig] = useState<RemoteRadioConfig | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadConfig = async () => {
      try {
        const cached = await AsyncStorage.getItem(STORAGE_KEY);

        if (cached) {
          const parsed = JSON.parse(cached);
          setConfig(parsed.data);

          const isExpired =
            Date.now() - parsed.fetchedAt > CACHE_TTL_MS;

          if (!isExpired) {
            setLoading(false);
            return;
          }
        }

        const res = await fetch(REMOTE_CONFIG_URL);
        const data = await res.json();

        await AsyncStorage.setItem(
          STORAGE_KEY,
          JSON.stringify({
            data,
            fetchedAt: Date.now(),
          })
        );

        setConfig(data);
      } catch (e) {
        console.log("Remote config error", e);
      } finally {
        setLoading(false);
      }
    };

    loadConfig();
  }, []);

  return { config, loading };
}