import { type CardData, type Schedule } from "../types/types";
import { WEEKDAYS, MONTHS } from "../types/types";

export default function WeeklyCalendar({ inputs, setInput }:
    { inputs: Schedule, setInput: (input: Schedule) => void }) {
    const start = new Date();
    // automatically adjusts for edge cases like shorter months
    const newDate = new Date()
    newDate.setMonth(start.getMonth() + 1);
    const end = newDate;

    let diffInMs = Math.abs(end.getTime() - start.getTime());
    let numOfDays = diffInMs / (1000 * 60 * 60 * 24);

    const dayCards: CardData[] = [];

    const frequencyGaps = { "Weekly": 7, "Biweekly": 14, "Triweekly": 21, "Monthly": 30 };
    let cardNum = -1;
    let isActive = false;
    for (let i = 0; i < numOfDays; i++) {
        start.setDate(start.getDate() + 1);
        // When a frequency radio button is selected  
        // make the chosen date and the next set of dates active
        if (start.getDate() == inputs.displayDate[1]) cardNum = i;
        isActive = (cardNum >= 0 && (i - cardNum) % frequencyGaps[inputs.frequency] == 0);

        dayCards.push({
            date: [WEEKDAYS[start.getDay()], start.getDate(), MONTHS[start.getMonth() - 1], start.getFullYear()],
            active: isActive
        });
    }
    const handleChange = (card: CardData) => {
        var mm = (MONTHS.indexOf(card.date[2]) + 1).toString();
        mm = mm.length < 2 ? "0" + mm : mm;
        var dd = card.date[1].toString();
        dd = dd.length < 2 ? "0" + dd : dd;
        setInput({
            type: "schedule",
            displayDate: card.date,
            date: `${card.date[3]}-${mm}-${dd}`,
            frequency: inputs.frequency,
            makeDefault: inputs.makeDefault,
        });
    }
    return (
        <div className="w-full space-y-4" >
            <div className="border-2 border-[#4AA5F6] rounded-xl bg-[#D9D9D9] p-1 flex gap-x-2 overflow-x-scroll">
                {dayCards.map((card, key) =>
                    <div key={key} onClick={() => handleChange(card)}
                        className={`rounded-lg ${card.active ? "bg-[#0088FF] text-white" : "bg-[#F3F6FB]"}`}>
                        <ol className="text-center w-[90px] h-[90px] flex flex-col justify-evenly">
                            <li className="text-xs">{card.date[0]}</li>
                            <li>{card.date[1]}</li>
                            <li>{card.date[2]}</li>
                        </ol>
                    </div>
                )}
            </div>
            <div className="flex justify-end space-x-2 text-sm">
                <input type="checkbox" name="makeDefault" checked={inputs.makeDefault}
                    onChange={() => setInput({
                        ...inputs,
                        makeDefault: !inputs.makeDefault
                    })} />
                <label>
                    Make my default
                </label>
            </div>
        </div>
    )
}
