"use client";

import Sidebar from "./components/Sidebar";

export default function Home() {
  return (
    <div className="flex h-screen w-full bg-gray-900">
      <Sidebar />
      <div className="flex-1 flex items-center justify-center bg-gray-900">
        <h1 className="text-center">
          Hoş Geldiniz, başlamak için soldan bir PDF seçin veya yeni yükleyin
        </h1>
      </div>
    </div>
  );
}
