import {
  getChats,
  IngestDocument,
  removeChat,
  updateChat,
} from "@/services/apiService";
import { Chat, UpdateChatTitle } from "../types/chat";
import { useRouter } from "next/navigation";
import { useChatStore } from "../stores/ChatStore";
import { useShallow } from "zustand/shallow";

export const useDocuments = () => {
  const {
    chatList,
    setChatList,
    setUploading,
    removeChatById,
    updateChatTitleById,
  } = useChatStore(
    useShallow((state) => ({
      chatList: state.chatList,
      setChatList: state.setChatList,
      setUploading: state.setUploading,
      removeChatById: state.removeChatById,
      updateChatTitleById: state.updateChatTitleById,
    })),
  );
  const router = useRouter();

  const uploadFile = async (file: File) => {
    setUploading(true, "uploading");
    const formData = new FormData();
    formData.append("file", file);
    const response = await IngestDocument(formData);

    if (!response.success) {
      console.log(response.error);

      setUploading(false, "fail");
      return;
    }
    setUploading(false, "success");

    const { id } = response.data;
    if (id) router.push(`/chat/${id}`);
  };

  const getDocuments = async () => {
    const response = await getChats();
    if (!response.success) {
      return { error: response.error, success: response.success };
    }
    setChatList(response.data);
    return { success: true };
  };

  const deleteChat = async (id: string) => {
    const response = await removeChat(id);
    if (!response.success) {
      return { error: response.error, success: response.success };
    }
    removeChatById(id);
    return { success: true };
  };

  const updateChatTitle = async (updatedChat: UpdateChatTitle) => {
    const response = await updateChat(updatedChat);
    if (!response.success) {
      return { error: response.error, success: response.success };
    }
    const { title, id } = response.data;
    updateChatTitleById(id, title);
    return { success: true, title: title };
  };

  return { uploadFile, getDocuments, chatList, deleteChat, updateChatTitle };
};
