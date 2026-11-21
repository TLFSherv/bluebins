import { useEffect, useState, useRef } from "react";

export function BookingTimeline() {
    const [selectedBooking, setSelectedBooking] = useState<string>();
    const [isDragging, setIsDragging] = useState(false);
    const sliderRef = useRef<HTMLDivElement>(null);
    const sliderBarRef = useRef<HTMLDivElement>(null);
    const scrollWindowRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const slider = sliderRef.current;
        const sliderBar = sliderBarRef.current
        const scrollWindow = scrollWindowRef.current;
        const dragMove = (e: MouseEvent) => {
            if (slider != null && sliderBar != null) {
                const sliderRadius = slider.offsetWidth / 2;
                const min = sliderBar.offsetLeft + sliderRadius;
                const max = sliderBar.offsetWidth + min - sliderRadius * 2;
                const sliderPosition = Math.min(Math.max(min, e.clientX), max) - sliderRadius;
                slider.style.left = sliderPosition + "px";
                scrollWindow?.scroll({ left: sliderPosition, behavior: "instant" })
            }
        }
        const stopDrag = () => {
            if (!isDragging) return;
            setIsDragging(false);
        }

        if (isDragging) {
            // move element when mouse moves
            document.addEventListener("mousemove", dragMove);
            // stop dragging when the mouse is up
            document.addEventListener("mouseup", stopDrag);
        }

        // stop moving slider
        return () => {
            document.removeEventListener("mousemove", dragMove)
            document.removeEventListener("mouseup", stopDrag)
        }

    }, [isDragging]);

    var bookings = [
        { id: "31", date: "10/10/2026" },
        { id: "32", date: "10/10/2026" },
        { id: "33", date: "10/10/2026" },
        { id: "34", date: "10/10/2026" },
        { id: "35", date: "10/10/2026" },
        { id: "36", date: "10/10/2026" },
        { id: "37", date: "10/10/2026" },
        { id: "38", date: "10/10/2026" },
        { id: "39", date: "10/10/2026" },
        { id: "40", date: "10/10/2026" },
        { id: "41", date: "10/10/2026" },
        { id: "42", date: "10/10/2026" }];
    return (
        <div className="relative space-y-6 text-sm">
            <div ref={scrollWindowRef} className="flex space-x-12 mx-auto w-4/5 overflow-x-hidden" >
                {bookings.map(b => <label>
                    #{b.id}
                    <p
                        onClick={() => setSelectedBooking(b.id)}
                        style={{ writingMode: "vertical-rl" }}
                        className={`px-2 py-1 cursor-pointer ${selectedBooking == b.id ? "bg-[#3CC4FA]/50" : "bg-[#D9D9D9]"}`}>
                        {b.date}
                    </p>
                </label>)}
            </div >
            <div className="flex flex-col justify-center items-center">
                <div ref={sliderBarRef} className="absolute w-4/5 h-[2px] bg-[#2496FA]/42"></div>
                <div
                    ref={sliderRef}
                    onMouseDown={() => setIsDragging(true)}
                    className="absolute size-10 bg-[#D9D9D9]/60 rounded-full z-1 cursor-pointer"></div>
            </div>
        </div>
    );
}