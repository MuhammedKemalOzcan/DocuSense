import { auth } from "@/auth";
import Sidebar from "./components/Sidebar";

export default async function Home() {
  const session = await auth();
  const user = session?.user;
  return (
    <div className="flex h-screen w-full bg-gray-900">
      {user && <Sidebar user={user} />}
      <div className="flex-1 flex items-center justify-center bg-gray-900">
        <h1 className="text-center">
          Hoş Geldiniz, başlamak için soldan bir PDF seçin veya yeni yükleyin
        </h1>
      </div>
    </div>
  );
}
