import { useState } from "react";

export default function AddressForm() {
    const [isFullAddressFormVisible, setIsFullAddressFormVisible] = useState<boolean>(false);
    const [isAdditionalInfoVisible, setIsAdditionalInfoVisible] = useState<boolean>(false);
    return (
        <div className="space-y-6">
            <h1 className="text-center text-2xl font-[Lato]">
                Booking Address
            </h1>
            <p className="text-center text-lg">
                Where can we collect your recycling?
            </p>
            <div className="space-y-3">
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
                </div>
                {isAdditionalInfoVisible &&
                    <div className="form-floating">
                        <textarea name="additionalInformation" className="form-control-1" />
                        <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Additional information</label>
                    </div>}
                <div className="space-y-3">
                    {isFullAddressFormVisible && <FullAddressForm />}
                    <button type="button" onClick={() => setIsAdditionalInfoVisible(prev => !prev)}>
                        {isAdditionalInfoVisible ? "- Hide additional information" : "+ Add additional information"}
                    </button>
                    <button type="button" onClick={() => setIsFullAddressFormVisible(prev => !prev)}>
                        {isFullAddressFormVisible ? "- Hide manual fields" : "+ Enter address manually"}
                    </button>
                </div>
            </div>
        </div>
    );
}

function FullAddressForm() {
    return (
        <div className="space-y-6">
            <div className="form-floating">
                <input type="text" name="postcode" className="form-control-1 w-[100px]" />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Postcode</label>
            </div>
            <div className="form-floating">
                <input type="text" name="postcode" className="form-control-1 w-[150px]" />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Parish</label>
            </div>
        </div>
    );
}