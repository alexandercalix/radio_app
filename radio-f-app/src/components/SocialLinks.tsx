import { View, Pressable, Linking } from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { theme } from "../config/theme";
import { LINKS } from "../config/links.config";

export function SocialLinks() {
  const open = (url: string) => {
    Linking.openURL(url);
  };

  return (
    <View
      style={{
        flexDirection: "row",
        justifyContent: "center",
        marginTop: 16,
        gap: 16,
      }}
    >
      <Pressable onPress={() => open(LINKS.website)}>
        <Ionicons name="globe-outline" size={26} color={theme.colors.text} />
      </Pressable>

      <Pressable onPress={() => open(LINKS.facebook)}>
        <Ionicons name="logo-facebook" size={26} color={theme.colors.text} />
      </Pressable>

      <Pressable onPress={() => open(LINKS.instagram)}>
        <Ionicons name="logo-instagram" size={26} color={theme.colors.text} />
      </Pressable>

      <Pressable onPress={() => open(LINKS.tiktok)}>
        <Ionicons name="logo-tiktok" size={26} color={theme.colors.text} />
      </Pressable>
    </View>
  );
}