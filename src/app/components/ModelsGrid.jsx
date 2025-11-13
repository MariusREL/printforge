'use client';
import ModelCard from "@/app/components/ModelCard";
import { usePathname } from 'next/navigation';

export default function ModelsGrid({title, models}){
    const pathname = usePathname() || '';
    const isCategoryPage =
    pathname.startsWith('/3dmodels/categories') &&
    pathname !== '/3dmodels/categories/';
        return (
        <div className="mx-10">
            {!isCategoryPage && <h1 className="mb-8 text-3xl font-bold">{title}</h1>}
            
            <div
        className="grid gap-6 grid-cols-[repeat(auto-fit,minmax(230px,1fr))]"
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