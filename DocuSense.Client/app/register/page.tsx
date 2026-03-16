import React from "react";
import { MdEmail, MdLock, MdVisibility, MdPerson } from "react-icons/md";
import { FaGoogle, FaGithub } from "react-icons/fa";
import { GrDocumentPdf } from "react-icons/gr";
import Link from "next/link";
import Header from "../components/register/Header";
import FormContainer from "../components/register/FormContainer";

export default function Page() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 flex flex-col items-center justify-center p-4">
      {/* Header */}
      <Header />

      {/* Register Form Container */}
      <FormContainer />

      {/* Footer */}
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
    </div>
  );
}
