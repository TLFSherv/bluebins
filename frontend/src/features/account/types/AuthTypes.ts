import { z } from 'zod'

export const SignInSchema = z.object({
    email: z.email("Please enter a valid email address"),
    password: z.string("Not a string")
        .min(6, "Passwords must be at least 6 characters")
        .max(24, "Password is too long")
});

export const SignUpSchema = z.object({
    email: z.email("Please enter a valid email address"),
    password: z.string("Not a string")
        .min(6, "Passwords must be at least 6 characters")
        .max(24, "Password is too long")
    ,
    confirmPassword: z.string("Not a string")
        .min(6, "Passwords must be at least 6 characters")
        .max(24, "Password is too long")
}).refine((data) => data.password == data.confirmPassword, {
    message: "Password and confirm password must be the same",
    path: ["confirmPassword"]
})

export type AuthResponse = {
    success: boolean,
    data?: string,
    message: string,
    error?: AuthError
}

export type AuthError = {
    email: string[],
    password: string[],
    other: string[],
    confirmPassword?: string[]
}

export type SignInRequest = {
    email: string,
    password: string,
}
export type SignUpRequest = SignInRequest & {
    confirmPassword: string,
    useCookies: true,
};

export interface IAuthService {
    backendUrl: string
    signIn(prevState: AuthResponse, request: FormData): Promise<AuthResponse>,
    signUp(prevState: AuthResponse, request: FormData): Promise<AuthResponse>,
    signOut(navigate: (path: string) => void): void,
    isSignedIn(): Promise<boolean>,
    resendConfirmationEmail(email: string): void,
    signInWithGoogle(): void
}