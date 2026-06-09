import { useActionState, useContext } from "react"
import { authService } from "../../Services/AuthService"
import { useNavigate } from "react-router";
import type { SignUpResponse } from "../../Types/AuthTypes";
import { UserContext } from "../../../Store/UserContext";

export default function SignUp() {
    const navigate = useNavigate();
    const { setIsSignedIn } = useContext(UserContext);

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

    return (
        <form action={formAction} className="space-y-6 p-4" method="post">
            <h1 className="text-center text-4xl">Sign Up</h1>
            <h2>
                To create an account enter a valid email and password in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
                {state.error?.email &&
                    state.error.email.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" required />
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
            {state.message?.length > 0 &&
                <span className="text-danger">*{state.message}</span>}
            {state.error?.other &&
                state.error.other.map((error, i) => <span key={i} className="text-danger">{error}</span>)}
        </form>
    )
}