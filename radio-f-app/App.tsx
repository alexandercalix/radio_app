import { useEffect, useState } from "react";
import { SafeAreaProvider } from "react-native-safe-area-context";

import PlayerScreen from "./src/screens/PlayerScreen";
import AboutScreen from "./src/screens/AboutScreen";
import { BootScreen } from "./src/screens/BootScreen";
import { useRemoteConfig } from "./src/hooks/useRemoteConfig";
import { isVersionLower } from "./src/utils/version";
import { ForceUpdateModal } from "./src/components/ForceUpdateModal";
import { Audio } from "expo-av";
type Screen = "boot" | "player" | "about";
const APP_VERSION = "1.1.0";

export default function App() {
  const [screen, setScreen] = useState<Screen>("boot");
  const { config } = useRemoteConfig();

useEffect(() => {
  console.log("Setting audio mode");
  Audio.setAudioModeAsync({
    allowsRecordingIOS: false,
    staysActiveInBackground: true,
    playsInSilentModeIOS: true,
    shouldDuckAndroid: true,
  });
}, []);

  const mustUpdate =
    config?.app.forceUpdate === true &&
    isVersionLower(APP_VERSION, config.app.minVersion);

  return (
    <SafeAreaProvider>
      {!mustUpdate && screen === "boot" && (
        <BootScreen onReady={() => setScreen("player")} />
      )}

      {!mustUpdate && screen === "player" && (
        <PlayerScreen onAbout={() => setScreen("about")} />
      )}

      {!mustUpdate && screen === "about" && (
        <AboutScreen onBack={() => setScreen("player")} />
      )}

      <ForceUpdateModal
        visible={!!mustUpdate}
        message={config?.app.message}
      />
    </SafeAreaProvider>
  );
}