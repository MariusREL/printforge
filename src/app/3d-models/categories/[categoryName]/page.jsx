import { getCategoryBySlug, getDisplayNameFromSlug } from "@/app/library/categories.js"


export default async function Categories({ params }) {
    const { categoryName } = await params
    const displayname = getDisplayNameFromSlug(categoryName);
    const categorySlug = getCategoryBySlug(categoryName);
    return (
        <>
        <h1>{displayname}</h1>
        <h2>{categorySlug.slug}</h2>
        
        </>
    )
}