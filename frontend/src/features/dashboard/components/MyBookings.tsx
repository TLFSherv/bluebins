import { BookingTimeline } from "./BookingTimeline"
import { useEffect, useState } from "react";
import { DashboardService } from "../services/DashboardServices";
import type { Booking } from "../../bookings/types/types";
import type { BookingDates } from "../types/types";
import { toast } from "react-hot-toast";

export default function MyBookings() {
    const [bookingDates, setBookingDates] = useState<BookingDates[]>([]);
    const [selectedBooking, setSelectedBooking] = useState<Booking | null>(null);
    // get booking dates
    useEffect(() => {
        const showBookingDates = async () => {
            const response = await DashboardService.getBookingDates();
            if (response.success) {
                setBookingDates(response.result ?? [])
            }
            else {
                // show error to user
                toast.error(response.message);
            }
        }
        showBookingDates();
    }, []);
    // get booking data
    const showSelectedBooking = async (bookingId: number) => {
        const response = await DashboardService.getBooking(bookingId);
        if (response.success) {
            setSelectedBooking(response.result as Booking);
        }
        else {
            toast.error(response.message);
        }
    }
    const bookingData = { bookingDates, selectedBooking, showSelectedBooking };
    return (
        <div className="space-y-10">
            <BookingTimeline {...bookingData} />
            <div className="w-[90%] mx-auto space-y-4 font-inter">
                <div className="flex space-x-4 items-center">
                    <h1 className="text-2xl">Booking #{selectedBooking?.bookingId}</h1>
                    <button className="bg-[#5CDA02] rounded-xl text-sm text-white px-3 h-6 tracking-wider cursor-pointer">track</button>
                </div>
                <div className="grid grid-cols-2 gap-x-8 gap-y-5">
                    <div className="col-span-2 grid grid-cols-subgrid">
                        <div className="space-y-1">
                            <p><span className="font-medium">Address:</span> {selectedBooking?.location.address}</p>
                            <p><span className="font-medium">Postcode:</span> {selectedBooking?.location.postcode}</p>
                            <p><span className="font-medium">Parish:</span> {selectedBooking?.location.parish}</p>
                        </div>
                        <div className="space-y-1">
                            <p className="font-medium">Additional information</p>
                            <p>{selectedBooking?.location.details}</p>
                        </div>
                    </div>

                    <div className="col-span-2">
                        <p><span className="font-medium">Date collected:</span> {selectedBooking?.bookingDate.toString()}</p>
                    </div>

                    <div className="col-span-2 grid grid-cols-subgrid">
                        <p><span className="font-medium">Number of bags:</span> {selectedBooking?.recycling.numberOfBags}</p>
                        <p><span className="font-medium">Bag volume:</span> Not given</p>
                    </div>

                    <div className="col-span-2 space-y-1">
                        <p><span className="font-medium">Tin quantity:</span> {selectedBooking?.recycling.materialQuantities.tin}</p>
                        <p><span className="font-medium">Aluminium quantity:</span> {selectedBooking?.recycling.materialQuantities.aluminium}</p>
                        <p><span className="font-medium">Glass quantity:</span> {selectedBooking?.recycling.materialQuantities.glass}</p>
                    </div>
                </div>
                <div className="space-x-6">
                    <button className="bg-[#F07070] text-white p-2 cursor-pointer">Delete</button>
                    <button className="bg-[#FF8001]/62 text-white p-2 cursor-pointer">Cancel</button>
                </div>
            </div >
        </div >
    );
}