declare global {
  namespace NodeJS {
    interface ProcessEnv {
      API_BASE_URL: string;
      NODE_ENV: 'development' | 'production';
    }
  }
}

// If this file has no imports/exports, turn it into a module
export {};