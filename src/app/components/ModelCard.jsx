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
                <h1 className="font-bold ">{model.name}</h1>
                <p>{model.description}</p>
                <Pill>{model.category}</Pill>
                <div className="flex gap-1 items-center mt-2">
                    <FaRegHeart />
                    <span>{model.likes} likes</span>
                </div>
            </div>
        </Link>
    )
}