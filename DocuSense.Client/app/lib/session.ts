"use server";

import { cookies } from "next/headers";

export async function createSession(token: string, expires: number) {
  if (!token) return;
  const cookieStore = await cookies();
  const expiresAt = new Date(Date.now() + expires * 1000);

  cookieStore.set("session", token, {
    httpOnly: true,
    secure: false,
    expires: expiresAt,
    sameSite: "lax",
    path: "/",
  });
}

export async function deleteSession() {
  const cookieStore = await cookies();
  cookieStore.delete("session");
}
