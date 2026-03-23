"use client";

import { SignUpFormState } from "@/app/lib/definition";
import { signUp } from "@/services/auth";
import { useActionState, useState } from "react";
import { MdEmail, MdLock, MdPerson } from "react-icons/md";
import ShowPassword from "./ShowPassword";

export default function Form() {
  const initialState: SignUpFormState = { errors: {}, message: null };
  const [state, action, pending] = useActionState(signUp, initialState);
  const [showPassword, setShowPassword] = useState<boolean>(false);

  return (
    <form action={action}>
      {state?.message && (
        <div className="mb-4 p-3 bg-red-500/20 border border-red-500 text-red-300 rounded">
          {state.message}
        </div>
      )}
      <div className="mb-5">
        <label
          htmlFor="firstName"
          className="block text-slate-300 text-sm font-medium mb-2"
        >
          First Name
        </label>
        <div className="relative">
          <MdPerson className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 text-lg" />
          <input
            name="firstName"
            id="firstName"
            type="text"
            placeholder="John"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-4 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>
        {state.errors?.firstName && (
          <p className="text-sm text-red-500">{state.message}</p>
        )}
      </div>
      <div className="mb-5">
        <label
          htmlFor="lastName"
          className="block text-slate-300 text-sm font-medium mb-2"
        >
          Last Name
        </label>
        <div className="relative">
          <MdPerson className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 text-lg" />
          <input
            name="lastName"
            id="lastName"
            type="text"
            placeholder="Doe"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-4 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>
        {state.errors?.lastName && (
          <p className="text-sm text-red-500">{state.message}</p>
        )}
      </div>

      {/* Email Field */}
      <div className="mb-5">
        <label
          htmlFor="email"
          className="block text-slate-300 text-sm font-medium mb-2"
        >
          Email address
        </label>
        <div className="relative">
          <MdEmail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 text-lg" />
          <input
            name="email"
            id="email"
            type="email"
            placeholder="name@company.com"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-4 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>
        {state.errors?.email && (
          <p className="text-sm text-red-500">{state.message}</p>
        )}
      </div>

      {/* Password Field */}
      <div className="mb-6">
        <label
          htmlFor="password"
          className="block text-slate-300 text-sm font-medium mb-2"
        >
          Password
        </label>
        <div className="relative">
          <MdLock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 text-lg" />
          <input
            id="password"
            name="password"
            type={showPassword ? "text" : "password"}
            placeholder="••••••••"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-10 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
          <ShowPassword
            showPassword={showPassword}
            setShowPassword={setShowPassword}
          />
        </div>
        {state.errors?.password && (
          <p className="text-sm text-red-500">{state.message}</p>
        )}
      </div>
      <button
        type="submit"
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 rounded-lg transition mb-6"
      >
        Get Started →
      </button>
    </form>
  );
}
