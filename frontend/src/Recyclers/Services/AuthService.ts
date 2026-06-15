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
    async signIn(prevState, formData) {
        try {
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

            const response = await fetch(`${backendUrl}/login?useCookies=true`,
                {
                    method: "Post",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(request),
                    credentials: "include"
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
                else if (response.status == 401) {
                    throw new Error("Incorrect username or password");
                }
                throw new Error(errorData.detail || "Sign in failed");
            }
            return {
                success: true,
                message: "Signed in successfully",
                error: null
            };

        } catch (error: any) {
            if (error.message.includes("Server error")) {
                throw error;
            }
            return {
                success: false,
                message: error.message,
                error: null
            }
        }

    },
    async signUp(prevState, formData) {
        try {
            const request = {
                email: formData.get("email") as string,
                password: formData.get("password") as string,
                confirmPassword: formData.get("confirmPassword") as string,
                useCookies: true,
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

            const response = await fetch(`${backendUrl}/register?useCookies=true`, {
                method: "Post",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(request),
                credentials: "include"
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
                throw new Error(errorData.detail || "Sign up failed");
            }

            return {
                success: true,
                data: request.email,
                message: "Signed up succesfully",
                error: null
            };

        } catch (error: any) {
            if (error.message.includes("Server error")) {
                throw error;
            }
            return {
                success: false,
                message: error.message,
                error: null
            }
        }
    },
    async signOut(navigate) {
        const response = await fetch(`${backendUrl}/logout`, {
            method: "POST",
            headers: {
                "Content-Type": "text"
            },
            credentials: "include",
        });

        if (!response.ok) {
            const errorData = await response.json();
            return new Error(errorData ?? "Problem with signing out");
        }

        navigate("/");
    },
    async isSignedIn() {
        try {
            const response = await fetch(`${backendUrl}/isSignedIn`, {
                method: "GET",
                headers: {
                    "Content-Type": "application/json"
                },
                credentials: "include"
            });

            if (!response.ok) {
                const errorData = await response.json();
                throw new Error(errorData ?? "Error verifying if user is signed in");
            }

            const data = await response.json();
            return data.isSignedIn;
        } catch (error: any) {
            console.log(error.message);
            return false;
        }
    },
    async resendConfirmationEmail(email: string) {
        // put a limit on the number of confirmation emails that can be sent
        console.log(`Resend email confirmation to ${email}`);

    },
    async signInWithGoogle() {
        const returnUrl = encodeURIComponent("https://localhost:5173/portal/dashboard");
        window.location.href = `${backendUrl}/login/google?returnUrl=${returnUrl}`;
    },
}