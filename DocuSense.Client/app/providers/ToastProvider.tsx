"use client";

import { Toaster } from "react-hot-toast";

export default function ToastProvider() {
  return (
    <Toaster
      position="bottom-right"
      toastOptions={{
        duration: 4000,
        style: {
          background: "#333",
          color: "#fff",
          padding: `12px`,
        },
        success: {
          duration: 3000,
          iconTheme: {
            primary: "green",
            secondary: "blue",
          },
        },
      }}
    />
  );
}
