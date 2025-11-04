import Link from "next/link"
export default function BackToOverview({className = ""}){
    return (
        <header className={`block ${className}`}>
            <Link href="/3d-models">
                <div className="container p-4 flex justify-center">
                    <span className="uppercase">back to overview</span>
                </div>
            </Link>
        </header>
    )
}