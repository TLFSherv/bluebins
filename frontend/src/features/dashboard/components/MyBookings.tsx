import { BookingTimeline } from "./BookingTimeline"

export default function MyBookings() {
    return (
        <div className="space-y-6">
            <BookingTimeline />
            <div className="w-[90%] mx-auto space-y-4 font-inter text-sm">
                <h1 className="text-2xl">Booking #31</h1>
                <div className="grid grid-cols-2 gap-x-8 gap-y-5">
                    <div className="col-span-2 grid grid-cols-subgrid">
                        <div className="space-y-1">
                            <p><span className="font-medium">Address:</span> 8 Link Lane</p>
                            <p><span className="font-medium">Postcode:</span> CR03</p>
                            <p><span className="font-medium">Parish:</span> Hamilton parish</p>
                        </div>
                        <div className="space-y-1">
                            <p className="font-medium">Additional information</p>
                            <p>It's the blue house.</p>
                        </div>
                    </div>

                    <div className="col-span-2">
                        <p><span className="font-medium">Date collected:</span> Oct 10 2026</p>
                    </div>

                    <div className="col-span-2 grid grid-cols-subgrid">
                        <p><span className="font-medium">Number of bags:</span> 2</p>
                        <p><span className="font-medium">Bag volume:</span> 2 litres</p>
                    </div>

                    <div className="col-span-2 space-y-1">
                        <p><span className="font-medium">Tin quantity:</span> not given</p>
                        <p><span className="font-medium">Aluminium quantity:</span> not given</p>
                        <p><span className="font-medium">Glass quantity:</span> not given</p>
                    </div>
                </div>
                <div className="space-x-8 font-inter text-sm">
                    <button className="bg-[#F07070] text-white p-2">Delete</button>
                    <button className="bg-[#FF8001]/62 text-white p-2">Cancel</button>
                </div>
            </div >
        </div >
    );
}