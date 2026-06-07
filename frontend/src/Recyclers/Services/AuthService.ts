import type { IAuthService, AuthError } from "../Types/AuthTypes"
import { z } from 'zod'

const backendUrl = import.meta.env.VITE_SERVER_URL;

const SignInSchema = z.object({
    email: z.email("Please enter a valid email address"),
    password: z.string("Not a string")
        .min(6, "Passwords must be at least 6 characters")
        .max(24, "Password is too long")
});

const SignUpSchema = z.object({
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



const parseError = (errorData: any): AuthError => {
    const error: AuthError = { email: [], password: [], other: [] };
    for (const property in errorData?.errors) {
        const value = errorData.errors[property] as string;
        if (property.includes("Password"))
            error.password.push(value);
        else if (property.includes("Email"))
            error.email.push(value);
        else
            error.other.push(value);
    }
    return error;
}

export const authService: IAuthService = {
    async signIn(prevState, formData, navigate) {
        const request = {
            email: formData.get("email") as string,
            password: formData.get("password") as string,
        };

        const validationResult = SignInSchema.safeParse(request);
        if (!validationResult.success) {
            const zodError = z.flattenError(validationResult.error);
            return {
                success: false,
                message: "Validation error",
                error: {
                    email: zodError.fieldErrors.email ?? [],
                    password: zodError.fieldErrors.password ?? [],
                    other: []
                }
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
            if (response.status == 400) {
                return {
                    success: false,
                    message: errorData.title,
                    error: parseError(errorData)
                }
            }
            throw new Error(errorData.detail || "Sign in failed");
        }
        throw new Error("Sign in failed");
        // successful response is just a 200 status code
        await response.json();
        navigate("/portal/dashboard");

    },
    async signUp(prevState, formData, navigate) {
        const request = {
            email: formData.get("email") as string,
            password: formData.get("password") as string,
            confirmPassword: formData.get("confirmPassword") as string
        };

        const validationResult = SignUpSchema.safeParse(request);
        if (!validationResult.success) {
            const zodError = z.flattenError(validationResult.error);
            return {
                success: false,
                message: "Validation error",
                error: {
                    email: zodError.fieldErrors.email ?? [],
                    password: zodError.fieldErrors.password ?? [],
                    confirmPassword: zodError.fieldErrors.confirmPassword ?? [],
                    other: []
                }
            }
        }

        const response = await fetch(`${backendUrl}/register`, {
            method: "Post",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(request)
        });

        if (!response.ok) {
            const errorData = await response.json();
            const error = parseError(errorData);
            if (response.status == 400) {
                return {
                    success: false,
                    message: errorData.title,
                    error: {
                        ...error,
                        confirmPassword: []
                    }
                }
            }
            throw new Error(errorData.detail || "Sign in failed");
        }

        await response.json();
        navigate("/portal/dashboard");
    }
}