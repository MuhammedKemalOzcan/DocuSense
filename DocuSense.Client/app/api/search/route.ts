import { getSession } from "@/app/lib/session";
import { NextRequest, NextResponse } from "next/server";

export async function POST(request: NextRequest) {
  try {
    const { query, top, chatId } = await request.json();
    const token = await getSession();

    if (!token) {
      return NextResponse.json({ error: "Yetkisiz erişim" }, { status: 401 });
    }

    const searchParams = new URLSearchParams({
      query: query || "",
      top: String(top || 10),
      chatId: chatId || "",
    });

    const apiUrl = `https://localhost:7100/api/Document/search?${searchParams.toString()}`;

    const response = await fetch(apiUrl, {
      headers: {
        Authorization: `Bearer ${token}`,
        Accept: "text/event-stream",
      },
    });

    if (!response.ok) {
      const errorText = await response.text();
      return new NextResponse(errorText, { status: response.status });
    }

    return new NextResponse(response.body);
  } catch (error: any) {
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
