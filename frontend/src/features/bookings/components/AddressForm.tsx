import { useState } from "react";
import { type AddressInputs } from "../types/types";

export default function AddressForm({ inputs, setInput }: {
    inputs: AddressInputs,
    setInput: (input: AddressInputs) => void
}) {
    const [isFullAddressFormVisible, setIsFullAddressFormVisible] = useState<boolean>(false);
    const [isAdditionalInfoVisible, setIsAdditionalInfoVisible] = useState<boolean>(false);
    let isShowingMore = isAdditionalInfoVisible || isFullAddressFormVisible;
    const { address, additionalInfo, postcode, parish } = inputs;
    const handleChange = (e: React.ChangeEvent) => {
        const target = e.target as HTMLInputElement | HTMLTextAreaElement;
        const name = target.name;
        const value = target.value;
        setInput({ ...inputs, [name]: value });
    }
    return (
        <div>
            <div className="space-y-6">
                <h1 className="text-center text-3xl font-[Lato]">
                    Booking Address
                </h1>
                <p className="text-center text-lg">
                    Where can we collect your recycling?
                </p>
            </div>
            <div style={{ height: isShowingMore ? "170px" : "130px" }} className={`space-y-3 overflow-y-scroll px-2 pt-8 scrollbar-thin ${isShowingMore ? "shadow-lg rounded-xl" : ""}`}>
                <div className="form-floating">
                    <input
                        type="text"
                        name="address"
                        className="form-control-1"
                        autoComplete="off"
                        value={address}
                        onChange={e => handleChange(e)} />
                    <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-lg rounded-md p-1">Address</label>
                </div>
                <div className="flex justify-end space-x-2 text-sm">
                    <input type="checkbox" name="makeDefault" />
                    <label>
                        Make my default
                    </label>
                </div>
                <div className="space-y-6">
                    {isAdditionalInfoVisible &&
                        <div className="form-floating">
                            <textarea
                                name="additionalInfo"
                                className="form-control-1"
                                value={additionalInfo}
                                onChange={e => handleChange(e)} />

                            <label className="form-label bg-default -translate-y-8 translate-x-1 text-black rounded-md p-1 text-lg">Additional information</label>
                        </div>}
                    {isFullAddressFormVisible && <FullAddressForm inputs={{ postcode, parish }}
                        handleChange={handleChange} />}
                </div>
            </div>
            <div className="space-y-2">
                <button type="button" onClick={() => setIsAdditionalInfoVisible(prev => !prev)}>
                    {isAdditionalInfoVisible ? "- Hide additional information" : "+ Add additional information"}
                </button>
                <button type="button" onClick={() => setIsFullAddressFormVisible(prev => !prev)}>
                    {isFullAddressFormVisible ? "- Hide manual fields" : "+ Enter address manually"}
                </button>
            </div>
        </div>
    );
}

function FullAddressForm({ inputs, handleChange }: {
    inputs: { postcode: string, parish: string },
    handleChange: (event: React.ChangeEvent) => void
}) {
    return (
        <div className="space-y-6">
            <div className="form-floating">
                <input
                    type="text"
                    name="postcode"
                    className="form-control-1 w-[100px]"
                    value={inputs.postcode}
                    onChange={e => handleChange(e)} />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-lg rounded-md p-1">Postcode</label>
            </div>
            <div className="form-floating">
                <input
                    type="text"
                    name="parish"
                    className="form-control-1 w-[150px]"
                    value={inputs.parish}
                    onChange={e => handleChange(e)} />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-lg rounded-md p-1">Parish</label>
            </div>
        </div>
    );
}