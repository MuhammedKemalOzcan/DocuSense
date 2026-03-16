import Header from "@/app/components/Header";
import Sidebar from "@/app/components/Sidebar";
import React from "react";

export default function layout({ children }: { children: React.ReactNode }) {
  return (
    <div className="flex h-screen bg-gray-900">
      <Sidebar />
      <div className="flex flex-col w-full h-screen">
        {children}
      </div>
    </div>
  );
}
