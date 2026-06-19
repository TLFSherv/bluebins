import FormTitle from "./FormTitle"

export default function AddressForm() {
    return (
        <div className="space-y-5">
            <FormTitle>Address</FormTitle>
            <p>Where do you want us to collect your recycling?</p>
            <div className="form-floating">
                <input type="text" name="address" className="form-control-1" autoComplete="off" />
                <label className="form-label bg-default -translate-y-8 translate-x-1 text-black text-base rounded-md p-1">Address</label>
            </div>
            <div>
                <div className="flex justify-end space-x-2 text-sm">
                    <input type="checkbox" name="makeDefault" />
                    <label>
                        Make my default
                    </label>
                </div>
                <div className="space-y-3">
                    <button type="button">+ Enter address manually</button>
                    <button type="button">+ Add additional information</button>
                </div>
            </div>
            <button className="w-full h-[48px] text-xl bg-linear-[88deg,#3511FB,#4AA5F6] text-white rounded-xl tracking-wide font-light" type="button">
                Next
            </button>
        </div>
    );
}