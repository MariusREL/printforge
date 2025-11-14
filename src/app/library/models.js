import modelsData from "@/app/data/models.json";

const BACKEND =
  process.env.BACKEND_URL ??
  process.env.NEXT_PUBLIC_API_URL ??
  "http://localhost:5000";

// The function now accepts an optional 'category'
export default async function getAllModels({ category } = {}) {
  try {
    // Build the URL. If a category is provided, add it as a query parameter.
    const baseUrl = `${BACKEND.replace(/\/$/, "")}/3dmodels`;
    const url = category
      ? `${baseUrl}?category=${encodeURIComponent(category)}`
      : baseUrl;

    const res = await fetch(url, { cache: "no-store" });

    if (!res.ok) {
      console.error(`API Error: ${res.status}`);
      // Fallback to local data if API fails
      const models = category
        ? modelsData.filter(
            (m) => m.category.toLowerCase() === category.toLowerCase()
          )
        : modelsData;
      return { items: models };
    }
    const data = await res.json();
    console.log(data);
    return data; // API returns { totalCount, items }
  } catch (error) {
    console.error("Fetch Error:", error);
    // Fallback on network error
    const models = category
      ? modelsData.filter(
          (m) => m.category.toLowerCase() === category.toLowerCase()
        )
      : modelsData;
    return { items: models };
  }
}

export async function getModelById(id) {
  try {
    // Fetch a single model directly from the API
    const url = `${BACKEND.replace(/\/$/, "")}/3dmodels/${id}`;
    const res = await fetch(url, { cache: "no-store" });

    if (!res.ok) {
      // If API fails (e.g., 404 Not Found), try the local fallback
      console.error(`API Error for model ${id}: ${res.status}`);
      return modelsData.find((model) => model.id.toString() === id.toString());
    }

    return res.json();
  } catch (error) {
    // On network error, try the local fallback
    console.error(`Fetch Error for model ${id}:`, error);
    return modelsData.find((model) => model.id.toString() === id.toString());
  }
}
