"use client";

import React, { useEffect, useRef, useState } from "react";
import { FaRegFilePdf, FaCheckCircle, FaTimesCircle, FaTimes } from "react-icons/fa";
import { SlCloudUpload } from "react-icons/sl";
import { useChatStore } from "../stores/ChatStore";

interface Props {
  onUploadFile: (file: File) => Promise<void>;
}

function formatBytes(bytes: number) {
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

export default function FileUploadDropZone({ onUploadFile }: Props) {
  const [file, setFile] = useState<File>();
  const [isDragging, setIsDragging] = useState(false);
  const status = useChatStore((state) => state.status);
  const setUploading = useChatStore((state) => state.setUploading);
  const isUploading = status === "uploading";

  useEffect(() => {
    if (status !== "success") return;
    const timer = setTimeout(() => setUploading(false, "initial"), 3000);
    return () => clearTimeout(timer);
  }, [status]);

  const inputRef = useRef<HTMLInputElement>(null);

  const focusInput = () => {
    if (isUploading) return;
    inputRef.current?.click();
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files) return;
    setFile(e.target.files[0]);
  };

  const handleDragOver = (e: React.DragEvent) => {
    e.preventDefault();
    if (!isUploading) setIsDragging(true);
  };

  const handleDragLeave = () => setIsDragging(false);

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    setIsDragging(false);
    if (isUploading) return;
    const dropped = e.dataTransfer.files[0];
    if (dropped?.type === "application/pdf") setFile(dropped);
  };

  const handleRemoveFile = (e: React.MouseEvent) => {
    e.stopPropagation();
    setFile(undefined);
    if (inputRef.current) inputRef.current.value = "";
  };

  const handleClick = () => {
    if (!file || isUploading) return;
    onUploadFile(file);
  };

  const dropZoneBorder = isDragging
    ? "border-blue-500 bg-blue-500/5"
    : file
    ? "border-blue-600/50 bg-blue-600/5"
    : "border-gray-700 hover:border-gray-500";

  return (
    <div className="flex flex-col gap-3">
      {/* Drop Zone */}
      <div
        onClick={focusInput}
        onDragOver={handleDragOver}
        onDragLeave={handleDragLeave}
        onDrop={handleDrop}
        className={`border-2 border-dashed rounded-xl p-6 flex flex-col items-center h-48 justify-center text-center transition-all duration-200 cursor-pointer ${dropZoneBorder} ${isUploading ? "opacity-60 cursor-not-allowed" : ""}`}
      >
        {file ? (
          <div className="flex flex-col items-center gap-2 w-full px-2">
            <div className="relative">
              <FaRegFilePdf className="text-red-400" size={36} />
              {!isUploading && (
                <button
                  onClick={handleRemoveFile}
                  className="absolute -top-1.5 -right-1.5 bg-gray-700 hover:bg-gray-600 rounded-full p-0.5 transition-colors"
                >
                  <FaTimes size={9} className="text-gray-300" />
                </button>
              )}
            </div>
            <p className="text-sm font-medium text-gray-200 break-all leading-tight line-clamp-2 max-w-full">
              {file.name}
            </p>
            <span className="text-xs text-gray-500 bg-gray-800 px-2 py-0.5 rounded-full">
              {formatBytes(file.size)}
            </span>
          </div>
        ) : (
          <div className="flex flex-col items-center gap-2">
            <SlCloudUpload
              size={40}
              className={`transition-colors ${isDragging ? "text-blue-400" : "text-gray-500"}`}
            />
            <input
              ref={inputRef}
              className="hidden"
              accept=".pdf"
              type="file"
              id="file"
              name="file"
              onChange={handleChange}
            />
            <div>
              <p className="text-gray-300 text-sm font-medium">
                {isDragging ? "Release to drop" : "Drop PDF here"}
              </p>
              <p className="text-gray-600 text-xs mt-0.5">or click to browse</p>
            </div>
          </div>
        )}
      </div>

      {/* Upload Button */}
      <button
        onClick={handleClick}
        disabled={!file || isUploading}
        className={`w-full font-semibold py-2.5 px-4 rounded-xl flex items-center justify-center gap-2 transition-all duration-200 text-sm
          ${!file || isUploading
            ? "bg-gray-800 text-gray-500 cursor-not-allowed"
            : "bg-blue-600 hover:bg-blue-500 active:scale-[0.98] text-white shadow-lg shadow-blue-600/20"
          }`}
      >
        {isUploading ? (
          <>
            <svg className="animate-spin h-4 w-4 text-gray-400" viewBox="0 0 24 24" fill="none">
              <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
              <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8v4a4 4 0 00-4 4H4z" />
            </svg>
            <span>Uploading...</span>
          </>
        ) : (
          <>
            <SlCloudUpload size={16} />
            <span>Upload Document</span>
          </>
        )}
      </button>

      {/* Status Result */}
      <Result status={status} />
    </div>
  );
}

const Result = ({ status }: { status: string }) => {
  if (status === "fail") {
    return (
      <div className="flex items-center gap-2.5 bg-red-500/10 border border-red-500/20 text-red-400 rounded-xl px-4 py-3 text-sm">
        <FaTimesCircle size={15} className="shrink-0" />
        <span className="font-medium">Upload failed. Please try again.</span>
      </div>
    );
  }

  if (status === "success") {
    return (
      <div className="flex items-center gap-2.5 bg-green-500/10 border border-green-500/20 text-green-400 rounded-xl px-4 py-3 text-sm">
        <FaCheckCircle size={15} className="shrink-0" />
        <span className="font-medium">Document uploaded successfully.</span>
      </div>
    );
  }

  return null;
};
