//Sunucu tarafında çalışırken SERVER_API_URL (http://api:8080/api) mevcuttur, onu kullanır.
"use server";
import { auth } from "@/auth";

//Tarayıcıda çalışırken SERVER_API_URL undefined olur, NEXT_PUBLIC_API_URL (http://localhost:8080/api) devreye girer.
const BASE_URL = process.env.SERVER_API_URL || process.env.NEXT_PUBLIC_API_URL;

interface FetchOptions extends RequestInit {
  body?: any;
}

export type ApiResponse<T> =
  | { success: true; data: T }
  | { success: false; error: string; statusCode?: number };

export async function apiClient<T>(
  endpoint: string,
  options: FetchOptions = {},
): Promise<ApiResponse<T>> {
  try {
    const session = await auth();
    const token = session?.accessToken;

    const headers: Record<string, string> = {
      "Content-Type": "application/json",
      ...(options.headers as Record<string, string>),
    };

    if (token) headers["Authorization"] = `Bearer ${token}`;

    // Eğer gelen veri standart bir obje ise otomatik JSON'a çevir
    if (
      options.body &&
      typeof options.body === "object" &&
      !(options.body instanceof FormData)
    ) {
      options.body = JSON.stringify(options.body);
    }

    if (options.body instanceof FormData) {
      delete headers["Content-Type"];
    }

    const response = await fetch(`${BASE_URL}${endpoint}`, {
      ...options,
      headers,
    });

    if (!response.ok) {
      if (response.status === 401) {
        if (typeof window !== "undefined") {
          window.location.href = "/login";
        }
        return {
          success: false,
          error: "Oturum süreniz doldu. Lütfen tekrar giriş yapın.",
          statusCode: 401,
        };
      }

      let errorMessage = "Sunucu ile iletişim kurulamadı.";

      const errorData = await response.text();
      const text = JSON.parse(errorData);
      errorMessage = text.message || text.title || errorMessage;

      return {
        success: false,
        error: errorMessage,
        statusCode: response.status,
      };
    }

    const textResponse = await response.text();
    if (!textResponse) {
      return { success: true, data: null as T };
    }
    return { success: true, data: JSON.parse(textResponse) };
  } catch (error: any) {
    // 4. Ağ (Network) kopması gibi en dipten gelen try-catch hataları
    return {
      success: false,
      error: error.message || "Bilinmeyen bir ağ hatası oluştu.",
    };
  }
}
