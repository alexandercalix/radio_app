import { Pressable, ActivityIndicator } from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { theme } from "../config/theme";

type Props = {
  playing: boolean;
  loading: boolean;
  onPlay: () => void;
  onStop: () => void;
};

export function PlayPauseButton({
  playing,
  loading,
  onPlay,
  onStop,
}: Props) {
  return (
    <Pressable
      onPress={playing ? onStop : onPlay}
      disabled={loading}
      accessibilityRole="button"
        accessibilityLabel={
    playing ? "Detener reproducción" : "Reproducir radio en vivo"
  }
  accessibilityHint="Control de reproducción de la radio"
      style={({ pressed }) => ({
        width: 88,
        height: 88,
        borderRadius: 44,
        backgroundColor: "#c0392b",
        justifyContent: "center",
        alignItems: "center",
        opacity: pressed ? 0.85 : 1,
        shadowColor: "#000",
        shadowOffset: { width: 0, height: 6 },
        shadowOpacity: 0.3,
        shadowRadius: 8,
        elevation: 6, // Android shadow
      })}
    >
      {loading ? (
        <ActivityIndicator color={theme.colors.text} size="large" />
      ) : (
        <Ionicons
          name={playing ? "stop" : "play"}
          size={42}
          color={"#ecf0f1"}
          style={{ marginLeft: playing ? 0 : 4 }} // optical centering for play icon
        />
      )}
    </Pressable>
  );
}