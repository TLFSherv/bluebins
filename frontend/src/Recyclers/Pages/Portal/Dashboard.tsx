import { useEffect, useState } from "react"
export default function Dashboard() {
    const [profile, setProfile] = useState(null);
    useEffect(() => {
        const fetchUser = async () => {
            try {
                console.log("Hello");
                const backendUrl = import.meta.env.VITE_SERVER_URL;
                const response = await fetch(`${backendUrl}/manage/info`,
                    {
                        method: "GET",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        credentials: "include"
                    }
                )
                if (response.ok) {
                    const data = await response.json();
                    console.log(data);
                    setProfile(data);
                }
                else {
                    throw new Error(response.statusText);
                }
            } catch (error: any) {
                console.log(error.message);
            }
        }
        fetchUser();
    }, []);
    return (
        <div>
            <h1>User Dashboard</h1>
        </div>
    )
}