import modelsData from "@/app/data/models.json";

export default async function getAllModels() {
  const res = await fetch("http://localhost:5000/3dmodels");
  return res.json();
}

export async function getModelById(id) {
  const foundModel = modelsData.find(
    (model) => model.id.toString() === id.toString()
  );
  return foundModel;
}
