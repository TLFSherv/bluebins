export type SignInResponse = {
    success: boolean,
    message: string,
    error: SignInError
}

export type SignUpResponse = {
    success: boolean,
    data?: string,
    message: string,
    error: SignUpError,
}

export type AuthError = {
    email: string[],
    password: string[],
    other: string[]
}
export type SignUpError = AuthError & { confirmPassword: string[] } | null
export type SignInError = AuthError | null

export interface IAuthService {
    signIn(prevState: SignInResponse, request: FormData): Promise<SignInResponse | undefined>,
    signUp(prevState: SignUpResponse, request: FormData): Promise<SignUpResponse | undefined>,
    signOut(navigate: (path: string) => void): void,
    isSignedIn(): Promise<boolean>,
    resendConfirmationEmail(email: string): void
}