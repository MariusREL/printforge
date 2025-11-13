import Link from "next/link"
export default function BackToOverview({className = ""}){
  return (
    <header className={`bg-[--header-bg] text-[--header-fg]  ${className}`}>
      <Link href="/3dmodels">
        <div className="container p-2 flex justify-center">
          <span className="uppercase border-b border-[--header-border]">back to overview</span>
        </div>
      </Link>
    </header>
  )
}