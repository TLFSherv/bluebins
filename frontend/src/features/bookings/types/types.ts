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
    type: z.literal("date").readonly(),
    date: z.tuple([z.literal(WEEKDAYS), z.number().min(1).max(31), z.literal(MONTHS)]),
    frequency: z.literal(FREQUENCY)
});

export const MATERIALS = ["tin", "aluminium", "glass"] as const;

export const QuantitySchema = z.object({
    type: z.literal("quantity").readonly(),
    quantity: z.number().gte(1),
    materialQuantity: z.record(z.literal(MATERIALS), z.number().gte(0))
})

export const BookingSchema = z.object({
    location: LocationSchema,
    schedule: ScheduleSchema,
    quantity: QuantitySchema
})
export type CardData = { date: [typeof WEEKDAYS[number], number, typeof MONTHS[number]], active: boolean };

export type Booking = z.infer<typeof BookingSchema>;
export type Address = z.infer<typeof LocationSchema>;
export type Date = z.infer<typeof ScheduleSchema>;
export type Quantity = z.infer<typeof QuantitySchema>;

export type BookingResponse = {
    success: boolean,
    message: string,
    error: {
        address: string[] | undefined,
        date: string[] | undefined,
        quantity: string[] | undefined
    } | null
}

export interface IBookingService {
    backendUrl: string
    createBooking(booking: Booking): Promise<BookingResponse>
};

