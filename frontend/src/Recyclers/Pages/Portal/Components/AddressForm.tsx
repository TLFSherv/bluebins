import React, { useState } from "react";

export default function AddressForm() {
    var [showExtraFields, setShowExtraFields] = useState<boolean>(false);
    return (
        <div className="space-y-6">
            <h1 className="text-center text-2xl font-[Lato]">
                Booking Address
            </h1>
            <p className="text-center text-lg">
                Where can we collect your recycling?
            </p>
            <div>
                <div className="form-floating">
                    <input type="text" name="address" className="form-control-1" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Address</label>
                </div>
                <div className="flex justify-end space-x-2 text-sm pt-2">
                    <input type="checkbox" name="makeDefault" />
                    <label>
                        Make my default
                    </label>
                </div>
                <div className="space-y-3">
                    {showExtraFields && <ManualAddressFields />}
                    <button type="button" onClick={() => setShowExtraFields(prev => !prev)}>
                        {showExtraFields ? "- Hide manual fields" : "+ Enter address manually"}
                    </button>
                    <button type="button">+ Add additional information</button>
                </div>
            </div>
        </div>
    );
}

function ManualAddressFields() {
    return (
        <div className="space-y-6">
            <div className="form-floating">
                <input type="text" name="postcode" className="form-control-1 w-[100px]" />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Postcode</label>
            </div>
            <div className="form-floating">
                <textarea name="additionalInformation" className="form-control-1" />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Additional information</label>
            </div>
        </div>
    );
}