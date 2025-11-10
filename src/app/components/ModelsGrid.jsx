'use client';
import ModelCard from "@/app/components/ModelCard";

export default function ModelsGrid({ title, models }) {
  return (
    <div className="mx-10">
      <h1 className="mb-8 text-3xl font-bold">{title}</h1>
      <div className="grid gap-6 grid-cols-[repeat(auto-fit,minmax(min(100%,280px),1fr))]">
        {models.map(model => (
          <ModelCard key={model.id} model={model} />
        ))}
      </div>
    </div>
  );
}