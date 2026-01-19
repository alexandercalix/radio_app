import AsyncStorage from "@react-native-async-storage/async-storage";
import { useEffect, useState } from "react";
import type { RemoteRadioConfig } from "../models/remote-config.model";

const STORAGE_KEY = "REMOTE_CONFIG_CACHE";
const CACHE_TTL_MS = 7 * 24 * 60 * 60 * 1000;
const REMOTE_CONFIG_URL =
  "https://alexandercalix.github.io/radio_app/remote-config/config.json";

export function useRemoteConfig() {
  const [config, setConfig] = useState<RemoteRadioConfig | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadConfig = async () => {

      try {
        const cached = await AsyncStorage.getItem(STORAGE_KEY);

        if (cached) {
          const parsed = JSON.parse(cached);
          const age = Date.now() - parsed.fetchedAt;

         

          setConfig(parsed.data);

          const isExpired = age > CACHE_TTL_MS;

          if (!isExpired) {
           
            setLoading(false);
            return;
          }

          
        } else {
         
        }

        console.log("🌐 RemoteConfig: fetching from server");
        const res = await fetch(REMOTE_CONFIG_URL);
        const data = await res.json();

        await AsyncStorage.setItem(
          STORAGE_KEY,
          JSON.stringify({
            data,
            fetchedAt: Date.now(),
          })
        );

        console.log("💾 RemoteConfig: saved new cache");
        setConfig(data);
      } catch (e) {
        console.log("❌ RemoteConfig error", e);
      } finally {
        setLoading(false);
        console.log("🔚 RemoteConfig: done");
      }
    };

    loadConfig();
  }, []);

  return { config, loading };
}