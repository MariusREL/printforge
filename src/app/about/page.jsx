import Image from "next/image";

export default function About() {
  return (
    <main>
      {/* Hero Section */}
      <section className="container max-w-6xl px-5 py-10 mx-auto lg:px-10" aria-labelledby="about-heading">
        <div className="grid grid-cols-1 gap-12 lg:grid-cols-2 border-b border-zinc-200 pb-10">
          <div className="relative aspect-square">
            <Image
              src="/hero-image-square.png"
              alt="PrintForge community members collaborating on 3D printing projects"
              fill
              className="object-contain"
              priority
            />
          </div>
          
          <article className="flex flex-col justify-center gap-5">
            <p className="text-sm font-medium uppercase tracking-wide" aria-hidden="true">
              About PrintForge
            </p>
            <h1 id="about-heading" className="font-bold text-[40px] lg:text-[50px] leading-tight">
              Empowering makers worldwide
            </h1>
            <p className="text-[18px] lg:text-[20px] leading-relaxed">
              Founded in 2023, PrintForge has quickly become the go-to platform for 3D printing enthusiasts, makers, and professional designers to share and discover amazing STL files for 3D printing.
            </p>
            <p className="text-[18px] lg:text-[20px] leading-relaxed">
              Our mission is to foster a vibrant community where creativity meets technology, enabling anyone to bring their ideas to life through 3D printing.
            </p>
          </article>
        </div>
      </section>

      {/* Features Section */}
      <section className="py-12" aria-labelledby="features-heading">
        <div className="container max-w-6xl px-5 mx-auto lg:px-20">
          <h2 id="features-heading" className="sr-only">Key Features</h2>
          <div className="grid grid-cols-1 gap-8 lg:grid-cols-3 lg:divide-x lg:divide-zinc-200">
            <article className="flex flex-col gap-4 lg:pr-12">
              <h3 className="font-bold text-[24px] lg:text-[28px]">
                100K+ Models
              </h3>
              <p className="text-[16px] lg:text-[18px] leading-relaxed">
                Access our vast library of community-created 3D models, from practical tools to artistic creations.
              </p>
            </article>
            
            <article className="flex flex-col gap-4 lg:px-12">
              <h3 className="font-bold text-[24px] lg:text-[28px]">
                Active Community
              </h3>
              <p className="text-[16px] lg:text-[18px] leading-relaxed">
                Join thousands of makers who share tips, provide feedback, and collaborate on projects.
              </p>
            </article>
            
            <article className="flex flex-col gap-4 lg:pl-12">
              <h3 className="font-bold text-[24px] lg:text-[28px]">
                Free to Use
              </h3>
              <p className="text-[16px] lg:text-[18px] leading-relaxed">
                Most models are free to download, with optional premium features for power users.
              </p>
            </article>
          </div>
        </div>
      </section>

      <hr className="border-zinc-200" aria-hidden="true" />

      {/* Vision Section */}
      <section className="container max-w-3xl px-5 py-12 mx-auto" aria-labelledby="vision-heading">
        <h2 id="vision-heading" className="font-bold text-[40px] lg:text-[50px] mb-8">
          Our vision
        </h2>
        
        <div className="flex flex-col gap-8">
          <p className="text-[18px] lg:text-[20px] leading-relaxed">
            At PrintForge, we believe that 3D printing is revolutionizing the way we create, prototype, and manufacture. Our platform serves as a bridge between designers and makers, enabling the sharing of knowledge and creativity that pushes the boundaries of what's possible with 3D printing.
          </p>
          
          <hr className="w-full max-w-xs self-center border-zinc-300" aria-hidden="true" />
          
          <p className="text-[18px] lg:text-[20px] leading-relaxed">
            Whether you're a hobbyist looking for your next weekend project, an educator seeking teaching materials, or a professional designer wanting to share your creations, PrintForge provides the tools and community to support your journey in 3D printing.
          </p>
        </div>
      </section>
    </main>
  );
}