import Link from "next/link";
import { getAllCategories } from "@/app/library/categories";

export default async function CategoryBar() {
  const categories = await getAllCategories(); // string[]
  if (!categories?.length) return null;

  return (
    <nav className="flex md:flex-col md:overflow-visible overflow-scroll gap-3">
      <Link href="/3dmodels" className="uppercase text-sm font-semibold hover:opacity-70 transition">
        All
      </Link>
      {categories.map((name) => {
        const slug = encodeURIComponent(name.toLowerCase());
        return (
          <Link
            key={slug}
            href={`/3dmodels/categories/${slug}`}
            className="uppercase text-sm hover:opacity-70 transition"
          >
            {name}
          </Link>
        );
      })}
    </nav>
  );
}