import AddressForm from "./Components/AddressForm"
import DateForm from "./Components/DateForm"
import QuantityForm from "./Components/QuantityForm"

export default function Collection() {
    return (
        <form className="p-2 space-y-4">
            <AddressForm />
            <QuantityForm />
            <DateForm />
        </form>
    );
}