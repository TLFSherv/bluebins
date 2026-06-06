export type AuthResponse = {
    success: boolean,
    data: any,
    message: string,
    error: any
}

export interface IAuthService {
    signIn(prevState: AuthResponse, request: FormData): Promise<AuthResponse>,
    signUp(prevState: AuthResponse, request: FormData): Promise<AuthResponse>
}