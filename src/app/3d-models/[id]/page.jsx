import { getModelById } from "@/app/library/models"
import Link from "next/link"
import Image from "next/image"
import { FaRegHeart } from "react-icons/fa6"
import Pill from "@/app/components/Pill"
import BackToOverview from "@/app/components/BackToOverview"
export default async function page({params}){
    
    const {id} = await params
    console.log(await getModelById(id));
    const { category, dateAdded, description, id: ObjId, image, likes, name} = await getModelById(id)
    
    
    return (
        <>  
            <div className="container max-w-6xl px-4 mx-auto">
                <BackToOverview className="md:hidden"/>
                <article className="grid gird-cols-1 gap-8 md:grid-cols-2">
                    <figure className="relative mt-3 overflow-hidden rounded-lg shadow-lg aspect-square">
                        <Image 
                            src="/placeholder.png"
                            alt="placeholder image"
                            width={300}
                            height={300}
                            className="absolute inset-0 object-cover w-full h-full"
                            ></Image>
                    </figure>

                    <section className="relative flex flex-col justify-center h-full">
                        <BackToOverview className="hidden md:block absolute top-[-15] left-[-15]"/>
                        <div className="flex items-center mt-2 text-gray-600">
                                <FaRegHeart className="w-5 h-5 mr-1 text-gray-400"/>
                                <span>
                                    {likes}
                                </span>
                        </div>
                        <div>
                            <h1>
        
                            </h1>
                        </div>
                        <div>
                        <Pill>{category}</Pill>
                        
                        </div>
                        <div className="flex flex-col justify-around">
                            <p>{description}</p>
                            <p>{dateAdded}</p>
                        </div>
                    </section>
                </article>
            </div>
        </>
    )
}