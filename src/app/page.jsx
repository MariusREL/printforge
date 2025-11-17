import Image from "next/image";
import Link from "next/link";

export default function Home() {
  return (
    <div className="grid grid-cols-1 lg:grid-cols-2 gap-20 lg:px-20">
      <div className="flex flex-col gap-2 px-5 pt-10 justify-center max-w-[600]">
        <p className="text-[16px] hidden lg:block">
          YOUR GO-TO PLATFORM FOR 3D PRINTING FILES
        </p>
        <h1 className="text-[40px] font-bold text-left lg:text-[56px]">
          Discover what's possible with 3D printing
        </h1>

        <p className="text-left text-2xl lg:text-[28px]">
          Join our community of creators and explore a vast library of
          user-submitted models.
        </p>
        <Link href="/3dmodels">
          <button className="uppercase border-1 border-[var(--header-bg)] w-49 h-10 mt-10 hover:bg-gray-800 cursor-pointer font-semibold text-xl text-[var(--fg)] ">
            browse models
          </button>
        </Link>
      </div>
      <div className="relative aspect-square  max-w-[600] lg:mt-10 mb-5">
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
