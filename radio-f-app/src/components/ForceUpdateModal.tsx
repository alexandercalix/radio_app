import { Modal, View, Text, Pressable, Linking, Platform } from "react-native";
import { theme } from "../config/theme";

type Props = {
  visible: boolean;
  message?: string;
  storeUrls?: {
    ios?: string;
    android?: string;
  };
};

export function ForceUpdateModal({
  visible,
  message,
  storeUrls,
}: Props) {
  if (!visible || !storeUrls) return null;

  const storeUrl =
    Platform.OS === "ios"
      ? storeUrls.ios
      : storeUrls.android;

  if (!storeUrl) return null;

  return (
    <Modal visible transparent animationType="fade">
      <View
        style={{
          flex: 1,
          backgroundColor: "rgba(0,0,0,0.6)",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <View
          style={{
            width: "85%",
            backgroundColor: theme.colors.background,
            borderRadius: 12,
            padding: 20,
          }}
        >
          <Text
            style={{
              fontSize: 18,
              fontWeight: "600",
              color: theme.colors.text,
              textAlign: "center",
              marginBottom: 12,
            }}
          >
            Actualización requerida
          </Text>

          <Text
            style={{
              fontSize: 14,
              color: theme.colors.muted,
              textAlign: "center",
              marginBottom: 20,
            }}
          >
            {message ??
              "Hay una nueva versión disponible para continuar usando la aplicación."}
          </Text>

          <Pressable
            onPress={() => Linking.openURL(storeUrl)}
            style={{
              backgroundColor: theme.colors.primary,
              paddingVertical: 12,
              borderRadius: 8,
            }}
          >
            <Text
              style={{
                color: "#fff",
                fontSize: 16,
                fontWeight: "600",
                textAlign: "center",
              }}
            >
              Actualizar
            </Text>
          </Pressable>
        </View>
      </View>
    </Modal>
  );
}