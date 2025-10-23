import Image from "next/image";

export default function About(){
    return (
        <main >   
            <section className="">
                <div className="grid grid-cols-1 lg:grid-cols-2 lg:mx-10" >
                    <div className="relative aspect-square max-h-[400] lg:max-h-[627] lg:my-10 mx-5 ">
                        <Image 
                            src="/hero-image-square.png"
                            alt="A logo"
                            fill
                            className="object-contain">
                        </Image>
                    </div>
                    <article className="flex flex-col justify-center gap-5 mx-5">
                        <p className="font-albert text-[20px] font-medium">
                            ABOUT PRINTFORGE
                        </p>
                        <h1 className="font-mont font-medium text-[56px]">
                            Empowering Makers Worldwide
                        </h1>
                        <p>
                            Founded in 2023, PrintForge has quickly become the
                                go-to platform for 3D printing enthusiasts, makers,
                                and professional designers to share and discover
                                amazing STL files for 3D printing.
                        </p>
                        <p>
                            Our mission is to foster a vibrant community where
                                creativity meets technology, enabling anyone to
                                bring their ideas to life through 3D printing.
                        </p>
                    </article>
                </div>
                <article>
                    <section>
                        <h2></h2>
                        <p></p>
                    </section>
                    <section>
                        <h2></h2>
                        <p></p>
                    </section>
                    <section>
                        <h2></h2>
                        <p></p>
                    </section>
                </article>
                <article>
                    <section>
                        <h2>
                        </h2>
                        <p>

                        </p>
                        <p>

                        </p>
                    </section>
                </article>
            </section>
        </main>
    )
}