export function BookingTimeline() {
    var bookings = [
        { id: "31", date: "Oct 10 2026" },
        { id: "32", date: "Oct 10 2026" },
        { id: "33", date: "Oct 10 2026" },
        { id: "34", date: "Oct 10 2026" }];
    return (
        <div className="relative space-y-4">
            <div className="flex justify-evenly mt-8" >
                {bookings.map(b => <label>
                    #{b.id}<BookingTag bookingDate={b.date} />
                </label>)}
            </div >
            <div className="flex flex-col justify-center items-center">
                <div className="absolute w-4/5 h-[2px] bg-[#2496FA]/42"></div>
                <div className="size-6 bg-[#D9D9D9] rounded-full z-1"></div>
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