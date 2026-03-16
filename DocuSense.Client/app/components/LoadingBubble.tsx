import { PulseLoader } from "react-spinners";

export default function LoadingBubble() {
  return (
    <div className="flex gap-4 mb-4">
      {/* AI Avatarı - MessageBubble ile birebir aynı */}
      <div className="w-8 h-8 bg-gray-700 rounded-full flex items-center justify-center text-white text-sm shrink-0">
        🤖
      </div>

      {/* Yükleme Balonu */}
      <div className="max-w-2xl">
        <div className="bg-gray-800 rounded-lg px-6 py-4 flex items-center justify-center">
          {/* FadeLoader varsayılan olarak biraz büyük gelir, 
              içine tam oturması için CSS scale ile biraz küçültüyoruz */}
          <div className="scale-[0.6] h-8 flex items-center">
            <PulseLoader color="#9CA3AF" />
          </div>
        </div>
        <p className="text-left text-gray-500 text-xs mt-1">AI yanıtlıyor...</p>
      </div>
    </div>
  );
}
