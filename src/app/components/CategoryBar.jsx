import Link from "next/link";

export default function CategoryBar({ slugs = [] }) {
  const isArr = Array.isArray(slugs);
  if (!isArr) {
    return null;
  }

  return (
    <nav className="flex flex-col gap-3">
      {/* "ALL" link */}
      <Link href="/3d-models" className="uppercase text-sm font-semibold hover:opacity-70 transition">
        All
      </Link>

      {/* Category links */}
      {slugs.map(slug => (
        <Link
          key={slug}
          href={`/3d-models/categories/${slug}`}
          className="uppercase text-sm hover:opacity-70 transition"
        >
          {slug}
        </Link>
      ))}
    </nav>
  );
}