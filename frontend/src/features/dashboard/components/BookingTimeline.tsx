import { useEffect, useState, useRef } from "react";
import { type BookingDates } from "../types/types";
import type { Booking } from "../../bookings/types/types";


export function BookingTimeline(props: { bookingDates: BookingDates[], selectedBooking: Booking | null, showSelectedBooking: (bookingId: number) => Promise<void> }) {
    const [isDragging, setIsDragging] = useState(false);
    const sliderRef = useRef<HTMLDivElement>(null);
    const sliderBarRef = useRef<HTMLDivElement>(null);
    const scrollWindowRef = useRef<HTMLDivElement>(null);

    const { bookingDates, selectedBooking, showSelectedBooking } = props;
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

    return (
        <div className="relative space-y-6 text-sm">
            <div ref={scrollWindowRef} className="flex space-x-12 mx-auto w-4/5 overflow-x-hidden" >
                {bookingDates.map(bd => {
                    var bgColor = bookingDates[bookingDates.length - 1].bookingId == bd.bookingId ? "bg-[#34C759]/60" : "bg-[#D9D9D9]";
                    if (selectedBooking?.bookingId == bd.bookingId) {
                        bgColor = "bg-[#3CC4FA]/50";
                    }
                    return <label key={bd.bookingId}>
                        #{bd.bookingId}
                        <p
                            onClick={async () => await showSelectedBooking(bd.bookingId)}
                            style={{ writingMode: "vertical-rl" }}
                            className={`px-2 py-1 cursor-pointer ${bgColor}`}>
                            {bd.bookingDate}
                        </p>
                    </label>
                })}
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