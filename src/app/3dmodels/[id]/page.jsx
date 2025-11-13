import { getModelById } from "@/app/library/models"
import Link from "next/link"
import Image from "next/image"
import { FaRegHeart } from "react-icons/fa6"
import Pill from "@/app/components/Pill"
import BackToOverview from "@/app/components/BackToOverview"
import { notFound } from 'next/navigation';

export default async function page({params}){
    
    const {id} = await params
    const model = await getModelById(id);

    // If no model is found, show the 404 page
    if (!model) {
        notFound();
    }

    const { category, dateAdded, description, id: ObjId, image, likes, name} = model
    
    
    return (
        <>  
            <div className="container max-w-6xl px-4 mx-auto">
                <BackToOverview className="md:hidden"/>
                <article className="grid gird-cols-1 gap-2 md:gap-8 md:grid-cols-2">
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
                        <BackToOverview className="hidden md:block absolute top-0 left-0"/>
                        <div className="flex items-center text-gray-600">
                                <FaRegHeart className="w-5 h-5 mr-2 text-gray-400"/>
                                <span className="text-[var(--header-bg)] text-[1.25rem]">
                                    {likes}
                                </span>
                        </div>
                        <div className="flex flex-col gap-5">
                            <div>
                                <h1 className="mt-1 text-3xl font-bold">
                                    {name}
                                </h1>
                            </div>
                            <div>
                                <Pill>{category}</Pill>
                            
                            </div>
                            <div className="h-[200px] md:h-0 flex flex-col justify-between md:block">
                                <p className="md:mb-3">{description}</p>
                                <div className="flex flex-col justify-around">
                                    <time className="text-[0.8rem]">Added on {new Date(dateAdded).toLocaleDateString('en-GB')}</time>
                                </div>
                            </div>
                        </div>
                    </section>
                </article>
            </div>
        </>
    )
}