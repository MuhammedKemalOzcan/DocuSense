import React from "react";
import { ClipLoader } from "react-spinners";
import { CSSProperties } from "react";

// Props için tip tanımı
interface LoadingUIProps {
  isLoading: boolean;
  message?: string;
  color?: string;
  size?: number;
}

export const LoadingUI: React.FC<LoadingUIProps> = ({
  isLoading,
  message = "Yükleniyor...",
  color = "#36d7b7",
  size = 50,
}) => {
  if (!isLoading) return null;

  return (
    <div style={styles.overlay}>
      <div style={styles.container}>
        <ClipLoader color={color} size={size} speedMultiplier={1} />
        {message && <span style={styles.text}>{message}</span>}
      </div>
    </div>
  );
};

// CSS-in-JS için tip güvenli stil objesi
const styles: { [key: string]: CSSProperties } = {
  overlay: {
    position: "fixed",
    top: 0,
    left: 0,
    width: "100vw",
    height: "100vh",
    backgroundColor: "rgba(255, 255, 255, 0.75)",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    zIndex: 1000,
    backdropFilter: "blur(2px)", // Modern bir görünüm için hafif bulanıklık
  },
  container: {
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
    gap: "1rem",
  },
  text: {
    fontFamily: "sans-serif",
    fontSize: "1.1rem",
    color: "#444",
    fontWeight: 600,
  },
};

export default LoadingUI;
