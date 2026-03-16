"use client";

import React, { useEffect, useRef, useState } from "react";
import { FaRegFilePdf } from "react-icons/fa";
import { SlCloudUpload } from "react-icons/sl";
import { useChatStore } from "../stores/ChatStore";
import toast from "react-hot-toast";

interface Props {
  onUploadFile: (file: File) => Promise<void>;
}

export default function FileUploadDropZone({ onUploadFile }: Props) {
  const [file, setFile] = useState<File>();
  const status = useChatStore((state) => state.status);

  const inputRef = useRef<HTMLInputElement>(null);

  const focusInput = () => {
    if (inputRef.current) {
      inputRef.current.click();
    }
  };

  const handleChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files) return;
    const files = e.target.files;
    setFile(files[0]);
  };

  const handleClick = () => {
    if (!file) return;
    onUploadFile(file);
  };

  return (
    <div>
      <div
        onClick={focusInput}
        className="border-2 border-dashed border-gray-700 rounded-lg p-8 mb-6 flex flex-col items-center h-52 justify-center text-center"
      >
        {file ? (
          <p className="flex items-center gap-1 overflow-hidden">
            <span>
              <FaRegFilePdf color="red" size={30} />
            </span>
            <span className="text-sm break-all whitespace-normal">
              {file.name}
            </span>
          </p>
        ) : (
          <div className="flex flex-col items-center">
            <div className="text-4xl mb-3">
              <SlCloudUpload size={44} />
            </div>
            <input
              ref={inputRef}
              className="hidden"
              accept=".pdf"
              type="file"
              id="file"
              name="file"
              onChange={handleChange}
            />
            <p className="text-gray-400 text-sm font-medium">Drop PDF here</p>
            <p className="text-gray-500 text-xs">or click to browse</p>
          </div>
        )}
      </div>
      <button
        onClick={handleClick}
        className="w-full bg-blue-600 hover:bg-blue-700 text-white font-semibold py-3 px-4 rounded-lg mb-8 flex items-center justify-center gap-2"
      >
        <span>+</span>
        <span>Upload New</span>
      </button>
      <Result status={status} />
    </div>
  );
}

const Result = ({ status }: { status: string }) => {
  if (status === "fail") {
    return <p>❌ Files upload failed!</p>;
  } else if (status === "uploading") {
    return <p>⏳ Uploading selected files...</p>;
  } else {
    return null;
  }
};
