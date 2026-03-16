import React from "react";

export default function Footer() {
  return (
    <div className="absolute bottom-0 left-0 right-0 p-6 flex justify-between text-xs text-slate-500 border-t border-slate-700">
      <p>© 2024 DocuSense Inc. All rights reserved.</p>
      <div className="flex gap-6">
        <a href="#" className="hover:text-slate-300 transition">
          Privacy Policy
        </a>
        <a href="#" className="hover:text-slate-300 transition">
          Terms of Service
        </a>
      </div>
    </div>
  );
}
