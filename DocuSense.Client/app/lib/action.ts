"use server";

import { signIn, signOut } from "@/auth";
import { redirect } from "next/navigation";

export async function KeycloakSignIn() {
  await signIn("keycloak", { redirectTo: "/" });
}

export async function KeycloakSignOut() {
  await signOut({ redirect: false });

  const keycloakLogoutUrl = `${process.env.AUTH_KEYCLOAK_ISSUER}/protocol/openid-connect/logout?client_id=docusense-client&post_logout_redirect_uri=${encodeURIComponent("http://localhost:3000/login")}`;

  redirect(keycloakLogoutUrl);
}
