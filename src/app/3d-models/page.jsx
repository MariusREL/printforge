import getAllModels from "@/app/library/models";
import ModelsGrid from "../components/ModelsGrid";



export default async function ModelsPage(){
    const models = await getAllModels()
    return <ModelsGrid title="3D Models" models={models}/>
}