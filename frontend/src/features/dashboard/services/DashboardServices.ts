import type { Booking } from "../../bookings/types/types";
import { type IDashboardService, type BookingDates, type DashboardResponse } from "../types/types"

export const DashboardService: IDashboardService = {
    backendUrl: `${import.meta.env.VITE_SERVER_URL}`,
    async getBookingDates(): Promise<DashboardResponse<BookingDates[]>> {
        try {
            const response = await fetch(`${this.backendUrl}/dashboard/bookingDates`,
                {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    credentials: "include"
                }
            );
            const data = await response.json();
            if (!response.ok) {
                if (response.status == 400) {
                    return {
                        success: false,
                        message: "failed to get booking dates",
                    };
                }
                throw new Error(data.detail || "Failed to get booking dates");
            }

            return {
                success: true,
                message: "successfully restured booking dates",
                result: data
            };
        }
        catch (error: any) {
            if (error.message.includes("Server error")) {
                throw error;
            }
            return {
                success: false,
                message: error.message,
            };
        }
    },
    async getBooking(bookingId: number): Promise<DashboardResponse<Booking>> {
        try {
            const response = await fetch(`${this.backendUrl}/booking/${bookingId}`,
                {
                    method: "GET",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    credentials: "include"
                }
            );
            const data = await response.json();
            if (!response.ok) {
                if (response.status == 400) {
                    return {
                        success: false,
                        message: `failed to get booking ${bookingId}`,
                    };
                }
                throw new Error(data.detail || `failed to get booking ${bookingId}`);
            }
            return {
                success: true,
                message: `successfully returned booking ${bookingId}`,
                result: data
            };
        }
        catch (error: any) {
            if (error.message.includes("Server error")) {
                throw error;
            }
            return {
                success: false,
                message: error.message,
            };
        }

    }
}