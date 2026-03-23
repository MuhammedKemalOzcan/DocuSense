import { auth } from "@/auth";
import { NextRequest, NextResponse } from "next/server";

const BASE_URL = process.env.SERVER_API_URL || process.env.NEXT_PUBLIC_API_URL;

//Server actionlar stream döndüremediğinden bir route handler oluşturuldu.
export async function POST(request: NextRequest) {
  try {
    const { query, top, chatId } = await request.json();

    const session = await auth();
    const token = session?.accessToken;
    const user = session?.user;

    console.log("SESSION:", session);
    console.log("USER:", user);

    if (!token) {
      return NextResponse.json({ error: "Yetkisiz erişim" }, { status: 401 });
    }

    const searchParams = new URLSearchParams({
      query: query || "",
      top: String(top || 10),
      chatId: chatId || "",
    });

    const apiUrl = `${BASE_URL}/Document/search?${searchParams.toString()}`;

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
    //response.body bir ReadableStream — yani veri parça parça okunabilen bir akış.
    //Bunu NextResponse'a verdiğinde Next.js bu akışı tarayıcıya aynen aktarıyor.
    //Tarayıcı da parça parça gelen veriyi chat arayüzünde gösteriyor.
    //Bu yüzden LLM cevabı yazarken kelimelerin tek tek belirdiğini görüyorsun
    return new NextResponse(response.body);
  } catch (error: any) {
    console.log("Route handler hata:", error);
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
