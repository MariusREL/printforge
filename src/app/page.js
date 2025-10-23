import Image from "next/image";

export default function Home() {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-20 lg:px-20">
      <div className="flex flex-col gap-2 px-5 pt-10 justify-center max-w-[600]">
        <p className="text-[16px] hidden lg:block">
          YOUR GO-TO PLATFORM FOR 3D PRINTING FILES
        </p>
        <h1 className="font-heading text-[40px] font-bold text-left lg:text-[56px]">
          Discover what's possible with 3D printing
        </h1>

        <p className="font-para text-left text-2xl lg:text-[28px]">
          Join our community of creators and explore a vast library of
          user-submitted models.
        </p>
        <button className="border-2 border-black w-49 h-10 mt-10 bg-white hover:bg-gray-100 cursor-pointer font-semibold text-xl ">
          BROWSE MODELS
        </button>
      </div>
      <div className="relative aspect-square  max-w-[600] lg:mt-10">
        <Image
          src="/home-page-hero-img.png"
          alt="A logo"
          fill
          className="object-contain"
        ></Image>
      </div>
    </div>
  );
}
