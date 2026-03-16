import { NextRequest, NextResponse } from "next/server";
import { getSession } from "./app/lib/session";

export async function middleware(request: NextRequest) {
  const token = await getSession();
  const authRoutes = ["/login", "/register"];
  const isAuthPage = authRoutes.includes(request.nextUrl.pathname);

  if (!token && !isAuthPage) {
    return NextResponse.redirect(new URL("/login", request.url));
  }
  if (token && isAuthPage) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return NextResponse.next();
}

//Yıldız işareti, kendisinden önce gelen parametrenin (:path) hiç olmayabileceğini
//  veya sonsuz sayıda derinliğe sahip olabileceğini belirtir.
export const config = {
  matcher: ["/", "/chat/:path*", "/login", "/register"],
};
