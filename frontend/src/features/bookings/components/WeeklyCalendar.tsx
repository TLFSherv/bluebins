import { type CardData, type DateInputs } from "../types/types";

export default function WeeklyCalendar({ inputs, setInput }:
    { inputs: DateInputs, setInput: (input: DateInputs) => void }) {
    const start = new Date();
    // automatically adjusts for edge cases like shorter months
    const newDate = new Date()
    newDate.setMonth(start.getMonth() + 1);
    const end = newDate;

    let diffInMs = Math.abs(end.getTime() - start.getTime());
    let numOfDays = diffInMs / (1000 * 60 * 60 * 24);

    const dayCards: CardData[] = [];
    const WEEKDAYS: string[] = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    const MONTHS: string[] = ["Jan", "Feb", "Mar", "Apr", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"];

    const frequencyGaps = { "weekly": 7, "bi-weekly": 14, "tri-weekly": 21, "monthly": 30 };
    let cardNum = -1;
    let isActive = false;
    for (let i = 0; i < numOfDays; i++) {
        start.setDate(start.getDate() + 1);
        // When a frequency radio button is selected  
        // make the chosen date and the next set of dates active
        if (start.getDate() == inputs.dayNumber) cardNum = i;
        isActive = (cardNum >= 0 && (i - cardNum) % frequencyGaps[inputs.frequency] == 0);

        dayCards.push({
            weekDay: WEEKDAYS[start.getDay()],
            dayNum: start.getDate(),
            month: MONTHS[start.getMonth() - 1],
            active: isActive
        });
    }
    const handleChange = (card: CardData) => {
        setInput({
            type: "date",
            dayOfWeek: card.weekDay,
            dayNumber: card.dayNum,
            month: card.month,
            frequency: inputs.frequency
        });
    }
    return (
        <div className="w-full" >
            <div className="border-2 border-[#4AA5F6] rounded-xl bg-[#D9D9D9] p-1 flex gap-x-2 overflow-x-scroll">
                {dayCards.map((card, key) =>
                    <div key={key} onClick={() => handleChange(card)}
                        className={`rounded-lg ${card.active ? "bg-[#0088FF] text-white" : "bg-[#F3F6FB]"}`}>
                        <ol className="text-center w-[90px] h-[90px] flex flex-col justify-evenly">
                            <li className="text-xs">{card.weekDay}</li>
                            <li>{card.dayNum}</li>
                            <li>{card.month}</li>
                        </ol>
                    </div>
                )}
            </div>
        </div>
    )
}
