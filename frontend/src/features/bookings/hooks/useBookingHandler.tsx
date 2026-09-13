import { useState } from "react";
import { type BookingInputs } from "../types/types";

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
    const setInput = (event: React.ChangeEvent) => {
        const target = event.target as (EventTarget & HTMLInputElement) | (EventTarget & HTMLTextAreaElement)
        let name = target.name;
        let value: string | number = target.value;
        if (target.type == "number" && Number.isInteger(value)) {
            value = Number.parseInt(value);
        }
        if (bookingInputs != undefined) {
            // Set state for the correct BookingInputs property object
            setBookingInputs((prev) => {
                let newInput = {};
                for (const [key, value] of Object.entries(prev)) {
                    // Find the property the input is for  
                    // then create a new property and return in new BookingInputs instance
                    if (Object.hasOwn(value, name)) {
                        newInput = { ...value, [name]: value };
                        return { ...prev, [key]: newInput };
                    }
                    else {
                        let materialQuantity = prev.quantityInputs.materialQuantity;
                        if (Object.hasOwn(materialQuantity, name)) {
                            newInput = { ...materialQuantity, [name]: value };
                            const newQuantityInputs = {
                                ...prev.quantityInputs,
                                materialQuantity: { ...materialQuantity, [name]: value }
                            };
                            return { ...prev, quantityInputs: newQuantityInputs };
                        }
                    }
                }
                return prev;
            });
        }
    };
    return { bookingInputs, setInput };

}