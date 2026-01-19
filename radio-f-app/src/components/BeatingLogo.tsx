import { useEffect, useRef } from "react";
import { Animated, Image } from "react-native";

type Props = {
  active: boolean;
};

export function BeatingLogo({ active }: Props) {
  const scale = useRef(new Animated.Value(1)).current;
  const animationRef = useRef<Animated.CompositeAnimation | null>(null);

  useEffect(() => {
    // STOP animation when inactive
    if (!active) {
      animationRef.current?.stop();
      animationRef.current = null;
      scale.setValue(1); // reset size
      return;
    }

    // CREATE a new animation when active
    animationRef.current = Animated.loop(
      Animated.sequence([
        Animated.timing(scale, {
          toValue: 1.08,
          duration: 500,
          useNativeDriver: true
        }),
        Animated.timing(scale, {
          toValue: 1,
          duration: 500,
          useNativeDriver: true,
        }),
      ])
    );

    animationRef.current.start();

    // Cleanup on unmount
    return () => {
      animationRef.current?.stop();
      animationRef.current = null;
    };
  }, [active, scale]);

  return (
    <Animated.View style={{ transform: [{ scale }] }}>
      <Image
        source={require("../../assets/logo.png")}
        style={{ width: 200, height: 200 }}
        resizeMode="contain"
      />
    </Animated.View>
  );
}