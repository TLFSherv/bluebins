import { z } from 'zod'

export const LocationSchema = z.object({
    type: z.literal("location").readonly(),
    address: z.string("Not a string").nonoptional("Please enter a address"),
    mapsId: z.string().optional().nullable(),
    postcode: z.string().length(4, "Please add a 4 character postcode."),
    parish: z.literal(["st georges", "hamilton parish", "smiths", "st davids", "pempbroke", "devonshire", "sandys", "warwick", "somerset"]),
    details: z.string().optional(),
    latitude: z.number(),
    longitude: z.number(),
    makeDefault: z.boolean()
})

export const WEEKDAYS = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"] as const;
export const MONTHS = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"] as const;
export const FREQUENCY = ["Once", "Weekly", "Biweekly", "Triweekly", "Monthly"] as const;

// Validates strictly "YYYY-MM-DD"
const dateOnlySchema = z.string().regex(/^\d{4}-\d{2}-\d{2}$/, {
    message: 'Please select a date.',
});

export const ScheduleSchema = z.object({
    type: z.literal("schedule").readonly(),
    date: z.union([z.date(), dateOnlySchema]),
    frequency: z.literal(FREQUENCY),
    makeDefault: z.boolean()
});

export const MATERIALS = ["tin", "aluminium", "glass"] as const;

export const RecyclingSchema = z.object({
    type: z.literal("recycling").readonly(),
    numberOfBags: z.number().gte(1),
    materialQuantities: z.record(z.enum(MATERIALS), z.number().gte(0))
})

export const BookingSchema = z.object({
    location: LocationSchema,
    schedule: ScheduleSchema,
    recycling: RecyclingSchema
})
export type CardData = { date: Date, active: boolean };

export type Booking = z.infer<typeof BookingSchema>;
export type Location = z.infer<typeof LocationSchema>;
export type Schedule = z.infer<typeof ScheduleSchema>;
export type Recycling = z.infer<typeof RecyclingSchema>;

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

export type ValidationResponse = {
    isValid: boolean,
    errors?: BookingResponse<string>
}

export interface IBookingService {
    backendUrl: string
    validateBooking(booking: Booking): ValidationResponse
    getBooking(): Promise<BookingResponse<Booking>>
    createUpdateBooking(booking: Booking): Promise<BookingResponse<string>>
    parseBooking(booking: Booking): Booking
    hasBookingChanged(b1: Booking, b2: Booking): boolean
};



