"use client";

import FileUploadDropZone from "./FileUploadDropZone";
import { useDocuments } from "../hooks/useDocuments";
import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import { UpdateChatTitle } from "../types/chat";
import RenameModal from "./RenameModal";
import { User } from "next-auth";
import UserProfile from "./sidebar/UserProfile";
import ChatListItem from "./sidebar/ChatListItem";

export default function Sidebar({ user }: { user: User }) {
  const { uploadFile, getDocuments, updateChatTitle } = useDocuments();
  const [openMenuId, setOpenMenuId] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedChat, setSelectedChat] = useState<{
    id: string;
    title: string;
  } | null>(null);

  useEffect(() => {
    getDocuments();
  }, []);

  console.log(user);

  useEffect(() => {
    const handleClickOutside = () => setOpenMenuId(null);
    if (openMenuId) {
      window.addEventListener("click", handleClickOutside);
    }
    return () => window.removeEventListener("click", handleClickOutside);
  }, [openMenuId]);

  const handleRenameSubmit = async (id: string, title: string) => {
    setIsModalOpen(true);
    const updatedChat: UpdateChatTitle = { id, title };
    const response = await updateChatTitle(updatedChat);
    if (response.error) {
      toast.error(response.error);
    }

    if (response.success) {
      toast.success(`Chat title changed to: ${response.title}`);
      setIsModalOpen(false);
      setSelectedChat(null);
    }
  };

  return (
    <div className="w-80 bg-gray-950 border-r border-gray-800 flex flex-col p-6 flex-shrink-0">
      {/* Logo and Title */}
      <div className="flex items-center gap-3 mb-8">
        <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center text-white font-bold">
          📄
        </div>
        <div>
          <h1 className="text-white font-bold text-lg">PDF Assistant</h1>
          <p className="text-gray-500 text-xs">v1.0 Enterprise</p>
        </div>
      </div>
      <FileUploadDropZone onUploadFile={uploadFile} />
      <ChatListItem
        openMenuId={openMenuId}
        setOpenMenuId={setOpenMenuId}
        setSelectedChat={setSelectedChat}
        setIsModalOpen={setIsModalOpen}
      />
      <UserProfile user={user} />
      {isModalOpen && selectedChat && (
        <RenameModal
          isOpen={isModalOpen}
          setIsOpen={setIsModalOpen}
          initialTitle={selectedChat.title} // Mevcut başlığı gönder
          chatId={selectedChat.id} // ID'yi gönder
          onUpdate={handleRenameSubmit} // Submit fonksiyonunu gönder
        />
      )}
    </div>
  );
}
