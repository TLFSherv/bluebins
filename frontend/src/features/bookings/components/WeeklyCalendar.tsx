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

    const frequencyGaps = { "Once": numOfDays + 1, "Weekly": 7, "Biweekly": 14, "Triweekly": 21, "Monthly": numOfDays };
    let cardNum = -1;
    let isActive = false;
    for (let i = 0; i < numOfDays - 1; i++) {
        start.setDate(start.getDate() + 1);
        // When a frequency radio button is selected  
        // make the chosen date and the next set of dates active
        if (inputs.date instanceof Date &&
            start.getDate() == inputs.date.getDate())
            cardNum = i;
        isActive = (cardNum >= 0 && (i - cardNum) % frequencyGaps[inputs.frequency] == 0);
        dayCards.push({
            date: new Date(start),
            active: isActive
        });
    }
    const handleChange = (card: CardData) => {
        setInput({
            type: "schedule",
            date: card.date,
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
                            <li className="text-xs">{WEEKDAYS[card.date.getDay()]}</li>
                            <li>{card.date.getDate()}</li>
                            <li>{MONTHS[card.date.getMonth()]}</li>
                        </ol>
                    </div>
                )}
            </div>
            <div className="flex justify-end space-x-2 text-sm">
                <input type="checkbox" name="makeDefault" checked={inputs.makeDefault}
                    // make default checkbox disabled if frequency is Once
                    disabled={inputs.frequency == "Once"}
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
