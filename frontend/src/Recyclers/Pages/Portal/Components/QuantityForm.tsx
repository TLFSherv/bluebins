export default function QuantityForm() {
    return (
        <div className="space-y-7">
            <h1 className="text-center text-2xl font-[Lato]">
                Quantity & Contents
            </h1>
            <p className="text-center text-lg"> How many bags are we collecting?</p>
            <div className="flex justify-evenly">
                <div className="form-floating">
                    <input type="number" name="quantity" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">Quantity</label>
                </div>
                <button type="button" className="bg-[#3CC4FA] text-lg text-white w-[80px] h-[45px] rounded-lg">+</button>
                <button type="button" className="bg-[#3CC4FA] text-lg text-white w-[80px] h-[45px] rounded-lg">-</button>
            </div>
            <button type="button">+ Add contents</button>
        </div>
    );
}