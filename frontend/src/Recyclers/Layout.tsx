import type React from "react";
import Header from "./Components/Header";
import Footer from "./Components/Footer";

export default function Layout({ children }: React.PropsWithChildren) {
    return (
        <div className="flex flex-col min-h-dvh bg-[#F3F6FB]">
            <Header />
            <div className="flex-1 flex flex-col">
                {children}
            </div>
            <Footer />
        </div>
    )

}