import Image from "next/image";
import Link from "next/link";
import Pill from "./Pill";
import { FaRegHeart } from "react-icons/fa6";

export default function ModelCard({ model }) {
    return (
        <Link href={`/3d-models/${model.id}`} className="block hover:shadow-2xs hover:-translate-y-[3px] transition-all">
            <div className="overflow-hidden transition-shadow bg-white rounded-lg shadow-md hover:shadow-lg">
                <div className="relative aspect-square">
                    <Image 
                        src="/placeholder.png"
                        alt="placeholder image"
                        width={300}
                        height={300}
                        className="absolute inset-0 object-cover w-full h-full"
                        ></Image>
                </div>
                <div className="p-4">

                    <h2 className="font-bold ">
                        {model.name}
                    </h2>

                    <p className="text-gray-800 text-sm line-clamp-1 min-h-10 leading-5">
                        {model.description}
                    </p>

                    <div className="mt-2">
                        <Pill>
                            {model.category}
                        </Pill>
                    </div>

                    <div className="flex items-center mt-2 text-gray-600">
                        <FaRegHeart className="w-5 h-5 mr-1 text-gray-400"/>
                        <span>
                            {model.likes}
                        </span>
                    </div>
                </div>
            </div>
        </Link>
    )
}