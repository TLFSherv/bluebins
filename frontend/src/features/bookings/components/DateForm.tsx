export default function DateForm() {
    return (
        <div className="space-y-7">
            <h1 className="text-center text-2xl font-[Lato]">
                Date
            </h1>
            <p className="text-center">When do you want us to collect your recycling?</p>
            <div className="mx-auto border-3 border-[#D9D9D9] bg-[#34C759] rounded-full w-[112px] h-[40px] flex items-center justify-end px-1">
                <h1 className="text-white absolute left-38 font-light text-sm tracking-wider">Today</h1>
                <div className="size-7 bg-[#D9D9D9] rounded-full border-1 border-white"></div>
            </div>
        </div>
    );
}