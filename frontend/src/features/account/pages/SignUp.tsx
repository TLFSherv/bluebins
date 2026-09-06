import { useActionState, useContext } from "react"
import { authService } from "../services/AuthService"
import type { SignUpResponse } from "../types/AuthTypes";
import { UserContext } from "../store/UserContext";
import { Link } from "react-router";

export default function SignUp() {
    const { setIsSignedIn, isSignedIn: isSignedUp } = useContext(UserContext);
    const initState = { success: false, data: "", message: "", error: null };

    const [state, formAction, isPending] = useActionState(
        async (prevState: SignUpResponse, formData: FormData) => {
            const result = await authService.signUp(prevState, formData) ?? initState;
            if (result.success) {
                setIsSignedIn(true);
            }
            return result;
        }, initState);

    let pageTitle = "Sign up";
    let pageText = "To create an account enter a valid email and password in the fields below.";
    if (isSignedUp) {
        pageTitle = "Confirm Email";
        pageText = "A confirmation email has been sent, resend or otherwise click next. Email confirmation will be required before booking."
    }

    return (
        <form action={formAction} className="space-y-6 p-4">
            <h1 className="text-center text-4xl">{pageTitle}</h1>
            <h2>{pageText}</h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required disabled={isSignedUp} />
                <label className="form-label">Email</label>
                {state.error?.email &&
                    state.error.email.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
            </div>
            {isSignedUp && <VerifyEmailButton email={state.data as string} />}
            <div className={`space-y-6 ${isSignedUp && "hidden"}`}>
                <div className="form-floating">
                    <input type="password" name="password" className="form-control" placeholder="password" required />
                    <label className="form-label">Password</label>
                    {state.error?.password &&
                        state.error.password.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
                </div>
                <div className="form-floating">
                    <input type="password" name="confirmPassword" className="form-control" placeholder="password" required />
                    <label className="form-label">Confirm password</label>
                    {state.error?.confirmPassword &&
                        state.error.confirmPassword.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
                </div>
                <button id="login-submit" type="submit" className="form-btn" disabled={isPending}>
                    Create Account
                </button>
            </div>
            {!state.success && state.message?.length > 0 &&
                <span className="text-danger">*{state.message}</span>}
            {state.error?.other &&
                state.error.other.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
            <div className="space-y-4">
                <p className="border-b border-gray-300">Use another service to sign up</p>
                <button className="mx-4 bg-white border-1 tracking-wide rounded-lg w-1/3 py-2 flex items-center justify-center space-x-2 text-lg" type="button" onClick={authService.signInWithGoogle} >
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 48 48" width="16px" height="16px">
                        <path fill="#FFC107" d="M43.611,20.083H42V20H24v8h11.303c-1.649,4.657-6.08,8-11.303,8c-6.627,0-12-5.373-12-12c0-6.627,5.373-12,12-12c3.059,0,5.842,1.154,7.961,3.039l5.657-5.657C34.046,6.053,29.268,4,24,4C12.955,4,4,12.955,4,24c0,11.045,8.955,20,20,20c11.045,0,20-8.955,20-20C44,22.659,43.862,21.35,43.611,20.083z" />
                        <path fill="#FF3D00" d="M6.306,14.691l6.571,4.819C14.655,15.108,18.961,12,24,12c3.059,0,5.842,1.154,7.961,3.039l5.657-5.657C34.046,6.053,29.268,4,24,4C16.318,4,9.656,8.337,6.306,14.691z" />
                        <path fill="#4CAF50" d="M24,44c5.166,0,9.86-1.977,13.409-5.192l-6.19-5.238C29.211,35.091,26.715,36,24,36c-5.202,0-9.619-3.317-11.283-7.946l-6.522,5.025C9.505,39.556,16.227,44,24,44z" />
                        <path fill="#1976D2" d="M43.611,20.083H42V20H24v8h11.303c-0.792,2.237-2.231,4.166-4.087,5.571c0.001-0.001,0.002-0.001,0.003-0.002l6.19,5.238C36.971,39.205,44,34,44,24C44,22.659,43.862,21.35,43.611,20.083z" />
                    </svg>
                    <span>
                        Google
                    </span>
                </button>
            </div>
        </form>
    )
}

function VerifyEmailButton({ email }: { email: string }) {
    return (
        <div className="space-y-4">
            <button type="button" className="form-btn" onClick={() => authService.resendConfirmationEmail(email)}>
                Resend
            </button>
            <Link to={"/portal/dashboard"}>
                <button type="button" className="form-btn bg-black flex justify-center items-center space-x-2">
                    <span>Next</span>
                    <svg className="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                        <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 12H5m14 0-4 4m4-4-4-4" />
                    </svg>
                </button>
            </Link>
        </div>
    )
}
