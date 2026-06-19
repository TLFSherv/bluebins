import FormTitle from "./FormTitle";

export default function QuantityForm() {
    return (
        <div className="space-y-5">
            <FormTitle>Quantity & Contents</FormTitle>
            <p> How many bags do you want use to collect?</p>
            <div className="flex justify-evenly">
                <div className="form-floating">
                    <input type="number" name="quantity" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">Quantity</label>
                </div>
                <button type="button" className="bg-[#0088FF] text-lg text-white w-[80px] h-[45px] rounded-lg">+</button>
                <button type="button" className="bg-[#0088FF] text-lg text-white w-[80px] h-[45px] rounded-lg">-</button>
            </div>
            <button type="button">+ Add contents</button>
            <button className="w-full h-[48px] text-xl bg-linear-[88deg,#3511FB,#4AA5F6] text-white rounded-xl tracking-wide font-light" type="button">
                Next
            </button>
        </div>
    );
}