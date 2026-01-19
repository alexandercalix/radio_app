export type RadioStreamConfig = {
  primaryUrl: string;
  fallbackUrl?: string;
  name: string;
};

//

export const RADIO_CONFIG = {
  name: "My Radio Station",
  streams: [
    {
      url: "https://stream-154.zeno.fm/0uwm0hb5u0hvv?zt=eyJhbGciOiJIUzI1NiJ9.eyJzdHJlYW0iOiIwdXdtMGhiNXUwaHZ2IiwiaG9zdCI6InN0cmVhbS0xNTQuemVuby5mbSIsInJ0dGwiOjUsImp0aSI6IktEMk1Gdi1MUmsydWdLdzcwcHlqQ1EiLCJpYXQiOjE3Mzc0NzQ0NzksImV4cCI6MTczNzQ3NDUzOX0.P2gpnjCFi4NEualcn8HNh2Nn8_Ih053YZInQI_WdiCo",
      label: "Primary",
    },
    
  ],
  retryDelayMs: 1500,
  maxRetriesPerStream: 1,
};