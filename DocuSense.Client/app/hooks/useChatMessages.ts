import { getChatMessages } from "@/services/apiService";
import { useEffect, useRef, useState } from "react";
import toast from "react-hot-toast";
import { Message } from "../types/chat";

const createMessageObj = (chatId: string, isUser: boolean, text?: string) => {
  const query: Message = {
    id: Date.now().toString() + Math.random().toString(),
    text: text ? text : "",
    chatId: chatId,
    isUser: isUser,
    createdAt: new Date().toLocaleTimeString("tr-TR", {
      hour: "2-digit",
      minute: "2-digit",
    }),
  };

  return query;
};

export const useChatMessages = (chatId: string) => {
  const [messages, setMessages] = useState<Message[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const readAiResponse = async (
    body: ReadableStream<Uint8Array<ArrayBuffer>> | null,
  ) => {
    const reader = body?.getReader();
    const decoder = new TextDecoder();
    setIsLoading(false);
    while (true) {
      const r = await reader?.read();
      if (r?.done) break; //akış bittiyse döngüden çık
      const value = decoder.decode(r?.value, { stream: true }); //Veriler byte olarak geldiğinden decoder ile bunu anlamlı kelimelere çeviriyoruz.
      setMessages((prev) =>
        prev.map((msg, index) => {
          if (index === prev.length - 1) {
            return { ...msg, text: msg.text + value };
          }
          // Son eleman değilse, önceki mesajları hiç bozmadan aynen geri dön
          return msg;
        }),
      );
    }
  };

  //Veriyi hafızada sabit tuttuğu için useRef tercih ettik
  const abortControllerRef = useRef(new AbortController());

  const addMessage = async (text: string) => {
    if (!chatId) return;
    const userMessage = createMessageObj(chatId, true, text);
    setMessages((prev) => [...prev, userMessage]);
    setIsLoading(true);

    abortControllerRef.current.abort();
    const newController = new AbortController();
    abortControllerRef.current = newController;

    try {
      const response = await fetch("/api/search", {
        body: JSON.stringify({
          query: text,
          top: 10,
          chatId: chatId,
        }),
        method: "POST",
        signal: newController.signal,
      });

      if (!response.ok) {
        setIsLoading(false);
        return;
      }

      const aiResponse = createMessageObj(chatId, false);
      setMessages((prev) => [...prev, aiResponse]);

      readAiResponse(response.body);
    } catch (error: any) {
      if (error.name === "AbortError") return;

      toast.error(error);
    }
  };

  const cancelStream = () => {
    return abortControllerRef.current.abort();
  };

  useEffect(() => {
    if (!chatId) return;
    const fetchChatMessages = async () => {
      const response = await getChatMessages(chatId);
      if (!response.success) return;
      setMessages(response.data);
    };
    fetchChatMessages();
    //Cleanup func: (unmount) veya bağımlılıkları (chatId) değiştiğinde çalışır.
    //Chat a'dan chat b'ye geçildiği anda abort devreye girer ve fetch iptal edilir.
    return () => {
      abortControllerRef.current.abort();
    };
  }, [chatId]);

  return { messages, isLoading, addMessage, cancelStream };
};
