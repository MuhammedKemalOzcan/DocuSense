"use client";
import React, { useEffect, useRef } from "react"
import MessageBubble from "./MessageBubble";
import { Message } from "../types/chat";
import LoadingBubble from "@/app/components/LoadingBubble";

export default function ChatArea({
  messages,
  isLoading,
}: {
  messages: Message[];
  isLoading: boolean;
}) {
  const messagesEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages, isLoading]);

  return (
    <div className="flex-1 overflow-y-auto px-8 py-6 space-y-6 relative">
      {messages.length === 0 ? (
        <div className="absolute top-1/2 left-1/3">
          Merhaba Ne Merak Ediyorsun?
        </div>
      ) : (
        messages.map((message) => (
          <div key={message.id}>
            <MessageBubble message={message} />
          </div>
        ))
      )}

      {isLoading && <LoadingBubble />}
      <div ref={messagesEndRef} />
    </div>
  );
}
