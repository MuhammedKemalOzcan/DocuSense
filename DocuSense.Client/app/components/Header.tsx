import React from "react";

export default function Header() {
  return (
    <div className="border-b border-gray-800 px-8 py-4 flex items-center justify-between bg-gray-950">
      <div className="flex items-center gap-3">
        <div className="w-2 h-2 bg-green-500 rounded-full"></div>
        <span className="text-gray-400 text-sm font-semibold uppercase tracking-wider">
          Ready to Analyze
        </span>
      </div>
      <div className="flex items-center gap-4">
        <button className="flex items-center gap-2 px-4 py-2 border border-gray-700 rounded-lg text-gray-300 text-sm hover:bg-gray-800">
          <span>↗️</span>
          <span>Share Session</span>
        </button>
        <button className="text-gray-400 hover:text-white text-xl">🔔</button>
      </div>
    </div>
  );
}
