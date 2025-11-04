import getAllModels from "@/app/library/models";
import ModelCard from "@/app/components/ModelCard";


export default async function ModelsPage(){
    const models = await getAllModels()
    return (
        <>
        <h1>All Models</h1>
        <div className="container grid sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-3">
        
        
        {models.map((model)=> (
             <ModelCard key={model.id} model={model}/>
       ))}
       </div>
       </>
    )
}