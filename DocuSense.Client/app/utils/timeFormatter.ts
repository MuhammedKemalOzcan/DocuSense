export const dateFormat = (dateString: string) => {
  const date = new Date(dateString);

  return date.toLocaleTimeString("tr-TR", {
    hour: "2-digit",
    minute: "2-digit",
  });
};
