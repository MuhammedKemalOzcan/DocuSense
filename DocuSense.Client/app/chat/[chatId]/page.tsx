"use client";

import ChatArea from "@/app/components/ChatArea";
import InputArea from "@/app/components/InputArea";
import { useChatMessages } from "@/app/hooks/useChatMessages";
import { useParams } from "next/navigation";

export default function page() {
  const params = useParams();
  const chatId = params.chatId as string;

  const { isLoading, messages, addMessage } = useChatMessages(chatId);

  return (
    <div className="flex h-full w-full bg-gray-900">
      <div className="flex-1 flex flex-col bg-gray-900">
        <ChatArea isLoading={isLoading} messages={messages} />
        <InputArea onSendMessage={addMessage} />
      </div>
    </div>
  );
}
