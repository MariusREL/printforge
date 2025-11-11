import CategoryBar from "@/app/components/CategoryBar";
import { getAllCategories } from "@/app/library/categories";

export default function ModelsPageWrapper({ children }) {
  const slugs = getAllCategories().map(c => c.slug);

  return (
    <div className="container mx-auto px-4 lg:px-8 py-8 grid gap-8 grid-cols-1 md:grid-cols-10">
      {/* Sidebar on the left */}
      <aside className="lg:col-span-1">
        <div className="sticky top-1/2  md:-translate-y-2/5 ">
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