import { SignInFormState } from "@/app/lib/definition";
import { signIn } from "@/services/auth";
import React, { useActionState } from "react";
import { MdEmail, MdLock, MdVisibility } from "react-icons/md";

export default function SigninForm() {
  const initialState: SignInFormState = { errors: {}, message: null };
  const [state, action, pending] = useActionState(signIn, initialState);
  return (
    <form action={action}>
      {state?.message && (
        <div className="mb-4 p-3 bg-red-500/20 border border-red-500 text-red-300 rounded">
          {state.message}
        </div>
      )}
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
            id="email"
            name="email"
            type="email"
            placeholder="name@company.com"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-4 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
        </div>
      </div>
      {state?.errors?.email && <p>{state.errors.email}</p>}
      <div className="mb-6">
        <div className="flex items-center justify-between mb-2">
          <label
            htmlFor="password"
            className="block text-slate-300 text-sm font-medium"
          >
            Password
          </label>
          <a
            href="#"
            className="text-blue-400 text-sm hover:text-blue-300 transition"
          >
            Forgot password?
          </a>
        </div>
        {state?.errors?.password && <p>{state.errors.password}</p>}

        <div className="relative">
          <MdLock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-slate-400 text-lg" />
          <input
            id="password"
            name="password"
            type="password"
            placeholder="••••••••"
            className="w-full bg-slate-700 border border-slate-600 rounded-lg py-3 pl-10 pr-10 text-white placeholder-slate-500 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500 transition"
          />
          <button className="absolute right-3 top-1/2 transform -translate-y-1/2 text-slate-400 hover:text-slate-300">
            <MdVisibility className="text-lg" />
          </button>
        </div>
      </div>
      <button
        type="submit"
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 rounded-lg transition mb-6"
      >
        Sign In
      </button>
    </form>
  );
}
