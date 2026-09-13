export type ScheduleFrequency = "weekly" | "bi-weekly" | "tri-weekly" | "monthly";

export const RecyclableMaterials = ["tin", "aluminium", "glass"] as const;
type MaterialTypes = typeof RecyclableMaterials[number];

export type CardData = { weekDay: string, dayNum: string, month: string, active: boolean };

export type BookingInputs = {
    addressInputs: AddressInputs,
    dateInputs: DateInputs,
    quantityInputs: QuantityInputs
};

export type AddressInputs = {
    address: string,
    postcode: string,
    parish: string,
    additionalInfo: string,
    latitude: number,
    longitude: number
};

export type DateInputs = {
    dayOfWeek: String,
    dayNumber: number,
    month: string,
    frequency: ScheduleFrequency
};


export type QuantityInputs = {
    quantity: number,
    materialQuantity: Record<MaterialTypes, number>
};

