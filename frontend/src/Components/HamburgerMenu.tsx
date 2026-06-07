
import { useState, useRef, useEffect } from "react"
import { Link } from "react-router";

export default function HamburgerMenu() {
    const [isActive, setIsActive] = useState(false);
    const hamburgerRef = useRef<HTMLDivElement>(null);
    const handleClick = (event: Event) => {
        if (hamburgerRef.current != null) {
            const containsTarget = hamburgerRef.current.contains(event.target as Element);
            setIsActive(containsTarget);
        }
    }
    useEffect(() => {
        window.addEventListener('click', handleClick);
        return () => window.removeEventListener('click', handleClick);
    }, []);

    return (
        <div className="absolute top-0 right-0 z-10">
            <div ref={hamburgerRef}
                className={`flex flex-col justify-center items-center space-y-1.5 bg-[#DDDDDD]/30 backdrop-blur-lg shadow-lg border border-white/30 transition-[width,height] duration-300 cursor-pointer
                    ${isActive ? 'rounded-2xl size-40 m-8' : 'm-4 rounded-full size-14'}`}>
                {isActive ?
                    (
                        <ol className="space-y-2">
                            <li><Link to="/auth/sign-up">Sign up</Link></li>
                            <li><Link to="/auth/sign-in">Sign in</Link></li>
                        </ol>

                    ) :
                    (
                        <>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                            <div className="w-8 h-1 bg-white border-white inset-shadow-sm rounded-2xl"></div>
                        </>
                    )
                }
            </div>
        </div>
    )
}