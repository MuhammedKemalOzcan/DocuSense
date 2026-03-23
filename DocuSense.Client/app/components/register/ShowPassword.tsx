import React from "react";
import { MdVisibility } from "react-icons/md";

export default function ShowPassword({
  showPassword,
  setShowPassword,
}: {
  showPassword: boolean;
  setShowPassword: React.Dispatch<React.SetStateAction<boolean>>;
}) {
  return (
    <div>
      <button
        type="button"
        onClick={() => setShowPassword(!showPassword)}
        className="absolute right-3 top-1/2 transform -translate-y-1/2 text-slate-400 hover:text-slate-300"
      >
        <MdVisibility className="text-lg" />
      </button>
    </div>
  );
}
