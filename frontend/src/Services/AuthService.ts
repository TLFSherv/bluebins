import type { IAuthService, AuthResponse } from "../Types/AuthTypes"
import { z } from 'zod'

const backendUrl = import.meta.env.VITE_SERVER_URL;

const SignInSchema = z.object({
    email: z.email("Please enter a valid email address"),
    password: z.string("Not a string")
        .min(6, "Passwords must be greater than 6 characters")
        .max(24, "Password is too long")
});

const SignUpSchema = z.object({
    email: z.email("Please enter a valid email address"),
    password: z.string("Not a string")
        .min(6, "Passwords must be greater than 6 characters")
        .max(24, "Password is too long"),
    confirmPassword: z.string("Not a string")
        .min(6, "Passwords must be greater than 6 characters")
        .max(24, "Password is too long")
}).refine((data) => data.password == data.confirmPassword, {
    message: "Password and confirm password must be the same",
    path: ["confirmPassword"]
})

export const authService: IAuthService = {
    async signIn(prevState: AuthResponse, formData: FormData): Promise<AuthResponse> {
        try {
            const request = {
                email: formData.get("email") as string,
                password: formData.get("password") as string,
            };

            const validationResult = SignInSchema.safeParse(request);
            if (!validationResult.success) {
                return {
                    success: false,
                    data: null,
                    message: "Validation error",
                    error: z.flattenError(validationResult.error)
                }
            }

            const response = await fetch(`${backendUrl}/login`,
                {
                    method: "Post",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(request)
                });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.detail || "Sign in failed");
            }

            const data = await response.json();

            return {
                success: true,
                data,
                message: "Successful sign in",
                error: null
            };
        } catch (error: any) {
            return {
                success: false,
                data: null,
                message: "Error signing into the application",
                error: error.message
            }
        }
    },
    async signUp(prevState: AuthResponse, formData: FormData): Promise<AuthResponse> {
        try {
            console.log(backendUrl);
            const request = {
                email: formData.get("email") as string,
                password: formData.get("password") as string,
                confirmPassword: formData.get("confirmPassword") as string
            };

            const validationResult = SignUpSchema.safeParse(request);
            if (!validationResult.success) {
                return {
                    success: false,
                    data: null,
                    message: "Validation error",
                    error: z.flattenError(validationResult.error)
                }
            }

            const response = await fetch(`${backendUrl}/register`,
                {
                    method: "Post",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(request)
                });
            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData.detail || "Sign up failed");
            }

            const data = await response.json();
            console.log(data);
            return {
                success: true,
                data,
                message: "Successful sign up",
                error: null
            };
        } catch (error: any) {
            return {
                success: false,
                data: null,
                message: "Error signing up for the application",
                error: error.message
            }
        }
    }
}