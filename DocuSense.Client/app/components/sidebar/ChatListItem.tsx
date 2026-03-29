import { useDocuments } from "@/app/hooks/useDocuments";
import { useChatStore } from "@/app/stores/ChatStore";
import clsx from "clsx";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";
import React from "react";
import { Dispatch, SetStateAction } from "react";
import toast from "react-hot-toast";
import { BiTrash } from "react-icons/bi";
import { CiMenuKebab } from "react-icons/ci";
import { FaPen } from "react-icons/fa";

interface ChatListItemProps {
  openMenuId: string | null;
  setOpenMenuId: Dispatch<SetStateAction<string | null>>;
  setSelectedChat: Dispatch<
    SetStateAction<{
      id: string;
      title: string;
    } | null>
  >;
  setIsModalOpen: Dispatch<SetStateAction<boolean>>;
}

export default function ChatListItem({
  openMenuId,
  setOpenMenuId,
  setSelectedChat,
  setIsModalOpen,
}: ChatListItemProps) {
  const pathname = usePathname();
  const router = useRouter();

  const { deleteChat, chatList } = useDocuments();

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

  return (
    <div className="mt-4 flex-1 overflow-y-auto custom-scrollbar">
      {chatList !== null &&
        chatList.map((chat) => (
          <div
            className={clsx(
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
                {openMenuId === chat.id && (
                  <>
                    <div
                      className="fixed inset-0 z-[90]"
                      onClick={() => setOpenMenuId(null)}
                    />

                    <div
                      className="fixed z-[100] w-44 bg-gray-900 border border-gray-700 rounded-lg shadow-2xl py-1 overflow-hidden"
                      style={{
                        left: "310px",
                        marginTop: "-20px",
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
  );
}
