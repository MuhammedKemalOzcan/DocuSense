"use client";

import { useParams } from "next/navigation";
import { Dispatch, SetStateAction, useState } from "react";

export default function RenameModal({
  isOpen,
  setIsOpen,
  initialTitle,
  chatId,
  onUpdate,
}: {
  isOpen: boolean;
  setIsOpen: Dispatch<SetStateAction<boolean>>;
  initialTitle: string; // Eklendi
  chatId: string; // Eklendi
  onUpdate: (id: string, title: string) => Promise<void>;
}) {
  const [title, setTitle] = useState<string>(initialTitle);

  const handleUpdate = () => {
    if (title.trim() && chatId) {
      onUpdate(chatId, title);
    }
  };

  return (
    <div className="flex flex-col items-center justify-center">
      {isOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
          {/* Arka Plan Karartma */}
          <div
            className="absolute inset-0 bg-slate-900/40 backdrop-blur-sm transition-opacity"
            onClick={() => setIsOpen(false)}
          />

          {/* Modal İçeriği */}
          <div className="relative w-full max-w-sm bg-white rounded-2xl shadow-2xl p-6 animate-in fade-in zoom-in duration-200">
            <h3 className="text-lg font-bold text-slate-800">
              Bu sohbeti yeniden adlandırın
            </h3>

            <div className="mt-4">
              <label className="text-xs font-semibold text-slate-500 uppercase tracking-wider">
                Yeni Başlık
              </label>
              <input
                type="text"
                onChange={(e) => setTitle(e.target.value)}
                autoFocus
                className="mt-1 w-full px-4 py-2 bg-slate-100 border-transparent focus:bg-white focus:ring-2 focus:ring-indigo-500 rounded-lg outline-none transition-all text-slate-700"
              />
            </div>

            {/* Aksiyon Butonları */}
            <div className="mt-6 flex justify-end gap-3">
              <button
                onClick={() => setIsOpen(false)}
                className="px-4 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-100 rounded-lg transition"
              >
                İptal
              </button>
              <button
                onClick={handleUpdate}
                className="px-4 py-2 text-sm font-semibold text-white bg-indigo-600 hover:bg-indigo-700 rounded-lg transition shadow-md"
              >
                Yeniden Adlandır
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
