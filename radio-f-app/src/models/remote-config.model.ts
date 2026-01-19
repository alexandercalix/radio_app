// src/models/remote-config.model.ts

import { RadioConfig } from "./radio.models";

export type RemoteAppConfig = {
  version: string;
  minVersion: string;
  forceUpdate: boolean;
  message?: string;
  storeUrls?: {
    ios?: string;
    android?: string;
  };
};

export type RemoteRadioConfig = {
  app: RemoteAppConfig;
  radio: RadioConfig;
  links: {
    website: string;
    facebook: string;
    instagram: string;
    tiktok: string;
  };
};