import { type IBookingService, type Booking, type BookingResponse, BookingSchema, type ValidationResponse } from "../types/types";
import { z } from "zod"

export const BookingService: IBookingService = {
    backendUrl: `${import.meta.env.VITE_SERVER_URL}/booking`,
    validate(booking: Booking): ValidationResponse {
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
                        quantity: zodError.fieldErrors.quantity
                    }
                }
            }
        }
        return { isValid: true };
    },
    async create(booking: Booking): Promise<BookingResponse<string>> {
        try {

            const { isValid, errors } = this.validate(booking);
            if (!isValid) {
                return errors as BookingResponse<string>;
            }
            const json = JSON.stringify(booking);
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
                        error: data
                    };
                }
                throw new Error(data.detail || "Failed to create booking");
            }
            return {
                success: true,
                message: "Booking successfully created",
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
    async get<T>(): Promise<BookingResponse<T>> {
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
    },
    async update(booking: Booking): Promise<BookingResponse<string>> {
        try {
            const { isValid, errors } = this.validate(booking);
            if (!isValid) {
                return errors as BookingResponse<string>;
            }

            const response = await fetch(`${this.backendUrl}/`,
                {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(booking),
                    credentials: "include"
                }
            );
            const data = await response.json();

            if (!response.ok) {
                if (response.status == 400)
                    return {
                        success: false,
                        message: "Booking data is incorrect",
                        error: data
                    }
                throw new Error(data.detail || "Failed to update booking");
            }

            return {
                success: true,
                message: "Booking successfully updated",
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
                error: null
            };
        }
    }
}