import Link from "next/link";

export default function CategoryBar({ slugs = [] }) {
  const isArr = Array.isArray(slugs);
  if (!isArr) {
    return null;
  }

  return (
    <nav className="flex md:flex-col md:overflow-visible overflow-scroll gap-3">
      {/* "ALL" link */}
      <Link href="/3dmodels" className="uppercase text-sm font-semibold hover:opacity-70 transition">
        All
      </Link>

      {/* Category links */}
      {slugs.map(slug => (
        <Link
          key={slug}
          href={`/3dmodels/categories/${slug}`}
          className=" uppercase text-sm hover:opacity-70 transition"
        >
          {slug}
        </Link>
      ))}
    </nav>
  );
}