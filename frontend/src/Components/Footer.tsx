export default function Footer() {
    const year = new Date().getFullYear().toString();
    return (
        <div>
            <h1 className="text-center text-sm py-1">
                {`@Copyright ${year}`}
            </h1>
        </div>
    )
}