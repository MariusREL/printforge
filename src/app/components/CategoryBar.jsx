export default function CategoryBar({ slugs = [] }){
    const isArr = Array.isArray(slugs)
    if (!isArr){
        return null
    }
    return (
        <div>
    {slugs.map(slug => {
       return (
            <p key={slug}>{slug}</p>
       )
    })}
        </div>
        )
}