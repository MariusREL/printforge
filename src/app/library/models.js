import modelsData from "@/app/data/models.json";

const BACKEND =
  process.env.BACKEND_URL ??
  process.env.NEXT_PUBLIC_API_URL ??
  "http://localhost:5000";

export default async function getAllModels() {
  try {
    const url = `${BACKEND.replace(/\/$/, "")}/3dmodels`;
    const res = await fetch(url, { cache: "no-store" });

    if (!res.ok) {
      console.error(`API responded with status ${res.status}`);
      // return modelsData;
    }

    // Guard against empty response body which causes JSON.parse errors
    const text = await res.text();
    // if (!text) return modelsData;

    return JSON.parse(text);
  } catch (error) {
    console.error("Error fetching models:", error);
    // return modelsData;
  }
}

export async function getModelById(id) {
  const models = await getAllModels();
  const foundModel = models.find(
    (model) => model.id.toString() === id.toString()
  );
  return foundModel;
}
