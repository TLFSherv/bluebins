
import { useState, useRef, useEffect, useContext } from "react"
import { Link } from "react-router";
import { authService } from "../features/account/services/AuthService";
import { useNavigate } from "react-router";
import { UserContext } from "../features/account/store/UserContext";

export default function HamburgerMenu() {
    const [isActive, setIsActive] = useState(false);
    const hamburgerRef = useRef<HTMLDivElement>(null);
    const searchRef = useRef
    const { isSignedIn, setIsSignedIn } = useContext(UserContext);

    const handleClick = () => {
        if (hamburgerRef.current != null) {
            setIsActive(false);
        }
    }

    useEffect(() => {
        window.addEventListener('click', handleClick);
        return () => window.removeEventListener('click', handleClick);
    }, []);

    return (
        <div className="absolute top-0 right-0 z-10">
            <div ref={hamburgerRef}
                // Stop propagation so internal DOM changes don't confuse the window listener.
                // Also, toggle the menu open if it's currently closed.
                onClick={(e) => {
                    e.stopPropagation();
                    if (!isActive) setIsActive(true);
                }}
                className={`flex flex-col justify-center items-center space-y-1.5 bg-[#DDDDDD]/30 backdrop-blur-lg shadow-lg border border-white/30 transition-[width,height] duration-300 cursor-pointer
                    ${isActive ? 'rounded-2xl size-50 m-8' : 'm-4 rounded-full size-14'}`}>
                {isActive ? <AuthNav {...{ isSignedIn, setIsSignedIn }} /> :
                    (
                        <>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                        </>
                    )
                }
            </div >
        </div >
    )
}

function AuthNav(props: { isSignedIn: boolean, setIsSignedIn: React.Dispatch<React.SetStateAction<boolean>> }) {
    const [signingOut, setSigningOut] = useState(false);
    const timeoutIdRef = useRef<number | null>(0);
    const navigate = useNavigate();

    const startSignOut = () => {
        setSigningOut(true);
        timeoutIdRef.current = setTimeout(() => {
            authService.signOut(navigate);
            props.setIsSignedIn(false);
            timeoutIdRef.current = null;
            setSigningOut(false);
        }, 3000);
    }

    const cancelSignOut = () => {
        if (timeoutIdRef.current != null) {
            clearTimeout(timeoutIdRef.current);
            setSigningOut(false);
        }
    }

    if (!props.isSignedIn) {
        return (
            <ol className="space-y-2 text-center text-lg">
                <li><Link to="/account/sign-up" >Sign up</Link></li>
                <li><Link to="/account/sign-in" >Sign in</Link></li>
            </ol>
        )
    }
    return (
        <div className="space-y-2 text-center p-2 h-full">
            <form className="p-1">
                <input name="accountName" type="text" placeholder={"Find page"} className="w-full px-2 py-[2px] border rounded-md border-gray-300 text-sm text-center" />
            </form>
            <ol className="space-y-2 h-full">
                <li>
                    <Link to={"/booking"}> Booking</Link>
                </li>
                <li>
                    <Link to={"/dashboard"}>Dashboard</Link>
                </li>
                <li>
                    Account
                </li>
                {signingOut ?
                    <li className="flex justify-center">
                        <svg className="animate-spin -ml-6 mt-1 mr-2 h-4 w-4 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                            <path className="opacity-75" fill="black" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                        </svg>
                        <button type="button" onClick={() => cancelSignOut()} className="font-medium">
                            Cancel
                        </button>
                    </li> :
                    <button type="button" onClick={() => startSignOut()} >
                        Sign out
                    </button>
                }
            </ol>
        </div>
    );
}



