import { create } from "zustand";

interface MessageProps {
  message: string | null;
  isLoading: boolean;
}

export const useMessageStore = create<MessageProps>((set) => ({
  message: null,
  isLoading: false,
}));
