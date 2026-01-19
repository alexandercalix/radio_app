import { View, Text, ActivityIndicator, Image, Pressable } from "react-native";
import NetInfo from "@react-native-community/netinfo";
import { useEffect, useState } from "react";
import { theme } from "../config/theme";

export function BootScreen({ onReady }: { onReady: () => void }) {
  const [checking, setChecking] = useState(true);
  const [offline, setOffline] = useState(false);

  const checkNetwork = async () => {
    setChecking(true);
    const state = await NetInfo.fetch();
    setOffline(!state.isConnected);
    setChecking(false);

    if (state.isConnected) {
      onReady();
    }
  };

  useEffect(() => {
    checkNetwork();
  }, []);

  return (
    <View
      style={{
        flex: 1,
        backgroundColor: theme.colors.background,
        justifyContent: "center",
        alignItems: "center",
        padding: 24,
      }}
    >
      <Image
        source={require("../../assets/logo.png")}
        style={{ width: 180, height: 180, marginBottom: 24 }}
        resizeMode="contain"
      />

      {checking && (
        <>
          <ActivityIndicator color={theme.colors.primary} />
          <Text style={{ color: theme.colors.muted, marginTop: 12 }}>
            Checking connection…
          </Text>
        </>
      )}

      {!checking && offline && (
        <>
          <Text
            style={{
              color: theme.colors.text,
              textAlign: "center",
              marginBottom: 16,
            }}
          >
            No internet connection.
            {"\n"}Please connect to play the radio.
          </Text>

          <Pressable
            onPress={checkNetwork}
            style={{
              backgroundColor: theme.colors.primary,
              paddingHorizontal: 24,
              paddingVertical: 12,
              borderRadius: 10,
            }}
          >
            <Text style={{ color: theme.colors.text, fontWeight: "600" }}>
              Retry
            </Text>
          </Pressable>
        </>
      )}
    </View>
  );
}