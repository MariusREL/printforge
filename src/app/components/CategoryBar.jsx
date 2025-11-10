import Link from "next/link"

export default function CategoryBar({ slugs = [] }){
    const isArr = Array.isArray(slugs)
    if (!isArr){
        return null
    }
    return (
        
        <div className="flex md:flex-col gap-10 md:gap-5 h-full justify-center">

            <Link href="/3d-models"><h1 className="uppercase text-sm">all</h1></Link>

    {slugs.map(slug => {
       return (
            <div key={slug}><Link  href={`/3d-models/categories/${slug}`}><p className="uppercase text-sm">{slug}</p></Link></div>
       )
    })}
        </div>
        )
}