import { getCategoryBySlug, getDisplayNameFromSlug, getAllCategories } from "@/app/library/categories.js"
import CategoryBar from "@/app/components/CategoryBar.jsx";
import ModelsGrid from "@/app/components/ModelsGrid.jsx";
import getAllModels from "@/app/library/models.js";


export default async function Categories({ params }) {
    const { categoryName } = await params
    const displayname = getDisplayNameFromSlug(categoryName);
    const categorySlug = getCategoryBySlug(categoryName);
    const allModels = await getAllModels()
    const allCategories = getAllCategories().map(category => category.slug)

    const modelsFiltered = allModels.map(model => model).filter(c => categorySlug.slug === c.category)
    // console.log(modelsFiltered);

    return (
        <div className="container grid grid-cols-10 gap-10">
            <aside className="col-span-1"><CategoryBar slugs = {allCategories} /></aside>
            <section className="col-span-9">
                <ModelsGrid 
                    title={displayname}
                    models={modelsFiltered}
                />
            </section>
        </div>
    )
}