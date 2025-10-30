import getAllModels from "@/app/library/models";
export default async function ModelsPage(){
    const models = await getAllModels()
    return (
        models.map((model)=> {

        })
    )
}