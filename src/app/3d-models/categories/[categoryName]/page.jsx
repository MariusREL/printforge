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
        <div className="container grid grid-cols-1 md:grid-cols-12 gap-10">

            <aside className="md:col-span-2 md:p-10">
                <div className="md:sticky md:top-1/2 md:-translate-y-1/2"><CategoryBar slugs = {allCategories} /></div>
            </aside>
            
            <section className="md:col-span-10">
                <ModelsGrid 
                    title={displayname} 
                    models={modelsFiltered}
                />
            </section>
        </div>
    )
}