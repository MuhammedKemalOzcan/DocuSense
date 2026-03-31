import { Message } from "../types/chat";
import clsx from "clsx";
import ChatMessage from "./ChatMessage";
import { dateFormat } from "../utils/timeFormatter";

export default function MessageBubble({ message }: { message: Message }) {
  console.log("message: ", message);
  console.log("Saat: ", message.createdAt);

  return (
    <div
      className={clsx("flex gap-4 mb-4", {
        "justify-end": message.isUser === true,
      })}
    >
      {!message.isUser && (
        <div className="w-8 h-8 bg-gray-700 rounded-full flex items-center justify-center text-white text-sm">
          🤖
        </div>
      )}
      <div className="max-w-2xl">
        <div
          className={clsx("bg-blue-600 rounded-lg p-4", {
            "bg-gray-800": message.isUser === false,
          })}
        >
          <div className="text-white text-sm">
            {/* Kod bloklarını ayıklar. */}
            <ChatMessage text={message.text} />
          </div>
        </div>
        <p
          className={clsx("text-gray-500 text-xs mt-1", {
            "text-right": message.isUser === false,
          })}
        >
          {dateFormat(message.createdAt)}
        </p>
      </div>
    </div>
  );
}
