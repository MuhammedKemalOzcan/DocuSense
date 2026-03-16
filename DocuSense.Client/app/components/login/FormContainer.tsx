import React from "react";
import { FaGithub, FaGoogle } from "react-icons/fa";
import { GrDocumentPdf } from "react-icons/gr";
import SigninForm from "./SigninForm";
import Link from "next/link";

export default function FormContainer() {
  return (
    <div className="w-full max-w-md">
      <div className="bg-slate-800 rounded-lg p-8 shadow-2xl border border-slate-700">
        {/* Logo */}
        <div className="flex justify-center mb-6">
          <div className="w-12 h-12 bg-blue-500 rounded-lg flex items-center justify-center">
            <GrDocumentPdf className="text-white text-2xl" />
          </div>
        </div>

        {/* Title */}
        <h1 className="text-center text-2xl font-bold text-white mb-2">
          Welcome back
        </h1>
        <p className="text-center text-slate-400 text-sm mb-8">
          Sign in to your account to continue
        </p>
        <SigninForm />
        {/* Divider */}
        <div className="relative mb-6">
          <div className="absolute inset-0 flex items-center">
            <div className="w-full border-t border-slate-600"></div>
          </div>
          <div className="relative flex justify-center text-sm">
            <span className="px-2 bg-slate-800 text-slate-400 text-xs font-medium">
              OR CONTINUE WITH
            </span>
          </div>
        </div>

        {/* Social Login Buttons */}
        <div className="grid grid-cols-2 gap-4 mb-6">
          <button className="bg-slate-700 hover:bg-slate-600 text-white border border-slate-600 rounded-lg py-3 transition flex items-center justify-center gap-2 font-medium">
            <FaGoogle className="text-lg" />
            Google
          </button>
          <button className="bg-slate-700 hover:bg-slate-600 text-white border border-slate-600 rounded-lg py-3 transition flex items-center justify-center gap-2 font-medium">
            <FaGithub className="text-lg" />
            GitHub
          </button>
        </div>

        {/* Sign Up Link */}
        <p className="text-center text-slate-400 text-sm">
          Don't have an account?{" "}
          <Link
            href="/register"
            className="text-blue-400 hover:text-blue-300 font-medium transition"
          >
            Sign up
          </Link>
        </p>
      </div>
    </div>
  );
}
