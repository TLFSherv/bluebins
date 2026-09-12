import { useState } from "react";
import AddressForm from "./components/AddressForm"
import DateForm from "./components/DateForm"
import QuantityForm from "./components/QuantityForm"

export default function Booking() {
    const [activeIndex, setActiveIndex] = useState<number>(0);
    const formComponents = [<AddressForm />, <QuantityForm />, <DateForm />];
    const COUNT = formComponents.length;
    const GAP = 4;
    const SCREEN = window.screen.width;
    let itemWidth = Math.round(SCREEN * 0.9);
    // Calculate offset to center the active item relative to the container
    const translateX = -activeIndex * (itemWidth + GAP);
    return (
        <form className="flex-1 flex flex-col items-center justify-center h-full w-full mx-auto space-y-8">
            <div style={{ width: (activeIndex + 1) / COUNT * itemWidth }}
                className="w-1/2 h-2 bg-linear-to-r from-indigo-500 from-10% via-sky-500 via-30% to-emerald-500 to-90% rounded-lg" />
            <div className="overflow-hidden w-full mx-auto">
                <div className="flex transition-transform" style={{ transform: `translateX(${translateX}px)`, gap: `${GAP}px` }}>
                    {formComponents.map((component, i) => (
                        <div key={i} style={{ width: `${itemWidth}px` }} className="mx-2 object-cover shrink-0 border-2 border-[#2496FA] rounded-xl px-2 py-4 space-y-6">
                            {component}
                            <div className="flex flex-row items-center gap-4 justify-center">
                                <button className="btn" type="button" onClick={() => setActiveIndex(prev => Math.max(prev - 1, 0))}>
                                    Back
                                </button>
                                <button className="btn" type="button" onClick={() => setActiveIndex(prev => Math.min(prev + 1, COUNT - 1))}>
                                    Next
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            </div>
        </form >
    );
} 