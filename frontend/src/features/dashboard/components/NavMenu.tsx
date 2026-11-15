import { useState } from "react";

export function NavMenu() {
    const [activeIndex, setActiveIndex] = useState<number>(0);
    var navItems = ["My bookings", "My recycling", "My points", "My neighbours"];
    const handleClick = (index: number) => {
        setActiveIndex(index);
    }
    return (
        <div>
            <nav className="flex justify-center font-inter font-light text-sm/7">
                <ul className="space-y-1 w-3/4 text-center align-bottom ">
                    {navItems.map((item, i) =>
                        <li key={i}
                            className={`rounded-xl h-7 ${(i == activeIndex) ? "bg-[#2496FA]/42 text-base/7" : ""}`}
                            onClick={() => handleClick(i)}>
                            {item}
                        </li>
                    )}
                </ul>
            </nav>
        </div>
    );
}

