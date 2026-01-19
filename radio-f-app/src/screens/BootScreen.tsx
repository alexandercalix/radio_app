import { View, Animated, Image, Easing } from "react-native";
import { useEffect, useRef } from "react";
import { theme } from "../config/theme";

type Props = {
  onReady: () => void;
};

export function BootScreen({ onReady }: Props) {
  const scale = useRef(new Animated.Value(0.7)).current;
  const opacity = useRef(new Animated.Value(0)).current;

  useEffect(() => {
    Animated.sequence([
      // Fade + zoom in
      Animated.parallel([
        Animated.timing(opacity, {
          toValue: 1,
          duration: 1200,
          useNativeDriver: true,
        }),
        Animated.timing(scale, {
          toValue: 1.05,
          duration: 1800,
          easing: Easing.out(Easing.cubic),
          useNativeDriver: true,
        }),
      ]),

      // Settle back (cinematic overshoot)
      Animated.timing(scale, {
        toValue: 1,
        duration: 400,
        easing: Easing.out(Easing.quad),
        useNativeDriver: true,
      }),
    ]).start();

    const timer = setTimeout(onReady, 2600);
    return () => clearTimeout(timer);
  }, []);

  return (
    <View
      style={{
        flex: 1,
        backgroundColor: theme.colors.background,
        justifyContent: "center",
        alignItems: "center",
      }}
    >
      <Animated.View
        style={{
          opacity,
          transform: [{ scale }],
        }}
      >
        <Image
          source={require("../../assets/logo.png")}
          style={{ width: 190, height: 190 }}
          resizeMode="contain"
        />
      </Animated.View>
    </View>
  );
}