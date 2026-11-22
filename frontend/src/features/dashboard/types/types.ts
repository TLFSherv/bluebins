import type { Booking } from "../../bookings/types/types"

export type BookingDates = {
    bookingId: number,
    bookingDate: string
}

export type DashboardResponse<T> = {
    success: boolean,
    message: string,
    result?: T,
}

export interface IDashboardService {
    backendUrl: string,
    getBookingDates(): Promise<DashboardResponse<BookingDates[]>>,
    getBooking(bookingId: number): Promise<DashboardResponse<Booking>>
}