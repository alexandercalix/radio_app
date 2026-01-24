import { View, Text, Pressable, Image, Linking } from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { SafeAreaView } from "react-native-safe-area-context";
import { theme } from "../config/theme";
import { LINKS } from "../config/links.config";
type Props = {
  onBack: () => void;
};

export default function AboutScreen({ onBack }: Props) {
  return (
    <SafeAreaView
      style={{
        flex: 1,
        backgroundColor: theme.colors.background,
      }}
    >
      <View
        style={{
          flex: 1,
          paddingHorizontal: 20,
          paddingTop: 10,
        }}
      >
        {/* Header */}
        <View
          style={{
            flexDirection: "row",
            alignItems: "center",
            marginBottom: 24,
          }}
        >
          <Pressable
            onPress={onBack}
            accessibilityRole="button"
            accessibilityLabel="Volver"
            style={({ pressed }) => ({
              marginRight: 12,
              opacity: pressed ? 0.6 : 1,
            })}
          >
            <Ionicons
              name="arrow-back"
              size={26}
              color={theme.colors.text}
            />
          </Pressable>

          <Text
            style={{
              color: theme.colors.text,
              fontSize: 20,
              fontWeight: "600",
            }}
          >
            Acerca de
          </Text>
        </View>

        {/* Content */}
        <View style={{ alignItems: "center" }}>
          {/* Logo */}
          <Image
            source={require("../../assets/logo.png")}
            style={{ width: 140, height: 140 }}
            resizeMode="contain"
          />

          {/* Brand */}
          <Text
            style={{
              marginTop: 16,
              fontSize: 18,
              fontWeight: "600",
              color: theme.colors.text,
              textAlign: "center",
            }}
          >
            Frontera 95.1 FM
          </Text>

          <Text
            style={{
              marginTop: 4,
              fontSize: 14,
              color: theme.colors.muted,
              textAlign: "center",
            }}
          >
            La Ley del FM
          </Text>

          <Text
            style={{
              marginTop: 8,
              fontSize: 14,
              color: theme.colors.muted,
              textAlign: "center",
            }}
          >
            Desde Camasca, Intibucá para el mundo
          </Text>

          {/* Description */}
          <Text
            style={{
              marginTop: 24,
              fontSize: 14,
              lineHeight: 20,
              color: theme.colors.muted,
              textAlign: "center",
            }}
          >
            Frontera 95.1 FM es una radio en vivo que transmite
            programación musical, informativa y cultural para
            Honduras y el mundo.
          </Text>

          {/* Location */}
          <Pressable
            onPress={() =>
              Linking.openURL(
                "https://maps.app.goo.gl/bv3QbaQvWq972ZSz5"
              )
            }
            style={{ marginTop: 20 }}
          >
            <Text
              style={{
                fontSize: 14,
                color: theme.colors.primary,
                textAlign: "center",
              }}
            >
              📍 Ver ubicación en el mapa
            </Text>
          </Pressable>

          {/* Social links */}
          {/* Social links */}
<View
  style={{
    flexDirection: "row",
    marginTop: 20,
    gap: 20,
  }}
>
  <Pressable onPress={() => Linking.openURL(LINKS.website)}>
    <Ionicons
      name="globe-outline"
      size={24}
      color={theme.colors.text}
    />
  </Pressable>

  <Pressable onPress={() => Linking.openURL(LINKS.facebook)}>
    <Ionicons
      name="logo-facebook"
      size={24}
      color={theme.colors.text}
    />
  </Pressable>

  <Pressable onPress={() => Linking.openURL(LINKS.instagram)}>
    <Ionicons
      name="logo-instagram"
      size={24}
      color={theme.colors.text}
    />
  </Pressable>

  <Pressable onPress={() => Linking.openURL(LINKS.tiktok)}>
    <Ionicons
      name="logo-tiktok"
      size={24}
      color={theme.colors.text}
    />
  </Pressable>
</View>

          {/* Developer credit */}
          <Text
            style={{
              marginTop: 32,
              fontSize: 12,
              color: theme.colors.muted,
              textAlign: "center",
            }}
          >
            Desarrollo de la aplicación
          </Text>

          <Pressable
            onPress={() =>
              Linking.openURL(
                "https://www.linkedin.com/in/oscarcalixnolasco/"
              )
            }
          >
            <Text
              style={{
                marginTop: 4,
                fontSize: 13,
                color: theme.colors.primary,
                textAlign: "center",
              }}
            >
              Oscar Calix · SCSE
            </Text>
          </Pressable>

          {/* Footer */}
          <Text
            style={{
              marginTop: 24,
              fontSize: 12,
              color: theme.colors.muted,
              textAlign: "center",
            }}
          >
            © Radio Frontera HN · v1.0.0
          </Text>
        </View>
      </View>
    </SafeAreaView>
  );
}