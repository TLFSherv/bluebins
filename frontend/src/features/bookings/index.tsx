import { useState } from "react";
import AddressForm from "./components/AddressForm"
import DateForm from "./components/DateForm"
import QuantityForm from "./components/QuantityForm"

export default function Booking() {
    return (
        <div>
            <form className="p-4 space-y-12">
                <AddressForm />
                <QuantityForm />
                <DateForm />
            </form>
        </div>
    );
}