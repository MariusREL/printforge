import ModelCard from "@/app/components/ModelCard";

export default function ModelsGrid({ title, models }) {
  console.log('🔍 ModelsGrid received:', { 
    title, 
    models, 
    isArray: Array.isArray(models),
    length: models?.length,
    firstModel: models?.[0]
  });

  const modelItems = Array.isArray(models) ? models : [];

  if (modelItems.length === 0) {
    return (
      <div className="mx-5">
        <h1 className="mb-8 text-3xl font-bold">{title}</h1>
        <p>No models found.</p>
      </div>
    );
  }

  return (
    <div className="mx-5">
      <h1 className="mb-8 text-3xl font-bold text-center">{title}</h1>

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