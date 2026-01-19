import { Modal, View, Text, Pressable, Linking } from "react-native";
import { theme } from "../config/theme";

type Props = {
  visible: boolean;
  message?: string;
};

const APP_STORE_URL =
  "https://apps.apple.com/app/idXXXXXXXXX"; // luego lo cambias

export function ForceUpdateModal({ visible, message }: Props) {
  return (
    <Modal visible={visible} transparent animationType="fade">
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
            onPress={() => Linking.openURL(APP_STORE_URL)}
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