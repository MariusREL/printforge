"use client";

import { useEffect, useState } from "react";
import Image from "next/image";
import Link from "next/link";
import Pill from "./Pill";
import { FaRegHeart } from "react-icons/fa6";

const SPIN_CLASSES = [
  "spin-1",
  "spin-2",
  "spin-3",
  "spin-4",
  "spin-5",
  "spin-6",
  "spin-7",
  "spin-8",
  "spin-9",
  "spin-10",
];

export default function ModelCard({ model }) {
  const [spinClass, setSpinClass] = useState("spin-5"); // default mid-speed

  useEffect(() => {
    const randomIndex = Math.floor(Math.random() * SPIN_CLASSES.length);
    setSpinClass(SPIN_CLASSES[randomIndex]);
  }, []);

  return (
    <Link
      href={`/3dmodels/${model.id}`}
      className="block group hover:shadow-[0_5px_12px_rgba(0,0,0,0.1)] hover:-translate-y-[3px] transition-all"
      aria-labelledby={`model-${model.id}-title`}
    >
      <div
        className="max-h-[500px] max-w-[500px] mx-auto overflow-hidden transition-shadow rounded-lg shadow-md hover:shadow-lg bg-[--card-bg] text-[--card-fg] border border-[--card-border]"
        role="article"
      >
        <div className="relative aspect-square">
          <Image
            src="/cat.webp"
            alt={model.name}
            width={300}
            height={300}
            className={`animate-spin ${spinClass} absolute inset-0 object-cover w-full h-full`}
          />
        </div>
        <div className="p-4">
          <div className="flex justify-between min-h-[3rem]">
            <h2
              id={`model-${model.id}-title`}
              className="text-xl font-semibold line-clamp-2"
            >
              {model.name}
            </h2>
          </div>
          <p className="text-sm line-clamp-2 min-h-[2.5rem] leading-[1.25rem] opacity-90">
            {model.description}
          </p>
          <div className="mt-2">
            <Pill>{model.category}</Pill>
          </div>
          <div
            className="flex items-center mt-2 opacity-80"
            aria-label={`${model.likes} likes`}
          >
            <FaRegHeart className="w-5 h-5 mr-1" aria-hidden="true" />
            <span>{model.likes}</span>
          </div>
        </div>
      </div>
    </Link>
  );
}