import ModelsGrid from "@/app/components/ModelsGrid.jsx";
// import getAllModels from "@/app/library/models.js";
import { getAllModels } from "@/lib/data/models"; // NEW

export default async function ModelsPage() {
  // getAllModels() returns an object like { items: [...] }
  const modelsResponse = await getAllModels();

  // Extract the 'items' array from the response object
  const models = modelsResponse.items || [];

  return <ModelsGrid title="All Models" models={models} />;
}
