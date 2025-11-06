import { getCategoryBySlug, getDisplayNameFromSlug, getAllCategories } from "@/app/library/categories.js"
import CategoryBar from "@/app/components/CategoryBar";
import ModelsGrid from "@/app/components/ModelsGrid";
import getAllModels from "@/app/library/models";


export default async function Categories({ params }) {
    const { categoryName } = await params
    const displayname = getDisplayNameFromSlug(categoryName);
    const categorySlug = getCategoryBySlug(categoryName);
    const allModels = await getAllModels()
    console.log(allModels);

    const modelsFiltered = allModels.map(model => model).filter(c => categorySlug.slug === c.category)
    console.log(modelsFiltered);

    return (
        <>
        <CategoryBar slug = {categorySlug.slug} />
        <ModelsGrid title={displayname} models={modelsFiltered}/>
        </>
    )
}