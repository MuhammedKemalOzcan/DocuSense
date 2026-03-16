"use client";

import FileUploadDropZone from "./FileUploadDropZone";
import Link from "next/link";
import { usePathname } from "next/navigation";
import clsx from "clsx";
import { useDocuments } from "../hooks/useDocuments";
import { useEffect, useState, useRef } from "react"; // useRef eklendi
import { useChatStore } from "../stores/ChatStore";
import LoadingUI from "./Loading";
import { BiLogOut, BiTrash } from "react-icons/bi";
import { deleteSession } from "../lib/session";
import { useRouter } from "next/navigation";
import { CiMenuKebab } from "react-icons/ci";
import { FaPen } from "react-icons/fa";
import toast from "react-hot-toast";
import { UpdateChatTitle } from "../types/chat";
import RenameModal from "./RenameModal";

export default function Sidebar() {
  const pathname = usePathname();
  const router = useRouter();
  const { uploadFile, getDocuments, chatList, deleteChat, updateChatTitle } =
    useDocuments();
  const isUploading = useChatStore((state) => state.isUploading);
  const [openMenuId, setOpenMenuId] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedChat, setSelectedChat] = useState<{
    id: string;
    title: string;
  } | null>(null);

  useEffect(() => {
    getDocuments();
  }, []);

  useEffect(() => {
    const handleClickOutside = () => setOpenMenuId(null);
    if (openMenuId) {
      window.addEventListener("click", handleClickOutside);
    }
    return () => window.removeEventListener("click", handleClickOutside);
  }, [openMenuId]);

  const handleLogout = async () => {
    await deleteSession();
    router.push("/login");
  };

  const handleDelete = async (chatId: string) => {
    const response = await deleteChat(chatId);
    if (response.error) {
      toast.error(response.error);
    }
    if (response.success) {
      toast.success("Chat Deleted Successfully");
      if (pathname.includes(chatId)) router.push("/");
    }
  };

  const openRenameModal = (id: string, title: string) => {
    setSelectedChat({ id, title });
    setIsModalOpen(true);
    setOpenMenuId(null);
  };

  const handleRenameSubmit = async (id: string, title: string) => {
    setIsModalOpen(true);
    const updatedChat: UpdateChatTitle = { id, title };
    const response = await updateChatTitle(updatedChat);
    if (response.error) {
      toast.error(response.error);
    }
    console.log(response);

    if (response.success) {
      toast.success(`Chat title changed to: ${response.title}`);
      setIsModalOpen(false);
      setSelectedChat(null);
    }
    console.log("çalıştı");
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

      <div className="mt-4 flex-1 overflow-y-auto custom-scrollbar">
        {chatList !== null &&
          chatList.map((chat) => (
            <div
              className={clsx(
                // "overflow-hidden" buradan kaldırıldı, menünün görünmesini engelliyordu
                "text-sm p-3 rounded-xl flex mb-4 justify-between h-12 items-center relative group transition-colors",
                {
                  "bg-blue-600 text-white": pathname === `/chat/${chat.id}`,
                  "hover:bg-gray-900 text-gray-300":
                    pathname !== `/chat/${chat.id}`,
                },
              )}
              key={chat.id}
            >
              <Link
                href={`/chat/${chat.id}`}
                className="flex-1 overflow-hidden whitespace-nowrap text-ellipsis pr-8"
              >
                {chat.title}
              </Link>

              <div className="absolute right-2 inset-y-0 flex items-center">
                <div className="relative">
                  <button
                    onClick={(e) => {
                      e.preventDefault();
                      e.stopPropagation();
                      setOpenMenuId((prev) =>
                        prev === chat.id ? null : chat.id,
                      );
                    }}
                    className={clsx(
                      "p-1.5 rounded-lg hover:bg-gray-700/50 flex items-center justify-center transition-opacity",
                      {
                        "opacity-100": openMenuId === chat.id,
                        "opacity-0 group-hover:opacity-100":
                          openMenuId !== chat.id,
                      },
                    )}
                  >
                    <CiMenuKebab size={20} />
                  </button>

                  {/* Dropdown Menu - Fixed Konumlandırma */}
                  {openMenuId === chat.id && (
                    <>
                      {/* Arkaplanda görünmez bir katman: Menü dışına tıklandığında kapanması için */}
                      <div
                        className="fixed inset-0 z-[90]"
                        onClick={() => setOpenMenuId(null)}
                      />

                      <div
                        className="fixed z-[100] w-44 bg-gray-900 border border-gray-700 rounded-lg shadow-2xl py-1 overflow-hidden"
                        style={{
                          // Butonun yanına sabitlemek için butonun konumunu baz alıyoruz
                          // Sidebar genişliği 80 (w-80 = 320px) olduğu için sol tarafa 320px veriyoruz
                          left: "310px",
                          marginTop: "-20px", // Butonun hizasına çekmek için
                        }}
                        onClick={(e) => e.stopPropagation()}
                      >
                        <button
                          onClick={() => openRenameModal(chat.id, chat.title)}
                          className="flex items-center gap-4 w-full text-left px-4 py-2.5 text-xs text-white hover:bg-gray-800 transition-colors"
                        >
                          <span>
                            <FaPen size={12} />
                          </span>
                          <p className="text-[16px] whitespace-nowrap">
                            Yeniden Adlandır
                          </p>
                        </button>
                        <div className="h-px bg-gray-800 my-1" />
                        <button
                          onClick={() => handleDelete(chat.id)}
                          className="flex items-center gap-4 w-full text-left px-4 py-2.5 text-xs text-red-400 hover:bg-gray-800 transition-colors font-medium"
                        >
                          <span>
                            <BiTrash size={20} />
                          </span>
                          <p className="text-[16px]">Sil</p>
                        </button>
                      </div>
                    </>
                  )}
                </div>
              </div>
            </div>
          ))}
      </div>

      {/* User Profile */}
      <div className="border-t border-gray-700 pt-4 mt-auto flex items-center gap-3">
        <div className="w-10 h-10 bg-gradient-to-tr from-orange-400 to-pink-500 rounded-full flex items-center justify-center text-white font-bold text-sm">
          AM
        </div>
        <div className="flex-1 min-w-0">
          <p className="text-white text-sm font-semibold truncate">
            Alex Morgan
          </p>
          <p className="text-gray-500 text-xs">Pro Account</p>
        </div>
        <button
          onClick={handleLogout}
          className="text-gray-400 hover:text-white transition-colors"
        >
          <BiLogOut size={24} />
        </button>
      </div>
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
