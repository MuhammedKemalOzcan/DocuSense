import { create } from "zustand";
import { Chat, UploadStatus } from "../types/chat";

interface ChatState {
  chatList: Chat[] | [];
  chat: Chat | null;
  isUploading: boolean;
  setChats: (newChat: Chat) => void;
  setChatList: (chats: Chat[]) => void;
  removeChatById: (id: string) => void;
  status: UploadStatus;
  setUploading: (isUploading: boolean, status?: UploadStatus) => void;
  updateChatTitleById: (id: string, newTitle: string) => void
}

export const useChatStore = create<ChatState>((set) => ({
  chat: null,
  chatList: [],
  isUploading: false,
  status: "initial",
  setChats: (newChat: Chat) =>
    set((state) => ({
      chatList: [newChat, ...state.chatList],
    })),
  setUploading: (isUploading: boolean, status?: UploadStatus) =>
    set({ isUploading: isUploading, status: status }),
  setChatList: (chats: Chat[]) =>
    set({
      chatList: chats,
    }),
  removeChatById: (id: string) =>
    set((state) => ({
      chatList: state.chatList.filter((chat) => chat.id !== id),
    })),
  updateChatTitleById: (id: string, newTitle: string) =>
    set((state) => ({
      chatList: state.chatList.map((chat) =>
        chat.id === id ? { ...chat, title: newTitle } : chat,
      ),
    })),
}));
