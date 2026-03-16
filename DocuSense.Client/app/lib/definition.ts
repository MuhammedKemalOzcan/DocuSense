import z, { email } from "zod";

export const SignUpFormSchema = z.object({
  fullname: z
    .string()
    .min(2, { error: "Name must be at least 2 characters long." }),
  email: z.email({ error: "Please enter a valid email." }).trim(),
  password: z.string().min(8, { error: "Be at least 8 characters long" }),
});

export type SignUpFormState = {
  errors?: {
    fullname?: string[];
    email?: string[];
    password?: string[];
  };
  message?: string | null;
};

export const SignInFormSchema = z.object({
  email: z.email({ message: "Lütfen geçerli bir email girin." }).trim(),
  password: z.string().min(1, { message: "Şifre alanı boş bırakılamaz." }),
});

export type SignInFormState = {
  errors?: {
    email?: string[];
    password?: string[];
  };
  message?: string | null;
};
