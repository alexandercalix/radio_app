export type RadioStreamConfig = {
  primaryUrl: string;
  fallbackUrl?: string;
  name: string;
};

//

export const RADIO_CONFIG = {
  name: "Frontera 95.1FM",
  streams: [
    {
      url: "https://stream-176.zeno.fm/0uwm0hb5u0hvv?zt=eyJhbGciOiJIUzI1NiJ9.eyJzdHJlYW0iOiIwdXdtMGhiNXUwaHZ2IiwiaG9zdCI6InN0cmVhbS0xNzYuemVuby5mbSIsInJ0dGwiOjUsImp0aSI6Im9VOHRNSUZDU0dpY2dWeHk4RXFaZ0EiLCJpYXQiOjE3NjkyOTMyODgsImV4cCI6MTc2OTI5MzM0OH0.n1DlQzmag5HNU179zARU1QelvNIAcPHtdWWqgbAe-a0",
      label: "Primary",
    },
    
  ],
  retryDelayMs: 1500,
  maxRetriesPerStream: 1,
};