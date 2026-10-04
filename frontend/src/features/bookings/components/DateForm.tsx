import type { Schedule, FREQUENCY } from "../types/types";
import WeeklyCalendar from "./WeeklyCalendar";

export default function DateForm({ inputs, setInput }:
    { inputs: Schedule, setInput: (input: Schedule) => void }) {
    const radioLabels: typeof FREQUENCY[number][] = ["Once", "Weekly", "Biweekly", "Triweekly", "Monthly"];
    const radioMap = {
        Once: "once",
        Weekly: "weekly",
        Biweekly: "bi-weekly",
        Triweekly: "tri-weekly",
        Monthly: "monthly"
    };
    const handleChange = (e: React.ChangeEvent<HTMLSelectElement, HTMLSelectElement>) => {
        var value = e.target.value as typeof FREQUENCY[number];
        setInput({ ...inputs, frequency: value });
    }
    return (
        <div className="space-y-6">
            <h1 className="text-center text-3xl font-[Lato]">
                Date
            </h1>
            <div className="space-y-1">
                <p className="text-lg text-center">When do you want us to collect your recycling?</p>
                <select name="frequency"
                    className="border border-[#4AA5F6] rounded-lg block mx-auto text-center"
                    onChange={e => handleChange(e)}>
                    {radioLabels.map(label => (
                        <option value={label}>{radioMap[label]}</option>
                    ))}
                </select>
            </div>
            <div className="space-y-2">
                <WeeklyCalendar inputs={inputs} setInput={setInput} />
            </div>
        </div>
    );
}
