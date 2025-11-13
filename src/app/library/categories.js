import categoriesStatic from "@/app/data/categories.json";

const BACKEND =
  process.env.BACKEND_URL ??
  process.env.NEXT_PUBLIC_API_URL ??
  "http://localhost:5000";

export async function fetchCategoriesFromApi() {
  const url = `${BACKEND.replace(/\/$/, "")}/3dmodels/categories`;
  try {
    const res = await fetch(url, { cache: "no-store" });
    if (!res.ok) {
      console.error("Categories API status:", res.status);
      return [];
    }
    const data = await res.json();
    if (!Array.isArray(data)) return [];
    return data.filter((c) => typeof c === "string" && c.trim().length > 0);
  } catch (err) {
    console.error("Categories API error:", err);
    return [];
  }
}

export async function getAllCategories() {
  const apiCategories = await fetchCategoriesFromApi();
  if (apiCategories.length > 0) {
    return apiCategories;
  }
  // Fallback: derive names from local meta
  return categoriesStatic
    .map((c) => c.name || c.slug)
    .filter((v) => typeof v === "string" && v.trim().length > 0);
}
// Local meta lookup (still needed for slug/displayName until API supplies them)
export async function getCategoryMetaBySlug(slug) {
  if (!slug) return null;
  return categoriesStatic.find((c) => c.slug === slug) || null;
}

export async function getCategoryMetaByName(name) {
  if (!name) return null;
  return (
    categoriesStatic.find(
      (c) =>
        c.name?.toLowerCase() === name.toLowerCase() ||
        c.slug?.toLowerCase() === name.toLowerCase()
    ) || null
  );
}

// Async because it may need future API enrichment
export async function getDisplayNameFromSlug(slug) {
  const meta = getCategoryMetaBySlug(slug);
  return meta?.displayName || meta?.name || slug;
}
