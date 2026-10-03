import { useState } from "react";
import { MATERIALS, type Quantity } from "../types/types";

export default function QuantityForm({ inputs, setInput }:
    {
        inputs: Quantity,
        setInput: (input: Quantity) => void
    }) {
    const [isMaterialsFormVisible, setIsMaterialsFormVisible] = useState<boolean>(false);

    function handleChange(e: React.ChangeEvent<HTMLInputElement> | React.MouseEvent<HTMLButtonElement>) {
        const target = e.currentTarget;
        const { name, value } = target;

        const parsedValue = Number.parseInt(value, 10) || 0;
        const isClick = e.type === "click";

        if (name in inputs) {
            const currentValue = Number(inputs.numberOfBags) || 0;
            let newValue = isClick ? parsedValue + currentValue : value;
            const validatedValue = Number.isInteger(newValue) ? Math.max(Number(newValue), 1) : newValue;
            setInput({ ...inputs, [name]: validatedValue });
        }
        else {
            const key = name as keyof typeof inputs.materialQuantities;
            const currentQty = Number(inputs.materialQuantities[key]) || 0;
            const newValue = isClick ? parsedValue + currentQty : value;
            const validatedValue = Number.isInteger(newValue) ? Math.max(Number(newValue), 0) : newValue;

            const newMaterialQuantity = { ...inputs.materialQuantities, [name]: validatedValue };
            setInput({ ...inputs, materialQuantities: newMaterialQuantity });
        }
    }

    return (
        <div>
            <div className="space-y-6">
                <h1 className="text-center text-3xl font-[Lato]">
                    Quantity & Contents
                </h1>
                <p className="text-center text-lg"> How many bags are we collecting?</p>
            </div>
            <div style={{ height: isMaterialsFormVisible ? "170px" : "130px" }} className={`space-y-7 overflow-y-scroll px-2 pt-10 scrollbar-thin ${isMaterialsFormVisible ? "shadow-lg rounded-xl" : ""}`}>
                <div className="flex justify-evenly">
                    <div className="form-floating">
                        <input
                            type="number"
                            name="numberOfBags"
                            className="form-control-1 w-[112px] h-[45px] text-center"
                            autoComplete="off"
                            value={inputs.numberOfBags}
                            onKeyDown={(e) => {
                                // Block '-', '+', and 'e' / 'E'
                                if (["-", "+", "e", "E"].includes(e.key)) {
                                    e.preventDefault();
                                }
                            }}
                            onChange={e => {
                                const rawValue = e.target.value;
                                // Reject anything that isn't a positive digit or empty string
                                const val = Number.parseInt(rawValue, 10);
                                if (val > 0 || rawValue == "") {
                                    handleChange(e);
                                }
                            }}
                            min={1} />
                        <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">Quantity</label>
                    </div>
                    <button
                        name="numberOfBags"
                        value={1}
                        type="button"
                        className="quantity-btn w-[80px] h-[45px] rounded-lg"
                        onClick={(e) => handleChange(e)}>+</button>
                    <button
                        name="numberOfBags"
                        value={-1}
                        type="button"
                        className="quantity-btn w-[80px] h-[45px] rounded-lg"
                        onClick={(e) => handleChange(e)}>-</button>
                </div>
                {isMaterialsFormVisible && <MaterialsForm inputs={inputs} handleChange={handleChange} />}
            </div>
            <button type="button" className="pl-2" onClick={() => setIsMaterialsFormVisible(prev => !prev)}>
                {isMaterialsFormVisible ? "- Hide contents" : "+ Add contents"}
            </button>
        </div>
    );
}

function MaterialsForm({ inputs, handleChange }:
    {
        inputs: Quantity,
        handleChange: (event: React.ChangeEvent<HTMLInputElement> | React.MouseEvent<HTMLButtonElement>) => void
    }) {
    return (
        <div className="space-y-7">
            {MATERIALS.map(material =>
            (
                <div id={material} className="flex justify-evenly">
                    <div className="form-floating">
                        <input
                            type="number"
                            name={material}
                            className="form-control-1 w-[112px] h-[45px] text-center"
                            autoComplete="off"
                            value={inputs.materialQuantities[material]}
                            onKeyDown={(e) => {
                                // Block '-', '+', and 'e' / 'E'
                                if (["-", "+", "e", "E"].includes(e.key)) {
                                    e.preventDefault();
                                }
                            }}
                            onChange={e => {
                                const val = Number.parseInt(e.target.value, 10);
                                if (val > 0 || e.target.value == "") {
                                    handleChange(e);
                                }
                            }}
                            min={0} />
                        <label className="form-label bg-default -translate-y-8 text-black text-base rounded-md p-1">{material}</label>
                    </div>
                    <button
                        value={1}
                        onClick={e => handleChange(e)}
                        name={material}
                        type="button"
                        className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">+</button>
                    <button
                        value={-1}
                        onClick={e => handleChange(e)}
                        name={material}
                        type="button"
                        className="quantity-btn mx-[20px] w-[40px] h-[40px] rounded-full">-</button>
                </div>
            )
            )}
        </div>
    );
}