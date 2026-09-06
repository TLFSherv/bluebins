import React, { useState } from "react";

export default function QuantityForm() {
    const [isRecyclingItemsFormVisible, setIsRecyclingItemsFormVisible] = useState<boolean>(false);
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
                <button type="button" className="quantity-btn w-[80px] h-[45px] rounded-lg">+</button>
                <button type="button" className="quantity-btn w-[80px] h-[45px] rounded-lg">-</button>
            </div>
            {isRecyclingItemsFormVisible && <RecyclingItemsForm />}
            <button type="button" onClick={() => setIsRecyclingItemsFormVisible(prev => !prev)}>
                {isRecyclingItemsFormVisible ? "- Hide contents" : "+ Add contents"}
            </button>
            <div className="space-y-4  justify-center">
                <button className="btn" type="button">
                    Next
                </button>
                <button className="btn" type="button">
                    Back
                </button>
            </div>
        </div>
    );
}

function RecyclingItemsForm() {
    return (
        <div className="space-y-7">
            <div className="flex justify-evenly">
                <div className="form-floating">
                    <input type="number" name="quantity_tin" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">tin</label>
                </div>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">-</button>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">+</button>
            </div>
            <div className="flex justify-evenly">
                <div className="form-floating">
                    <input type="number" name="quantity_aluminium" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">aluminium</label>
                </div>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">+</button>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">-</button>
            </div>
            <div className="flex justify-evenly">
                <div className="form-floating">
                    <input type="number" name="quantity_glass" className="form-control-1 w-[112px] h-[45px] text-center" autoComplete="off" />
                    <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">glass</label>
                </div>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">+</button>
                <button type="button" className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">-</button>
            </div>
        </div>
    );
}