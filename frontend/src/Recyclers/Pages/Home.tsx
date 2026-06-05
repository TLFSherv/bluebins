
import { useState, useRef, useEffect } from "react"

export default function Home() {
    const [isActive, setIsActive] = useState(false);
    const hamburgerRef = useRef<HTMLDivElement>(null);
    const handleClick = (event: Event) => {
        const containsTarget = hamburgerRef?.current?.contains(event.target as Element) ?? false;
        setIsActive(containsTarget);
    }
    useEffect(() => {
        window.addEventListener('click', handleClick);
        return () => window.removeEventListener('click', handleClick);
    }, []);

    return (
        <>
            <div className="flex-1">
                <div ref={hamburgerRef}
                    className={`flex flex-col justify-center items-center space-y-1.5 bg-[#DDDDDD]/80 backdrop-blur-lg shadow-lg border border-white/30 transition-[width,height] duration-300 cursor-pointer
                    ${isActive ? 'rounded-2xl size-40 m-8' : 'm-4 rounded-full size-14'}`}>
                    {isActive ?
                        (
                            <ol>
                                <li>Sign up</li>
                                <li>Sign in</li>
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
        </>

    )
}