import { type IBookingService, type Booking, type BookingResponse, BookingSchema } from "../types/types";
import { z } from "zod"

export const BookingService: IBookingService = {
    backendUrl: import.meta.env.VITE_SERVER_URL,
    async createBooking(booking: Booking): Promise<BookingResponse> {
        try {
            // Validate booking input
            const validationResult = BookingSchema.safeParse(booking);
            if (!validationResult.success) {
                const zodError = z.flattenError(validationResult.error);
                return {
                    success: false,
                    message: "Validation error",
                    error: {
                        address: zodError.fieldErrors.address,
                        date: zodError.fieldErrors.date,
                        quantity: zodError.fieldErrors.quantity
                    }
                }
            }

            // Make API request
            const response = await fetch(`${this.backendUrl}/booking?useCookies=true`,
                {
                    method: "Post",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(booking),
                    credentials: "include"
                }
            );

            // Verify the response
            if (!response.ok) {
                const errorData = await response.json();
                // Let users know if it's a user error
                if (response.status == 400) {
                    return {
                        success: false,
                        message: errorData.title,
                        error: null
                    };
                }
                throw new Error(errorData.detail || "Failed to create booking");
            }
            return {
                success: true,
                message: "Booking successfully created",
                error: null
            };
        }
        catch (error: any) {
            if (error.message.includes("Server error")) {
                throw error;
            }
            return {
                success: false,
                message: error.message,
                error: null
            };
        }
    }
}