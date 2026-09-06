import HamburgerMenu from "./HamburgerMenu"

export default function Header() {
    return (
        <div className="bg-[#69B9FF] h-[30px] mb-16">
            <div className="relative w-fit">
                <a href="/">
                    <img className="size-22" src="/logo.svg" alt="logo" />
                </a>
            </div>
            <HamburgerMenu />
        </div>
    )
}