import Link from "next/link";
import React from "react";
import { GrDocumentPdf } from "react-icons/gr";

export default function Header() {
  return (
    <div className="absolute top-0 left-0 right-0 p-6 flex items-center justify-between">
      <div className="flex items-center gap-2">
        <div className="w-8 h-8 bg-blue-500 rounded flex items-center justify-center">
          <GrDocumentPdf className="text-white text-sm" />
        </div>
        <span className="text-white font-semibold text-lg">DocuSense</span>
      </div>
      <nav className="flex items-center gap-8">
        <a href="#" className="text-slate-300 hover:text-white transition">
          Product
        </a>
        <a href="#" className="text-slate-300 hover:text-white transition">
          Pricing
        </a>
        <a href="#" className="text-slate-300 hover:text-white transition">
          About
        </a>
        <Link
          href="/register"
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-2 rounded-lg transition font-medium"
        >
          Sign Up
        </Link>
      </nav>
    </div>
  );
}
