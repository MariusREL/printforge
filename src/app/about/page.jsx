import Image from "next/image";

export default function About(){
    return (
        <main >   
            <section className="">
                <div className="grid grid-cols-1 lg:grid-cols-2 lg:px-10 lg:gap-12 py-10 mb-10 border-b border-solid border-zinc-200" >
                    <div className="relative aspect-square mx-5 lg:mx-0 ">
                        <Image 
                            src="/hero-image-square.png"
                            alt="A logo"
                            fill
                            className="object-contain">
                        </Image>
                    </div>
                    <article className="flex flex-col m-5 justify-center gap-5" >
                        <p className="font-albert text-[14px] lg:text-[0.9rem] font-medium">
                            ABOUT PRINTFORGE
                        </p>
                        <h1 className="font-mont font-bold text-[40px] lg:text-[50px]">
                            Empowering makers worldwide
                        </h1>
                        <p className="font-albert text-[24px] lg:text-[28px]">
                            Founded in 2023, PrintForge has quickly become the
                                go-to platform for 3D printing enthusiasts, makers,
                                and professional designers to share and discover
                                amazing STL files for 3D printing.
                        </p>
                        <p className="font-albert text-[24px] lg:text-[28px]">
                            Our mission is to foster a vibrant community where
                                creativity meets technology, enabling anyone to
                                bring their ideas to life through 3D printing.
                        </p>
                    </article>
                </div>
                <article className="grid gap-8 grid-cols-1 lg:grid-cols-3 lg:divide-x lg:divide-black px-5 lg:px-20 mb-10">
                    <section className="flex flex-col gap-4 lg:pr-12">
                        <h2 className="font-mont font-bold text-[28px]"> 100K+ Models</h2>
                        <p className="font-albert text-[20px]">Access our vast library of community-created 3D
                                models, from practical tools to artistic
                                creations.</p>
                    </section>
                    <section className="flex flex-col gap-4 lg:px-12">
                        <h2 className="font-mont font-bold text-[28px]">Active Community</h2>
                        <p className="font-albert text-[20px]">Join thousands of makers who share tips, provide
                                feedback, and collaborate on projects.</p>
                    </section>
                    <section className="flex flex-col gap-4 lg:pl-12">
                        <h2 className="font-mont font-bold text-[28px]">Free to Use</h2>
                        <p className="font-albert text-[20px]">Most models are free to download, with optional
                                premium features for power users.</p>
                    </section>
                </article>

                <div className="w-full self-center h-px bg-zinc-300 mb-10"></div>

                <article className="px-5 lg:px-20 mb-16 lg:mx-[25%]">
                    <h2 className="font-mont font-bold text-[40px] lg:text-[50px] mb-3">
                        Our vision
                    </h2>

                    <div className="flex flex-col gap-8">
                        <p className="font-albert text-[18px] lg:text-[20px] text-zinc-700 leading-relaxed">
                            At PrintForge, we believe that 3D printing is revolutionizing the way we create, prototype, and manufacture. Our platform serves as a bridge between designers and makers, enabling the sharing of knowledge and creativity that pushes the boundaries of what's possible with 3D printing.
                        </p>
                        
                        <div className="w-3/5 lg:w-3.5/5 self-center max-w-xs h-px bg-black"></div>
                        
                        <p className="font-albert text-[18px] lg:text-[20px] text-zinc-700 leading-relaxed">
                            Whether you're a hobbyist looking for your next weekend project, an educator seeking teaching materials, or a professional designer wanting to share your creations, PrintForge provides the tools and community to support your journey in 3D printing.
                        </p>
                    </div>
                </article>
            </section>
        </main>
    )
}