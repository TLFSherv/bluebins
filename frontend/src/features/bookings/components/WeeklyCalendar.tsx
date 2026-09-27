import { type CardData, type Date } from "../types/types";
import { WEEKDAYS, MONTHS } from "../types/types";

export default function WeeklyCalendar({ inputs, setInput }:
    { inputs: Date, setInput: (input: Date) => void }) {
    const start = new Date();
    // automatically adjusts for edge cases like shorter months
    const newDate = new Date()
    newDate.setMonth(start.getMonth() + 1);
    const end = newDate;

    let diffInMs = Math.abs(end.getTime() - start.getTime());
    let numOfDays = diffInMs / (1000 * 60 * 60 * 24);

    const dayCards: CardData[] = [];

    const frequencyGaps = { "weekly": 7, "bi-weekly": 14, "tri-weekly": 21, "monthly": 30 };
    let cardNum = -1;
    let isActive = false;
    for (let i = 0; i < numOfDays; i++) {
        start.setDate(start.getDate() + 1);
        // When a frequency radio button is selected  
        // make the chosen date and the next set of dates active
        if (start.getDate() == inputs.date[1]) cardNum = i;
        isActive = (cardNum >= 0 && (i - cardNum) % frequencyGaps[inputs.frequency] == 0);

        dayCards.push({
            date: [WEEKDAYS[start.getDay()], start.getDate(), MONTHS[start.getMonth() - 1]],
            active: isActive
        });
    }
    const handleChange = (card: CardData) => {
        setInput({
            type: "date",
            date: card.date,
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
                            <li className="text-xs">{card.date[0]}</li>
                            <li>{card.date[1]}</li>
                            <li>{card.date[2]}</li>
                        </ol>
                    </div>
                )}
            </div>
        </div>
    )
}
