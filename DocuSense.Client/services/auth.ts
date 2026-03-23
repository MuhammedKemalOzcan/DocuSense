"use server";

import {
  SignInFormSchema,
  SignInFormState,
  SignUpFormSchema,
  SignUpFormState,
} from "@/app/lib/definition";
import { createSession } from "@/app/lib/session";
// import { cookies } from "next/headers";
import { redirect } from "next/navigation";

// //Promise<SignInFormState> : Dönüş tipini açıkça belirtmeliyiz.
// export async function signIn(
//   state: SignInFormState,
//   formData: FormData,
// ): Promise<SignInFormState> {
//   const validatedFields = SignInFormSchema.safeParse({
//     email: formData.get("email"),
//     password: formData.get("password"),
//   });

//   let isSuccess = false;
//   if (!validatedFields.success) {
//     return {
//       errors: validatedFields.error.flatten().fieldErrors,
//     };
//   }
//   const { email, password } = validatedFields.data;

//   if (!process.env.KEYCLOAK_CLIENT_SECRET) {
//     return { message: "Sunucu yapılandırma hatası." };
//   }
//   const body = new URLSearchParams({
//     grant_type: "password",
//     client_id: "docusense-client",
//     client_secret: process.env.KEYCLOAK_CLIENT_SECRET,
//     username: email,
//     password: password,
//   });

//   try {
//     const response = await fetch(`${process.env.SERVER_KEYCLOAK_TOKEN_URL}`, {
//       method: "POST",
//       headers: {
//         "Content-Type": "application/x-www-form-urlencoded",
//       },
//       body: body.toString(),
//     });

//     const data = await response.json();

//     if (!response.ok) {
//       return { message: data.error || "Giriş başarısız oldu." };
//     }
//     await createSession(data.access_token, data.expires_in);
//     isSuccess = true;
//   } catch (error) {
//     console.log(error);

//     return { message: "Sunucuya bağlanılamadı." };
//   }
//   if (isSuccess) {
//     redirect("/");
//   }

//   return state;
// }

export async function signUp(
  state: SignUpFormState,
  formData: FormData,
): Promise<SignUpFormState> {
  const validatedFields = SignUpFormSchema.safeParse({
    firstName: formData.get("firstName"),
    lastName: formData.get("lastName"),
    email: formData.get("email"),
    password: formData.get("password"),
  });

  let isSuccess = false;
  if (!validatedFields.success) {
    return {
      errors: validatedFields.error.flatten().fieldErrors,
    };
  }
  const { firstName, lastName, email, password } = validatedFields.data;

  try {
    console.log("URL:", process.env.SERVER_API_URL);
    const response = await fetch(
      `${process.env.SERVER_API_URL}/Auth/register`,
      {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          register: { firstName, lastName, email, password },
        }),
      },
    );
    if (!response.ok) {
      return { message: "Kayıt olma işlemi başarısız oldu." };
    }
    isSuccess = true;
  } catch (error) {
    console.log(error);

    return { message: "Sunucuya bağlanamadı" };
  }
  if (isSuccess) {
    redirect("/login");
  }

  return state;
}
