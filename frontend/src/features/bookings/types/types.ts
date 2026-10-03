import { z } from 'zod'

export const LocationSchema = z.object({
    type: z.literal("location").readonly(),
    address: z.string("Not a string"),
    mapsId: z.string().optional(),
    postcode: z.string().length(4),
    parish: z.literal(["st georges", "hamilton parish", "smiths", "st davids", "pempbroke", "devonshire", "sandys", "warwick", "somerset"]),
    details: z.string().optional(),
    latitude: z.number(),
    longitude: z.number(),
    makeDefault: z.literal([0, 1])
})

export const WEEKDAYS = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"] as const;
export const MONTHS = ["Jan", "Feb", "Mar", "Apr", "June", "July", "Aug", "Sept", "Oct", "Nov", "Dec"] as const;
export const FREQUENCY = ["weekly", "bi-weekly", "tri-weekly", "monthly"] as const;

export const ScheduleSchema = z.object({
    type: z.literal("schedule").readonly(),
    date: z.tuple([z.literal(WEEKDAYS), z.number().min(1).max(31), z.literal(MONTHS)]),
    frequency: z.literal(FREQUENCY),
    makeDefault: z.literal([0, 1])
});

export const MATERIALS = ["tin", "aluminium", "glass"] as const;

export const QuantitySchema = z.object({
    type: z.literal("quantity").readonly(),
    numberOfBags: z.number().gte(1),
    materialQuantities: z.record(z.enum(MATERIALS), z.number().gte(0))
})

export const BookingSchema = z.object({
    location: LocationSchema,
    schedule: ScheduleSchema,
    quantity: QuantitySchema
})
export type CardData = { date: [typeof WEEKDAYS[number], number, typeof MONTHS[number]], active: boolean };

export type Booking = z.infer<typeof BookingSchema>;
export type Location = z.infer<typeof LocationSchema>;
export type Schedule = z.infer<typeof ScheduleSchema>;
export type Quantity = z.infer<typeof QuantitySchema>;

export type BookingResponse<T> = {
    success: boolean,
    message: string,
    result?: T,
    error?: {
        address: string[] | undefined,
        date: string[] | undefined,
        quantity: string[] | undefined
    } | null
}

export type ValidationResponse = {
    isValid: boolean,
    errors?: BookingResponse<string>
}

export interface IBookingService {
    backendUrl: string
    validate(booking: Booking): ValidationResponse
    get<T>(): Promise<BookingResponse<T>>
    create(booking: Booking): Promise<BookingResponse<string>>
    update(booking: Booking): Promise<BookingResponse<string>>
};

