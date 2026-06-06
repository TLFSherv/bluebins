
export default function SignUp() {
    return (
        <form className="space-y-6 p-4" method="post">
            <h1 className="text-center text-4xl">Sign Up</h1>
            <h2>
                To create an account enter a valid email and password in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" required />
                <label className="form-label">Password</label>
            </div>
            <div className="form-floating">
                <input type="password" name="confirmPassword" className="form-control" placeholder="password" required />
                <label className="form-label">Confirm password</label>
            </div>
            <button id="login-submit" type="submit" className="form-btn">
                Create Account
            </button>
        </form>
    )
}