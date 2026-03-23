"use client";

import { KeycloakSignIn } from "../lib/action";
import Link from "next/link";

export default function page() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 flex flex-col items-center justify-center p-4">
      <div className="bg-slate-800/50 backdrop-blur-sm rounded-lg p-8 shadow-xl border border-slate-700 max-w-md w-full">
        <h1 className="text-2xl font-bold text-white text-center mb-6">
          DocuSense'e Giriş Yap
        </h1>

        <form action={KeycloakSignIn} className="space-y-4">
          <button
            type="submit"
            className="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-3 px-4 rounded-lg transition duration-200 flex items-center justify-center gap-2"
          >
            <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
              <path
                fillRule="evenodd"
                d="M10 1L3 7v10a2 2 0 002 2h10a2 2 0 002-2V7l-7-6zM5 9a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1zm0 4a1 1 0 011-1h8a1 1 0 110 2H6a1 1 0 01-1-1z"
                clipRule="evenodd"
              />
            </svg>
            Giriş Yap
          </button>
        </form>

        <div className="mt-6 text-center">
          <p className="text-slate-400 text-sm">Don't you have an account?</p>
          <Link
            href="/register"
            className="inline-block mt-2 text-blue-400 hover:text-blue-300 font-medium transition duration-200"
          >
            Register
          </Link>
        </div>
      </div>
    </div>
  );
}
