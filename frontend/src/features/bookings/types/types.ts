const ScheduleFrequency = ["weekly", "bi-weekly", "tri-weekly", "monthly"] as const;
export type ScheduleFrequency = typeof ScheduleFrequency[number];


export const RecyclableMaterials = ["tin", "aluminium", "glass"] as const;
export type MaterialTypes = typeof RecyclableMaterials[number];

export type CardData = { weekDay: string, dayNum: number, month: string, active: boolean };

type Input = { type: string };

export type BookingInputs = {
    addressInputs: AddressInputs,
    dateInputs: DateInputs,
    quantityInputs: QuantityInputs
};

export type AddressInputs = Input & {
    type: "address",
    address: string,
    postcode: string,
    parish: string,
    additionalInfo: string,
    latitude: number,
    longitude: number,
    makeDefault: number
};

export type DateInputs = Input & {
    type: "date",
    dayOfWeek: String,
    dayNumber: number,
    month: string,
    frequency: ScheduleFrequency
};


export type QuantityInputs = Input & {
    type: "quantity",
    quantity: number,
    materialQuantity: Record<MaterialTypes, number>
};

