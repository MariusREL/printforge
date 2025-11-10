import { getCategoryBySlug, getDisplayNameFromSlug } from "@/app/library/categories.js";
import ModelsGrid from "@/app/components/ModelsGrid.jsx";
import getAllModels from "@/app/library/models.js";

export default async function Categories({ params }) {
  const { categoryName } = await params;
  const displayname = getDisplayNameFromSlug(categoryName);
  const categorySlug = getCategoryBySlug(categoryName);
  const allModels = await getAllModels();

  const modelsFiltered = allModels.filter(m => m.category === categorySlug.slug);

  return <ModelsGrid title={displayname} models={modelsFiltered} />;
}