import { useState, useEffect } from "react";
import { type Booking, type Location, type Recycling, type Schedule } from "../types/types";
import { BookingService } from "../services/BookingService";
import toast from "react-hot-toast";

const initBooking: Booking = {
    location: {
        type: "location",
        address: "",
        details: "",
        postcode: "",
        parish: "hamilton parish",
        latitude: 1,
        longitude: 1,
        makeDefault: false
    },
    schedule: {
        type: "schedule",
        date: new Date(2026, 10, 26),
        frequency: "Once",
        makeDefault: false
    },
    recycling: {
        type: "recycling",
        numberOfBags: 1,
        materialQuantities: {
            tin: 0,
            aluminium: 0,
            glass: 0
        }
    }
};

const KEY_MAP = {
    location: "location",
    recycling: "recycling",
    schedule: "schedule",
} as const;

export function useBookingHandler() {
    const [booking, setBooking] = useState<Booking>(initBooking);

    useEffect(() => {
        const getUserBooking = async () => {
            // Check cache if current booking is there 
            // If cache is empty get booking from api
            const result = await BookingService.getBooking();
            if (!result.success) {
                toast.error(result.message);
                return;
            }
            setBooking(result.result ?? initBooking);
            console.log(`Successfully got user booking`);
            console.log(result);
        }
        getUserBooking();
    }, [])

    const setInput = (input: Location | Recycling | Schedule) => {
        // Set state for the correct Booking property object
        const key = KEY_MAP[input.type];
        setBooking(prev => ({ ...prev, [key]: input }));
    };
    return { booking, setInput };
}