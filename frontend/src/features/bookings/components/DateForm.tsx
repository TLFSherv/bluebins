import WeeklyCalendar from "./WeeklyCalendar";
export default function DateForm() {
    const radioLabels = ["weekly", "bi-weekly", "tri-weekly", "monthly"];
    return (
        <div className="space-y-7">
            <h1 className="text-center text-2xl font-[Lato]">
                Date
            </h1>
            <p className="text-center">When do you want us to collect your recycling?</p>
            <WeeklyCalendar />
            <div className="grid grid-cols-2 grid-rows-2 mx-auto gap-y-2">
                {radioLabels.map(labels =>
                (
                    <div className="space-x-2 mx-auto w-[100px]">
                        <input type="checkbox" name={labels} />
                        <label>{labels}</label>
                    </div>
                )
                )}
            </div>
        </div>
    );
}
