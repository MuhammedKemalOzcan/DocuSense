export interface Message {
  id: string;
  chatId: string | null;
  text: string;
  isUser: boolean;
  createdAt: string;
}

export interface Chat {
  id: string;
  title: string;
  documentId: string;
  createdAt: string;
}

export type UploadStatus = "initial" | "uploading" | "success" | "fail";

export interface UpdateChatTitle {
  id: string;
  title: string;
}
