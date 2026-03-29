import { KeycloakSignOut } from "@/app/lib/action";
import { User } from "next-auth";
import { BiLogOut } from "react-icons/bi";

export default function UserProfile({ user }: { user: User }) {
  return (
    <div>
      {user && (
        <div className="border-t border-gray-700 pt-4 mt-auto flex items-center gap-3">
          <div className="w-10 h-10 bg-gradient-to-tr from-orange-400 to-pink-500 rounded-full flex items-center justify-center text-white font-bold text-sm">
            {user.name?.charAt(0)}
          </div>
          <div className="flex-1 min-w-0">
            <p className="text-white text-sm font-semibold truncate">
              {user.name}
            </p>
            <p className="text-gray-500 text-xs">Pro Account</p>
          </div>

          <form action={KeycloakSignOut}>
            <button
              type="submit"
              className="text-gray-400 hover:text-white transition-colors"
            >
              <BiLogOut size={24} />
            </button>
          </form>
        </div>
      )}
    </div>
  );
}
