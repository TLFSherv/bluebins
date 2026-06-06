import { useActionState } from "react"
import { authService } from "../Services/AuthService"

export default function SignIn() {
    const [state, formAction, isPending] = useActionState(authService.signIn, {
        success: false,
        data: null,
        message: "",
        error: null
    });

    return (
        <form method="post" action={formAction} className="space-y-6 p-4">
            <h1 className="text-4xl text-center">Sign In</h1>
            <h2>
                To sign into your account enter your credentials in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
                {state?.error?.fieldErrors?.email &&
                    <span className="text-danger">{state?.error?.fieldErrors.email[0]}</span>}
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" minLength={6} required />
                <label className="form-label">Password</label>
                {state?.error?.fieldErrors?.password &&
                    <span className="text-danger">{state?.error?.fieldErrors?.password[0]}</span>}
            </div>
            <button id="login-submit" type="submit" className="form-btn" disabled={isPending}>
                Login
            </button>
            {state.message.length > 0 && <span className="text-danger">*{state.message}</span>}
        </form>
    )
}
