export default function SignIn() {
    return (
        <form method="post" className="space-y-6 p-4">
            <h1 className="text-4xl text-center">Sign In</h1>
            <h2>
                To sign into your account enter your credentials in the fields below.
            </h2>
            <div className="form-floating">
                <input type="email" name="email" className="form-control" placeholder="name@example.com" required />
                <label className="form-label">Email</label>
            </div>
            <div className="form-floating">
                <input type="password" name="password" className="form-control" autoComplete="current-password" placeholder="password" required />
                <label className="form-label">Password</label>
            </div>
            <button id="login-submit" type="submit" className="form-btn">
                Login
            </button>
        </form>
    )
}
