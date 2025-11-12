import modelsData from "@/app/data/models.json";

export default async function getAllModels() {
  return modelsData;
}

export async function getModelById(id) {
  const foundModel = modelsData.find(
    (model) => model.id.toString() === id.toString()
  );
  return foundModel;
}
