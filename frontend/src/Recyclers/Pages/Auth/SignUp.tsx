import { useActionState, useContext, useState } from "react"
import { authService } from "../../Services/AuthService"
import { useNavigate } from "react-router";
import type { SignUpResponse } from "../../Types/AuthTypes";
import { UserContext } from "../../../Store/UserContext";

export default function SignUp() {
    const navigate = useNavigate();
    const { setIsSignedIn } = useContext(UserContext);
    const [isSignedUp, setIsSignedUp] = useState(false);

    const initState = { success: false, message: "", error: null };
    const [state, formAction, isPending] = useActionState(
        async (prevState: SignUpResponse, formData: FormData) => {
            const result = await authService.signUp(prevState, formData) ?? initState;
            if (result.success) {
                setIsSignedIn(true);
                navigate("/portal/dashboard");
            }
            return result;
        }, initState);

    let pageTitle = "Sign up";
    let pageText = "To create an account enter a valid email and password in the fields below.";
    if (isSignedUp) {
        pageTitle = "Confirm Email";
        pageText = "A confirmation email has been sent, resend or skip this step. Email confirmation will be required before booking."
    }

    return (
        // <form action={formAction} className="space-y-6 p-4" method="post">
        <form className="space-y-6 p-4">
            <h1 className="text-center text-4xl">{pageTitle}</h1>
            <h2>{pageText}</h2>
            <div className="form-floating">
                {/* <input type="email" name="email" className="form-control" placeholder="name@example.com" required /> */}
                <input type="email" name="email" className="form-control" placeholder="name@example.com" value={"name@example.com"} disabled />
                <label className="form-label">Email</label>
                {state.error?.email &&
                    state.error.email.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
            </div>
            {isSignedUp && <VerifyEmailButton />}
            <div className={`space-y-6 ${isSignedUp && "hidden"}`}>
                <div className="form-floating">
                    {/* <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" required /> */}
                    <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" value="password" />
                    <label className="form-label">Password</label>
                    {state.error?.password &&
                        state.error.password.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
                </div>
                <div className="form-floating">
                    {/* <input type="password" name="confirmPassword" className="form-control" placeholder="password" required /> */}
                    <input type="password" name="confirmPassword" className="form-control" placeholder="password" value={"password"} />
                    <label className="form-label">Confirm password</label>
                    {state.error?.confirmPassword &&
                        state.error.confirmPassword.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
                </div>
                {/* <button id="login-submit" type="submit" className="form-btn" disabled={isPending}>
                Create Account
            </button> */}
                <button id="login-submit" onClick={() => setIsSignedUp(true)} type="button" className="form-btn" disabled={isPending}>
                    Create Account
                </button>
            </div>
            {state.message?.length > 0 &&
                <span className="text-danger">*{state.message}</span>}
            {state.error?.other &&
                state.error.other.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
        </form>
    )
}

function VerifyEmailButton() {
    return (
        <div className="space-y-4">
            <button type="button" className="form-btn" onClick={() => console.log("Confirmation email sent")}>Resend</button>
            <button type="button" className="form-btn bg-black flex justify-center items-center space-x-2" onClick={() => console.log("Skip step")}>
                <span>Skip</span>
                <svg className="w-6 h-6 text-gray-800 dark:text-white" aria-hidden="true" xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="none" viewBox="0 0 24 24">
                    <path stroke="currentColor" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 12H5m14 0-4 4m4-4-4-4" />
                </svg>
            </button>
        </div>
    )
}
