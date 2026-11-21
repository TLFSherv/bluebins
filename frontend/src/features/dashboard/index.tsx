import { NavMenu } from "./components/NavMenu"
import MyBookings from "./components/MyBookings"

export default function Dashboard() {
    return (
        <div className="space-y-8 mb-4">
            <NavMenu />
            <MyBookings />
        </div>
    )
}