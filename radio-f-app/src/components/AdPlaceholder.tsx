import { View, Text } from "react-native";
import { theme } from "../config/theme";

export function AdPlaceholder() {
  return (
    <View
      style={{
        height: 50,
        width: "100%",
        backgroundColor: theme.colors.surface,
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Text style={{ color: theme.colors.muted, fontSize: 12 }}>
        Advertisement
      </Text>
    </View>
  );
}