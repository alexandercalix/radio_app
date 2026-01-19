// src/models/radio.model.ts

export type RadioStreamConfig = {
  url: string;
  label: string;
};

export type RadioConfig = {
  name: string;
  streams: RadioStreamConfig[];
  retryDelayMs: number;
  maxRetriesPerStream: number;
};