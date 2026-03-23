import { FaGithub, FaGoogle } from "react-icons/fa";
import Form from "./Form";
import Terms from "./Terms";
import Link from "next/link";

export default function FormContainer() {
  return (
    <div className="w-full max-w-md">
      <div className="bg-slate-800 rounded-lg p-8 shadow-2xl border border-slate-700">
        <h1 className="text-center text-2xl font-bold text-white mb-2">
          Create your account
        </h1>
        <p className="text-center text-slate-400 text-sm mb-8">
          Join us to start analyzing PDFs with AI
        </p>
        <Form />
        <Terms />

        <div className="relative mb-6">
          <div className="absolute inset-0 flex items-center">
            <div className="w-full border-t border-slate-600"></div>
          </div>
          <div className="relative flex justify-center text-sm">
            <span className="px-2 bg-slate-800 text-slate-400 text-xs font-medium">
              OR SIGN UP WITH
            </span>
          </div>
        </div>
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
        <p className="text-center text-slate-400 text-sm">
          Already have an account?{" "}
          <Link
            href="/login"
            className="text-blue-400 hover:text-blue-300 font-medium transition"
          >
            Sign In
          </Link>
        </p>
      </div>
    </div>
  );
}
