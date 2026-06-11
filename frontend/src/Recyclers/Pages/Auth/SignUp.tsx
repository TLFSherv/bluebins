import { useActionState, useContext } from "react"
import { authService } from "../../Services/AuthService"
import type { SignUpResponse } from "../../Types/AuthTypes";
import { UserContext } from "../../../Store/UserContext";
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
        pageText = "A confirmation email has been sent, resend or select next. Email confirmation will be required before booking."
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
