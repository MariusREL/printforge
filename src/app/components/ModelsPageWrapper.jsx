import CategoryBar from "@/app/components/CategoryBar";
import { getAllCategories } from "@/app/library/categories";

export default async function ModelsPageWrapper({ children }) {
  const data = await getAllCategories()
  const slugs = data.map(c => c.category);

  return (
    <div className="container mx-auto px-4 py-8 grid gap-8 grid-cols-1 md:grid-cols-10">
      {/* Sidebar on the left */}
      <aside className="col-span-10 md:col-span-1">
        <div className="sticky top-10 md:mt-[50%]">
          <CategoryBar slugs={slugs} />
        </div>
      </aside>
      
      {/* Content area */}
      <section className="col-span-9 lg:col-span-9">
        {children}
      </section>
    </div>
  );
}