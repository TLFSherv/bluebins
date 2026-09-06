import { useState } from "react";
import AddressForm from "./components/AddressForm"
import DateForm from "./components/DateForm"
import QuantityForm from "./components/QuantityForm"

export default function Booking() {
    var [activeForm, setActiveForm] = useState<number>(0);
    var formList = [
        <AddressForm />,
        <QuantityForm />,
        <DateForm />];
    return (
        <div>
            <form className="p-4 space-y-12">
                {formList[activeForm]}
                <div className="space-y-4">
                    <button onClick={() => setActiveForm(prev => prev + 1)}
                        className="w-full h-[48px] text-xl bg-[#0088FF] text-white rounded-xl tracking-wider font-light" type="button">
                        {activeForm + 1 == formList.length ? "Done" : "Next"}
                    </button>
                    {activeForm > 0 && <button
                        onClick={() => setActiveForm(prev => prev - 1)}
                        className="w-full h-[48px] text-xl border-2 border-[#0088FF] rounded-xl tracking-wider font-light" type="button">
                        Back
                    </button>}
                </div>
            </form>
        </div>
    );
}