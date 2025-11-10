import CategoryBar from "@/app/components/CategoryBar";
import { getAllCategories } from "@/app/library/categories";

export default function ModelsPageWrapper({ children }) {
  const slugs = getAllCategories().map(c => c.slug);

  return (
    <div className="container mx-auto px-4 lg:px-8 py-8 grid gap-8 grid-cols-1 lg:grid-cols-12">
      {/* Sidebar on the left */}
      <aside className="lg:col-span-2">
        <div className="sticky top-1/2 -translate-y-1/2">
          <CategoryBar slugs={slugs} />
        </div>
      </aside>
      
      {/* Content area */}
      <section className="lg:col-span-10">
        {children}
      </section>
    </div>
  );
}