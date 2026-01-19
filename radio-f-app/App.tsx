import { useState } from "react";
import PlayerScreen from "./src/screens/PlayerScreen";
import AboutScreen from "./src/screens/AboutScreen";
import { BootScreen } from "./src/screens/BootScreen";
import { SafeAreaProvider } from "react-native-safe-area-context";
import { useRemoteConfig } from "./src/hooks/useRemoteConfig";
import { isVersionLower } from "./src/utils/version";
import { ForceUpdateModal } from "./src/components/ForceUpdateModal";

type Screen = "boot" | "player" | "about";
const APP_VERSION = "1.1.0";

export default function App() {
  const [screen, setScreen] = useState<Screen>("boot");
  const { config } = useRemoteConfig();

const mustUpdate =
  config?.app.forceUpdate === true &&
  isVersionLower(APP_VERSION, config.app.minVersion);

  function isVersionLower(current: string, min: string) {
  const c = current.split(".").map(Number);
  const m = min.split(".").map(Number);

  for (let i = 0; i < Math.max(c.length, m.length); i++) {
    const cv = c[i] ?? 0;
    const mv = m[i] ?? 0;

    if (cv < mv) return true;
    if (cv > mv) return false;
  }

  return false; // iguales
}

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