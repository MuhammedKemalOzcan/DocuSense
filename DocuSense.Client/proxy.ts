import { NextResponse } from "next/server";
import { auth } from "@/auth";

export const proxy = auth((req) => {
  const authRoutes = ["/login", "/register"];
  const isAuthPage = authRoutes.includes(req.nextUrl.pathname);

  if (!req.auth && !isAuthPage) {
    return NextResponse.redirect(new URL("/login", req.url));
  }
  if (req.auth && isAuthPage) {
    return NextResponse.redirect(new URL("/", req.url));
  }
  return NextResponse.next();
});

//Yıldız işareti, kendisinden önce gelen parametrenin (:path) hiç olmayabileceğini
//  veya sonsuz sayıda derinliğe sahip olabileceğini belirtir.
export const config = {
  matcher: ["/", "/chat/:path*", "/login", "/register"],
};
