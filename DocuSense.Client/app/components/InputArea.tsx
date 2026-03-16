"use client";

import { useState } from "react";
import { FiSend } from "react-icons/fi";

interface InputProps {
  onSendMessage: (value: string) => void;
}

export default function InputArea({ onSendMessage }: InputProps) {
  const [value, setValue] = useState<string>("");

  const handleSubmit = () => {
    if (value.trim() !== "") {
      onSendMessage(value);
    }
    setValue("");
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSubmit();
    }
  };

  return (
    <div className="border-t border-gray-800 px-8 py-4 bg-gray-950">
      <div className="flex gap-3">
        <input
          onKeyDown={handleKeyDown}
          type="text"
          value={value}
          placeholder="Ask a question about your PDF..."
          onChange={(e) => setValue(e.target.value)}
          className="flex-1 bg-gray-800 border border-gray-700 rounded-lg px-4 py-3 text-gray-200 placeholder-gray-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
        />
        <button
          onClick={handleSubmit}
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-3 rounded-lg font-semibold flex items-center gap-2"
        >
          <span>
            <FiSend />
          </span>
        </button>
      </div>
      <p className="text-gray-500 text-xs mt-2">
        SUGGESTED: SUMMARIZE DOCUMENT • LIST ACTION ITEMS • EXTRACT TABLES
      </p>
    </div>
  );
}
