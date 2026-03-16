"use client";

import React from "react";
import { MdEmail, MdLock, MdVisibility } from "react-icons/md";
import { FaGoogle, FaGithub } from "react-icons/fa";
import { GrDocumentPdf } from "react-icons/gr";
import Link from "next/link";
import Header from "../components/login/Header";
import Footer from "../components/login/Footer";
import FormContainer from "../components/login/FormContainer";

export default function page() {
  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-slate-800 to-slate-900 flex flex-col items-center justify-center p-4">
      <Header />
      {/* Login Form Container */}
      <FormContainer />
      <Footer />
    </div>
  );
}
