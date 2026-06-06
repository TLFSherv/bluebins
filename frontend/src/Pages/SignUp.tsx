import { useActionState } from "react"
import { authService } from "../Services/AuthService"

export default function SignUp() {
    const [state, formAction, isPending] = useActionState(authService.signUp, {
        success: false,
        data: null,
        message: "",
        error: null
    });
    return (
        <form action={formAction} className="space-y-6 p-4" method="post">
            <h1 className="text-center text-4xl">Sign Up</h1>
            <h2>
                To create an account enter a valid email and password in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
                {state?.error?.fieldErrors?.email &&
                    <span className="text-danger">{state?.error?.fieldErrors.email[0]}</span>}
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" required />
                <label className="form-label">Password</label>
                {state?.error?.fieldErrors?.password &&
                    <span className="text-danger">{state?.error?.fieldErrors?.password[0]}</span>}
            </div>
            <div className="form-floating">
                <input type="password" name="confirmPassword" className="form-control" placeholder="password" required />
                <label className="form-label">Confirm password</label>
                {state?.error?.fieldErrors?.confirmPassword &&
                    <span className="text-danger">{state?.error?.fieldErrors?.confirmPassword[0]}</span>}
            </div>
            <button id="login-submit" type="submit" className="form-btn" disabled={isPending}>
                Create Account
            </button>
            {state.message.length > 0 && <span className="text-danger">*{state.message}</span>}
        </form>
    )
}