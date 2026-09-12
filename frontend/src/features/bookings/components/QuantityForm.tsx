import { useState } from "react";
import { Materials } from "../types/types";

export default function QuantityForm() {
    const [isRecyclingItemsFormVisible, setIsRecyclingItemsFormVisible] = useState<boolean>(false);
    return (
        <div>
            <div className="space-y-6">
                <h1 className="text-center text-3xl font-[Lato]">
                    Quantity & Contents
                </h1>
                <p className="text-center text-lg"> How many bags are we collecting?</p>
            </div>
            <div style={{ height: isRecyclingItemsFormVisible ? "170px" : "130px" }} className={`space-y-7 overflow-y-scroll px-2 pt-10 scrollbar-thin ${isRecyclingItemsFormVisible ? "shadow-lg rounded-xl" : ""}`}>
                <div className="flex justify-evenly">
                    <div className="form-floating">
                        <input type="number" name="quantity" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                        <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">Quantity</label>
                    </div>
                    <button type="button" className="quantity-btn w-[80px] h-[45px] rounded-lg">+</button>
                    <button type="button" className="quantity-btn w-[80px] h-[45px] rounded-lg">-</button>
                </div>
                {isRecyclingItemsFormVisible && <RecyclingItemsForm />}
            </div>
            <button type="button" className="pl-2" onClick={() => setIsRecyclingItemsFormVisible(prev => !prev)}>
                {isRecyclingItemsFormVisible ? "- Hide contents" : "+ Add contents"}
            </button>
        </div>
    );
}

function RecyclingItemsForm() {
    return (
        <div className="space-y-7">
            {Materials.map(material =>
            (
                <div id={material} className="flex justify-evenly">
                    <div className="form-floating">
                        <input type="number" name={`quantity_${material}`} className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                        <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">{material}</label>
                    </div>
                    <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">-</button>
                    <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">+</button>
                </div>
            )
            )}
        </div>
    );
}