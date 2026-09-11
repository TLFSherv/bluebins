import { type CardData } from "../types/types";

export default function WeeklyCalendar() {
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
    for (let i = 0; i < numOfDays; i++) {
        start.setDate(start.getDate() + 1);
        dayCards.push({
            weekDay: WEEKDAYS[start.getDay()],
            dayNum: start.getDate().toString(),
            month: MONTHS[start.getMonth() - 1],
            active: false
        });
    }
    return (
        <div className="w-full" >
            <div className="border-2 border-[#4AA5F6] rounded-xl bg-[#D9D9D9] p-1 flex gap-x-2 overflow-x-scroll">
                {dayCards.map(card => <DayCard {...card} />)}
            </div>
        </div>
    )
}

function DayCard(cardData: CardData) {
    return (
        <div className={`rounded-lg ${cardData.active ? "bg-[#0088FF] text-white" : "bg-[#F3F6FB]"}`}>
            <ol className="text-center w-[90px] h-[90px] flex flex-col justify-evenly">
                <li className="text-xs">{cardData.weekDay}</li>
                <li>{cardData.dayNum}</li>
                <li>{cardData.month}</li>
            </ol>
        </div>
    )
}