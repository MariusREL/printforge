import { getDisplayNameFromSlug } from "@/app/library/categories.js";
import ModelsGrid from "@/app/components/ModelsGrid.jsx";
import getAllModels from "@/app/library/models.js";

export default async function Categories({ params }) {
  const { categoryName } = await params;

  // Ask the API for the display name and the pre-filtered models
  const displayName = await getDisplayNameFromSlug(categoryName);
  const modelsResponse = await getAllModels({ category: categoryName });

  // The API and fallback return an object like { items: [...] }
  const models = modelsResponse.items || [];
  console.log(models);
  // No more .filter() needed! The backend did the work.
  return <ModelsGrid title={displayName} models={models} />;
}


