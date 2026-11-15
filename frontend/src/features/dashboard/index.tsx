import { NavMenu } from "./components/NavMenu"
import { BookingTimeline } from "./components/BookingTimeline"

export default function Dashboard() {
    return (
        <div className="space-y-4">
            <NavMenu />
            <BookingTimeline />
        </div>
    )
}