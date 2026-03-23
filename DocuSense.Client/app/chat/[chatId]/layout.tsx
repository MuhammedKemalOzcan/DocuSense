import Sidebar from "@/app/components/Sidebar";
import { auth } from "@/auth";
import React from "react";

export default async function layout({
  children,
}: {
  children: React.ReactNode;
}) {
  const session = await auth();
  const user = session?.user;

  return (
    <div className="flex h-screen bg-gray-900">
      {user && <Sidebar user={user} />}
      <div className="flex flex-col w-full h-screen">{children}</div>
    </div>
  );
}
