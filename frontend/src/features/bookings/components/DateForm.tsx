import type { DateInputs, ScheduleFrequency } from "../types/types";
import WeeklyCalendar from "./WeeklyCalendar";
export default function DateForm({ inputs, setInput }:
    { inputs: DateInputs, setInput: (input: DateInputs) => void }) {
    const radioLabels: ScheduleFrequency[] = ["weekly", "bi-weekly", "tri-weekly", "monthly"];
    const handleChange = (frequency: ScheduleFrequency) => {
        setInput({ ...inputs, frequency });
    }
    return (
        <div className="space-y-6">
            <h1 className="text-center text-3xl font-[Lato]">
                Date
            </h1>
            <p className="text-center text-lg">When do you want us to collect your recycling?</p>
            <WeeklyCalendar inputs={inputs} setInput={setInput} />
            <div className="grid grid-cols-2 grid-rows-2 mx-auto gap-y-2">
                {radioLabels.map(label =>
                (
                    <div key={label} className="space-x-2 mx-auto w-[100px]">
                        <input type="radio" name="frequency" onClick={() => handleChange(label)} />
                        <label>{label}</label>
                    </div>
                )
                )}
            </div>
        </div>
    );
}
