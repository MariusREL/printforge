import categories from "@/app/data/categories.json";

export function getAllCategories() {
  return categories;
}

export function getCategoryBySlug(slug) {
  const category = categories.find((c) => c.slug === slug);
  if (!category) {
    throw new Error(`category with slug ${slug} not found`);
  }
  return category;
}
export function getDisplayNameFromSlug(slug) {
  const category = getCategoryBySlug(slug);
  return category.displayName;
}
