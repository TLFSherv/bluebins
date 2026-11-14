import type { Booking, FREQUENCY, Schedule } from "../types/types";
import WeeklyCalendar from "./WeeklyCalendar";

export default function DateForm({ input, setInput }:
    {
        input: Booking,
        setInput: (input: Booking | Schedule) => void
    }) {
    const radioLabels: typeof FREQUENCY[number][] = ["Once", "Weekly", "Biweekly", "Monthly"];
    const radioMap = {
        Once: "once",
        Weekly: "weekly",
        Biweekly: "biweekly",
        Monthly: "monthly"
    };
    const handleChange = (e: React.ChangeEvent<HTMLSelectElement, HTMLSelectElement>) => {
        var value = e.target.value as typeof FREQUENCY[number];
        if (value == "Once") {
            setInput({ ...input, schedule: null });
            return;
        }
        setInput({ frequency: value, makeDefault: false, type: "schedule" });
    }
    return (
        <div className="space-y-6">
            <h1 className="text-center text-3xl font-[Lato]">
                Date
            </h1>
            <div className="space-y-1">
                <p className="text-lg text-center">When do you want us to collect?</p>
                <select
                    value={input.schedule?.frequency || radioMap.Once}
                    name="frequency"
                    className="border border-[#4AA5F6] rounded-lg block mx-auto text-center"
                    onChange={e => handleChange(e)}>
                    {radioLabels.map(label => (
                        <option key={label} value={label}>
                            {radioMap[label]}
                        </option>
                    ))}
                </select>
            </div>
            <div className="space-y-2">
                <WeeklyCalendar input={input} setInput={setInput} />
            </div>
        </div>
    );
}
