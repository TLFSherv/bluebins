import { createContext, useEffect, useState } from "react";
import { authService } from "../Recyclers/Services/AuthService";

export const UserContext = createContext<{
    isSignedIn: boolean,
    setIsSignedIn: React.Dispatch<React.SetStateAction<boolean>>
}>({ isSignedIn: false, setIsSignedIn: () => { } });

export default function UserContextProvider({ children }: React.PropsWithChildren) {
    const [isSignedIn, setIsSignedIn] = useState(false);
    useEffect(() => {
        const checkSignInStatus = async () => {
            const result = await authService.isSignedIn();
            setIsSignedIn(result);
        }
        checkSignInStatus();
    }, []);
    return (
        <UserContext value={{ isSignedIn: isSignedIn, setIsSignedIn: setIsSignedIn }}>
            {children}
        </UserContext>
    )
}