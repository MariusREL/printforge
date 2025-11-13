import ModelCard from "@/app/components/ModelCard";

export default function ModelsGrid({ title, models }) {
  // Ensure 'models' is treated as an array, even if it's null or undefined.
  const modelItems = Array.isArray(models) ? models : [];

  if (modelItems.length === 0) {
    return (
      <div className="mx-10">
        <h1 className="mb-8 text-3xl font-bold">{title}</h1>
        <p>No models found.</p>
      </div>
    );
  }

  return (
    <div className="mx-10">
      <h1 className="mb-8 text-3xl font-bold">{title}</h1>

      <div
        className="grid gap-6 grid-cols-[repeat(auto-fit,minmax(230px,1fr))]"
        role="region"
        aria-label="3D Models Gallery"
      >
        {modelItems.map((model) => (
          <ModelCard key={model.id} model={model} />
        ))}
      </div>
    </div>
  );
}