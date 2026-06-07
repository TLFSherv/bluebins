import { useActionState } from "react"
import { authService } from "../../Services/AuthService"
import type { SignInResponse } from "../../Types/AuthTypes";
import { useNavigate } from "react-router";

export default function SignIn() {
    const navigate = useNavigate();
    const initState = { success: false, message: "", error: null };
    const [state, formAction, isPending] = useActionState(
        async (prevState: SignInResponse, formData: FormData) => {
            return await authService.signIn(prevState, formData, navigate) ?? initState;
        }, initState);

    return (
        <form method="post" action={formAction} className="space-y-6 p-4">
            <h1 className="text-4xl text-center">Sign In</h1>
            <h2>
                To sign into your account enter your credentials in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
                {state.error?.email &&
                    state.error.email.map((error, i) => <span key={i} className="text-danger">{error}</span>)
                }
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" minLength={6} required />
                <label className="form-label">Password</label>
                {state.error?.password &&
                    state.error.password.map((error, i) => <span key={i} className="text-danger">{error}</span>)
                }
            </div>
            <button id="login-submit" type="submit" className="form-btn" disabled={isPending}>
                Login
            </button>
            {state.message.length > 0 && <span className="text-danger">*{state.message}</span>}
            {state.error?.other &&
                state.error.other.map((error, i) => <span key={i} className="text-danger">{error}</span>)
            }
        </form>
    )
}
