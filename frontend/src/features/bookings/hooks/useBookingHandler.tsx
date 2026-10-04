import { useState } from "react";
import { type Booking, type Location, type Quantity, type Schedule } from "../types/types";

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

    const setInput = (input: Location | Quantity | Schedule) => {
        // Set state for the correct Booking property object
        const key = KEY_MAP[input.type];
        setBooking(prev => ({ ...prev, [key]: input }));
    };
    return { booking, setInput };
}