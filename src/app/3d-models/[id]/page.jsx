export default async function page({params}){
    const {id} = await params
    return (
        <h1>hei {id}</h1>
    )
}