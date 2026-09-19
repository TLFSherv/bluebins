import { useState } from "react";
import { type BookingInputs, type AddressInputs, type QuantityInputs, type DateInputs } from "../types/types";

const initBookingInputs: BookingInputs = {
    addressInputs: {
        type: "address",
        address: "",
        additionalInfo: "",
        postcode: "",
        parish: "",
        latitude: 0,
        longitude: 0,
        makeDefault: 0
    },
    dateInputs: {
        type: "date",
        dayOfWeek: "Mon",
        dayNumber: 1,
        month: "Oct",
        frequency: "bi-weekly"
    },
    quantityInputs: {
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
    address: "addressInputs",
    quantity: "quantityInputs",
    date: "dateInputs",
} as const;

export function useBookingHandler() {
    const [bookingInputs, setBookingInputs] = useState<BookingInputs>(initBookingInputs);
    const setInput = (input: AddressInputs | QuantityInputs | DateInputs) => {
        // Set state for the correct BookingInputs property object
        const key = KEY_MAP[input.type];
        setBookingInputs(prev => ({ ...prev, [key]: input }));
    };
    return { bookingInputs, setInput };
}