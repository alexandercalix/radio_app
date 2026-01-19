import { View, Text, Pressable, Linking } from "react-native";
import { Ionicons } from "@expo/vector-icons";
import { useRadio } from "../hooks/useRadio";
import { BeatingLogo } from "../components/BeatingLogo";
import { AdPlaceholder } from "../components/AdPlaceholder";
import { theme } from "../config/theme";
import { PlayPauseButton } from "../components/PlayButton";
import { SocialLinks } from "../components/SocialLinks";
import { SafeAreaView } from "react-native-safe-area-context";
import { LINKS } from "../config/links.config";

type Props = {
  onAbout: () => void;
};

export default function PlayerScreen({ onAbout }: Props) {
  const { play, stop, playing, loading, error } = useRadio();

  return (
    <SafeAreaView
      style={{
        flex: 1,
        padding: 5,
        backgroundColor: theme.colors.background,
      }}
    >
      <View
        style={{
          flex: 1,
          padding: 0,
        }}
      >
    <View
      style={{
        flex: 1,
        backgroundColor: theme.colors.background,
        padding: 5,
      }}
    >
      {/* Header */}
      <View
        style={{
          flexDirection: "row",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <Text
  style={{
    color: theme.colors.text,
    fontSize: 18,
    fontWeight: "600",
  }}
>
  Frontera 95.1FM
</Text>

        <Pressable
          onPress={onAbout}
          accessibilityRole="button"
          accessibilityLabel="About this radio"
          style={({ pressed }) => ({
            opacity: pressed ? 0.6 : 1,
          })}
        >
          <Ionicons
            name="information-circle-outline"
            size={28}
            color={theme.colors.text}
          />
        </Pressable>
      </View>

      {/* Content */}
      <View
        style={{
          flex: 1,
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <BeatingLogo active={playing} />
        {/* <Text
  style={{
    color: theme.colors.text,
    fontSize: 20,
    fontWeight: "600",
    marginTop: 12,
    borderBottomColor: "#ff6348",

  }}
>
  LA LEY DEL FM 
</Text> */}

<Text
  style={{
    marginTop: 15,
    marginBottom: 5,
    fontSize: 14,
    textAlign: "center",
  }}
>
  <Text style={{ color: theme.colors.muted }}>
    Desde{" "}
  </Text>

  <Text
    style={{
      color: theme.colors.primary,
      fontWeight: "700",
      letterSpacing: 0.5,
    }}
  >
    CAMASCA
  </Text>

  <Text style={{ color: theme.colors.muted }}>
    ,{" "}
  </Text>

  <Text
    style={{
      color: theme.colors.primary,
      fontWeight: "700",
      letterSpacing: 0.5,
    }}
  >
    INTIBUCÁ
  </Text>

  <Text style={{ color: theme.colors.muted }}>
    {" "}para el mundo
  </Text>
</Text>
<SocialLinks />
<Pressable
  onPress={() => Linking.openURL(LINKS.whatsapp)}
  style={{
    flexDirection: "row",
    alignItems: "center",
    marginTop: 16,
    paddingVertical: 10,
    paddingHorizontal: 18,
    borderRadius: 24,
    borderWidth: 1,
    borderColor: theme.colors.primary,
    backgroundColor: "transparent",
  }}
  accessibilityRole="button"
  accessibilityLabel="Escríbenos por WhatsApp"
>
  <Ionicons
    name="logo-whatsapp"
    size={20}
    color={theme.colors.primary}
  />
  <Text
    style={{
      color: theme.colors.primary,
      fontSize: 14,
      fontWeight: "600",
      marginLeft: 8,
    }}
  >
    Escríbenos por WhatsApp
  </Text>
</Pressable>
<Text
  style={{
    color: theme.colors.muted,
    marginTop: 15,
    fontSize: 20,
    
  }}
>
  {loading ? "Conectando…" : playing ? "En vivo" : "Detenido"}
</Text>
      </View>

      {/* Controls */}
      <View style={{ alignItems: "center", marginBottom: 16 }}>
        <PlayPauseButton
          playing={playing}
          loading={loading}
          onPlay={play}
          onStop={stop}
        />
        {error && (
  <Text
    style={{
      color: theme.colors.primary,
      marginTop: 16,
      textAlign: "center",
    }}
  >
    {error}
  </Text>
)}
      </View>

    </View>
     </View>
    </SafeAreaView>

  );
}