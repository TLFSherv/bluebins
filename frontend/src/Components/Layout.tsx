import Header from "./Header";
import Footer from "./Footer";
import { Outlet } from "react-router";

export default function Layout() {
    return (
        <div className="flex flex-col min-h-dvh bg-[#F3F6FB]">
            <Header />
            <div className="flex-1 flex flex-col">
                <Outlet />
            </div>
            <Footer />
        </div>
    )

}