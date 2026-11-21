import { useEffect, useState, useRef } from "react";

export function BookingTimeline() {
    const [isDragging, setIsDragging] = useState(false);
    const sliderRef = useRef<HTMLDivElement>(null);
    const barRef = useRef<HTMLDivElement>(null);

    useEffect(() => {
        const slider = sliderRef.current;
        const sliderBar = barRef.current
        const dragMove = (e: MouseEvent) => {
            if (slider != null && sliderBar != null) {
                const min = sliderBar.offsetLeft;
                const max = sliderBar.offsetWidth + min - slider.offsetWidth;
                slider.style.left = Math.min(Math.max(min, e.clientX), max) + "px";
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
        { id: "31", date: "Oct 10 2026" },
        { id: "32", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "34", date: "Oct 10 2026" }];
    return (
        <div className="relative space-y-4">
            <div className="flex space-x-12 mx-auto w-4/5 overflow-x-hidden" >
                {bookings.map(b => <label>
                    #{b.id}<BookingTag bookingDate={b.date} />
                </label>)}
            </div >
            <div className="flex flex-col justify-center items-center">
                <div ref={barRef} className="absolute w-4/5 h-[2px] bg-[#2496FA]/42"></div>
                <div
                    ref={sliderRef}
                    onMouseDown={() => setIsDragging(true)}
                    className="absolute size-6 bg-[#D9D9D9] rounded-full z-1 cursor-pointer"></div>
            </div>
        </div>
    );
}


function BookingTag(props: { bookingDate: string }) {
    return (
        <p style={{ writingMode: "vertical-rl" }} className="px-2 py-1 bg-[#D9D9D9]">
            {props.bookingDate}
        </p>);
}