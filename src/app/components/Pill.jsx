export default function Pill( { children, className = ""} ){
    return (
        <span className="inline-block bg-transparent border border-gray-200 px-3 py-1 rounded-2xl text-sm text-gray-800">
            {children}
        </span>
    )
}