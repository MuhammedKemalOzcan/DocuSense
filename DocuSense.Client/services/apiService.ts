import { apiClient } from "@/app/lib/apiClient";
import { Chat, Message, UpdateChatTitle } from "@/app/types/chat";

export async function IngestDocument(file: FormData) {
  return await apiClient<Chat>("/Document/ingest", {
    body: file,
    method: "POST",
  });
}
export async function getChats() {
  return await apiClient<Chat[]>("/Document");
}
export async function getChatMessages(chatId: string) {
  return await apiClient<Message[]>(`/Document/${chatId}/messages`, {
    method: "GET",
  });
}

export async function removeChat(chatId: string) {
  return await apiClient<string>(`/Document/delete/${chatId}`, {
    method: "DELETE",
  });
}

export async function updateChat(updateChat: UpdateChatTitle) {
  return await apiClient<Chat>(`/Document/update`, {
    body: updateChat,
    method: "PUT",
  });
}
