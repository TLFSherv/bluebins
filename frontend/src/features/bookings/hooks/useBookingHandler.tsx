import { useState, useEffect } from "react";
import { type Booking, type Location, type Quantity, type Schedule } from "../types/types";
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
        displayDate: ["Mon", 1, "Oct", 2026],
        date: "",
        frequency: "Once",
        makeDefault: false
    },
    quantity: {
        type: "quantity",
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
    quantity: "quantity",
    schedule: "schedule",
} as const;

export function useBookingHandler() {
    const [booking, setBooking] = useState<Booking>(initBooking);

    useEffect(() => {
        const getUserBooking = async () => {
            // Check cache if current booking is there 
            // If cache is empty get booking from api
            const result = await BookingService.get<Booking>();
            if (!result.success) {
                toast.error(result.message);
                return;
            }
            setBooking(result.result ?? initBooking);
            console.log(`Successfully got user booking`);
        }
        getUserBooking();
    }, [])

    const setInput = (input: Location | Quantity | Schedule) => {
        // Set state for the correct Booking property object
        const key = KEY_MAP[input.type];
        setBooking(prev => ({ ...prev, [key]: input }));
    };
    return { booking, setInput };
}