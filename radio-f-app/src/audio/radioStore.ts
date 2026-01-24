import { Audio } from "expo-av";

class RadioStore {
  sound: Audio.Sound | null = null;
  playing = false;
  loading = false;
}

export const radioStore = new RadioStore();