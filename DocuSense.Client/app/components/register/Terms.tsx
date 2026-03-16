import React from "react";

export default function Terms() {
  return (
    <div className="flex items-center mb-6">
      <input
        type="checkbox"
        id="terms"
        className="w-4 h-4 accent-blue-500 cursor-pointer"
      />
      <label htmlFor="terms" className="ml-3 text-slate-400 text-sm">
        I agree to the{" "}
        <a href="#" className="text-blue-400 hover:text-blue-300 transition">
          Terms and Conditions
        </a>
      </label>
    </div>
  );
}
