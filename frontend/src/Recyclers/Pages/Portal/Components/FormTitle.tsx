export default function FormTitle({ children }:
    { children: React.ReactNode; }) {
    return (
        <div className="rounded-xl p-1 bg-linear-[#69B9FF,#A83CFA,#4554FD]">
            <h1 className="text-center text-xl w-full rounded-lg p-1 bg-default">
                {children}
            </h1>
        </div>
    );
}