export default function FormTitle({ children }:
    { children: React.ReactNode; }) {
    return (
        <div className="rounded-xl p-1 bg-linear-[#69B9FF,#A83CFA,#4554FD] w-1/2">
            <h1 className="text-center tracking-wide text-lg w-full rounded-lg p-1 bg-default font-[Lato]">
                {children}
            </h1>
        </div>
    );
}