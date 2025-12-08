import Link from "next/link";
import { getAllCategories } from "@/lib/data/categories";

export default async function CategoryBar() {
  const categories = await getAllCategories(); // string[]
  if (!categories?.length) return null;

  return (
    <nav className="flex md:flex-col mx-auto max-w-3/5 md:overflow-visible overflow-x-auto gap-3 whitespace-nowrap md:whitespace-normal">
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