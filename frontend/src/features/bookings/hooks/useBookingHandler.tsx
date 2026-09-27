import { useState } from "react";
import { type Booking, type Address, type Quantity, type Date } from "../types/types";

const initBooking: Booking = {
    location: {
        type: "location",
        address: "",
        details: "",
        postcode: "",
        parish: "hamilton parish",
        latitude: 0,
        longitude: 0,
        makeDefault: 0
    },
    date: {
        type: "date",
        date: ["Mon", 1, "Oct"],
        frequency: "bi-weekly"
    },
    quantity: {
        type: "quantity",
        quantity: 1,
        materialQuantity: {
            tin: 0,
            aluminium: 0,
            glass: 0
        }
    }
};

const KEY_MAP = {
    address: "address",
    quantity: "quantity",
    date: "date",
} as const;

export function useBookingHandler() {
    const [booking, setBooking] = useState<Booking>(initBooking);

    const setInput = (input: Address | Quantity | Date) => {
        // Set state for the correct Booking property object
        const key = KEY_MAP[input.type];
        setBooking(prev => ({ ...prev, [key]: input }));
    };
    return { booking, setInput };
}