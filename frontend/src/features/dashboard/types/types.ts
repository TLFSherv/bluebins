
export type BookingDates = {
    bookingId: number,
    bookingDate: string
}

export type BookingResponse<T> = {
    success: boolean,
    message: string,
    result?: T,
    error?: {
        address: string[] | undefined,
        date: string[] | undefined,
        recycling: string[] | undefined
    } | null
}

export interface IDashboardService {
    backendUrl: string,
    getBookingDates(): Promise<BookingResponse<BookingDates[]>>
}