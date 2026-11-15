import { useState } from "react";
import { Reorder } from "framer-motion";

export function NavMenu() {
    const [navItems, setNavItems] = useState([
        "My bookings",
        "My recycling",
        "My points",
        "My neighbours"
    ]);

    const handleItemClick = (clickedItem: string) => {
        // Move the clicked item to the front of the array
        const reordered = [
            clickedItem,
            ...navItems.filter((item) => item !== clickedItem)
        ];
        setNavItems(reordered);
    };

    return (
        <div>
            <nav className="flex justify-center font-inter font-light text-sm/7">
                {/* Replace <ul> with Reorder.Group */}
                <Reorder.Group
                    axis="y"
                    values={navItems}
                    onReorder={setNavItems}
                    className="space-y-1 w-3/4 text-center list-none"
                >
                    {navItems.map((item, index) => (
                        <Reorder.Item
                            key={item}
                            value={item}
                            onClick={() => handleItemClick(item)}
                            className={`rounded-xl h-7 cursor-pointer select-none flex items-center justify-center transition-colors duration-200
                                ${index === 0 ? "bg-[#2496FA]/42 text-base/7 font-medium" : ""}`}
                        >
                            {item}
                        </Reorder.Item>
                    ))}
                </Reorder.Group>
            </nav>
        </div>
    );
}