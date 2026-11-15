import { useEffect, useState } from "react"
import { NavMenu } from "./components/NavMenu"
export default function Dashboard() {
    return (
        <div>
            <h1>User Dashboard</h1>
            <NavMenu />
        </div>
    )
}