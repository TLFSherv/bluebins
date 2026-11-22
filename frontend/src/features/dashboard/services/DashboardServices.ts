import { type IDashboardService, type BookingDates, type BookingResponse } from "../types/types"

export const DashboardService: IDashboardService = {
    backendUrl: `${import.meta.env.VITE_SERVER_URL}/dashboard`,
    async getBookingDates(): Promise<BookingResponse<BookingDates[]>> {
        try {
            const response = await fetch(`${this.backendUrl}/bookingDates`,
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
                        error: data
                    }
                }
                throw new Error(data.detail || "Failed to get booking dates");
            }

            return {
                success: true,
                message: "successfully restured booking dates",
                result: data
            }
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