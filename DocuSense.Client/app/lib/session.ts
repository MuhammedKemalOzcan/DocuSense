"use server";

import { cookies } from "next/headers";

export async function createSession(token: string, expires: string) {
  if (!token) return;
  const cookieStore = await cookies();
  const expiresAt = new Date(expires);

  cookieStore.set("session", token, {
    httpOnly: true,
    secure: false,
    expires: expiresAt,
    sameSite: "lax",
    path: "/",
  });
}

export async function getSession() {
  return (await cookies()).get("session")?.value;
}

export async function deleteSession() {
  const cookieStore = await cookies();
  cookieStore.delete("session");
}
