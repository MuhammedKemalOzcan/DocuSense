"use client"; // Hata kalkanları her zaman Client Component olmalıdır

import { useEffect } from "react";

export default function Error({
  error,
  reset,
}: {
  error: Error & { digest?: string };
  reset: () => void;
}) {
  useEffect(() => {
    // İleride buraya Sentry gibi bir loglama servisi ekleyip hataları sunucuya bildirebiliriz
    console.error("Sohbet ekranında kritik bir çökme yaşandı:", error);
  }, [error]);

  return (
    <div className="flex flex-col items-center justify-center h-full w-full bg-gray-900 text-white p-4">
      <div className="bg-red-900/20 border border-red-500/30 p-8 rounded-2xl flex flex-col items-center text-center max-w-md">
        {/* Şık bir hata ikonu */}
        <svg className="w-16 h-16 text-red-500 mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
        </svg>
        
        <h2 className="text-2xl font-bold mb-2">Eyvah, bağlantı koptu!</h2>
        <p className="text-gray-400 mb-6">
          Bu sohbeti yüklerken beklenmeyen bir hata oluştu. Sunucu yanıt vermiyor veya veri yapısı bozulmuş olabilir.
        </p>
        
        {/* Next.js'in bize sunduğu sihirli 'reset' fonksiyonu */}
        <button
          onClick={() => reset()}
          className="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-lg transition-colors font-medium"
        >
          Yeniden Dene
        </button>
      </div>
    </div>
  );
}