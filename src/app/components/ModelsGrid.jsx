import ModelCard from "@/app/components/ModelCard";

export default function ModelsGrid({title, models}){
        return (
        <div className="container px-4 py-8 mx-auto">
        <h1 className="mb-8 text-3xl font-bold">{title}</h1>
        <div className="container grid sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6"
        role="region"
        aria-label="3D Models Gallery"
        >
        
            {models.map((model)=> (
             <ModelCard key={model.id} model={model}/>
       ))}
       </div>
       </div>
    )
    
}