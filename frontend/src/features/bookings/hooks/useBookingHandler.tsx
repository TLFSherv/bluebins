import { useState } from "react";
import { type BookingInputs, type AddressInputs, type QuantityInputs, type DateInputs } from "../types/types";

const initBookingInputs: BookingInputs = {
    addressInputs: {
        address: "",
        additionalInfo: "",
        postcode: "",
        parish: "",
        latitude: 0,
        longitude: 0
    },
    dateInputs: {
        dayOfWeek: "Mon",
        dayNumber: 1,
        month: "Oct",
        frequency: "bi-weekly"
    },
    quantityInputs: {
        quantity: 1,
        materialQuantity: {
            tin: 0,
            aluminium: 0,
            glass: 0
        }
    }
};
export function useBookingHandler() {
    const [bookingInputs, setBookingInputs] = useState<BookingInputs>(initBookingInputs);
    const setInput = (input: AddressInputs | QuantityInputs | DateInputs) => {
        // Set state for the correct BookingInputs property object
        let key: string;
        if ("address" in input)
            key = "addressInputs";
        else if ("dayOfWeek" in input)
            key = "dateInputs";
        else if ("quantity" in input)
            key = "quantityInputs";
        console.log(input);

        setBookingInputs(prev => ({ ...prev, [key]: input }));
    };
    // console.log(bookingInputs.addressInputs);
    return { bookingInputs, setInput };
}