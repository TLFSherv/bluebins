import { type IBookingService, type Booking, type BookingResponse, BookingSchema, type ValidationResponse } from "../types/types";
import { z } from "zod"

export const BookingService: IBookingService = {
    backendUrl: `${import.meta.env.VITE_SERVER_URL}/booking`,
    validateBooking(booking: Booking): ValidationResponse {
        // Validate booking input
        const validationResult = BookingSchema.safeParse(booking);
        if (!validationResult.success) {
            const zodError = z.flattenError(validationResult.error);
            return {
                isValid: false,
                errors: {
                    success: false,
                    message: "validation error",
                    error: {
                        address: zodError.fieldErrors.location,
                        date: zodError.fieldErrors.schedule,
                        recycling: zodError.fieldErrors.recycling
                    }
                }
            }
        }
        return { isValid: true };
    },
    async createUpdateBooking(booking: Booking): Promise<BookingResponse<string>> {
        try {
            const { isValid, errors } = this.validateBooking(booking);
            if (!isValid) {
                return errors as BookingResponse<string>;
            }

            const json = JSON.stringify(this.parseBooking(booking));
            console.log(json);

            // Make API request
            const response = await fetch(`${this.backendUrl}/`,
                {
                    method: "Post",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: json,
                    credentials: "include"
                }
            );

            const data = await response.json();

            // Verify the response
            if (!response.ok) {
                // Let users know if it's a user error
                if (response.status == 400) {
                    return {
                        success: false,
                        message: data.title,
                        error: data.errors
                    };
                }
                throw new Error(data.detail || "Failed to create booking");
            }
            return {
                success: true,
                message: "Data saved successfully",
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
    async getBooking(): Promise<BookingResponse<Booking>> {
        try {
            const response = await fetch(`${this.backendUrl}/`,
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
                        message: "failed to get user booking",
                        error: data
                    }
                }
                throw new Error(data.detail || "Failed to get user booking");
            }

            return {
                success: true,
                message: "successfully returned user booking",
                result: this.parseBooking(data)
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
    },
    parseBooking(booking: Booking): Booking {
        // Create a new object payload so you don't mutate the original state
        const parsedBooking: Booking = {
            ...booking,
        }
        // change booking date from UTC string to date
        // or date to UTC string
        var date = booking.bookingDate;
        if (date instanceof Date) {
            var mm = (date.getMonth() + 1).toString();
            mm = mm.length < 2 ? "0" + mm : mm
            var dd = (date.getDate()).toString();
            dd = dd.length < 2 ? "0" + dd : dd;
            parsedBooking.bookingDate = `${date.getFullYear()}-${mm}-${dd}`;
            return parsedBooking;
        }

        const [year, month, day] = date.split('-').map(Number);
        // Month is 0-indexed in JS Date (0 = January)
        parsedBooking.bookingDate = new Date(year, month - 1, day);
        return parsedBooking;
    },
    hasBookingChanged(b1: Booking, b2: Booking): boolean {
        return JSON.stringify(b1) != JSON.stringify(b2);
    }
}