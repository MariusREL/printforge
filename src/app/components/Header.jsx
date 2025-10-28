import Link from 'next/link';
import Image from 'next/image';

export default function Header() {
  return (
    <header className='mb-10'>
      <div className='container px-5 md:px-0 py-4 mx-auto'>
        <nav className='flex justify-between md:px-9'>
          <div className='flex items-center w-5xl'>
            <Link href="/">
              <Image
                src="/printforge-logo-icon.svg"
                alt="PrintForge logo icon"
                width={20}
                height={20}
                className='md:hidden h-8 w-auto '
              />
            </Link>
            <Link href="/">
              <Image
                src="/printforge-logo.svg"
                alt="PrintForge logo"
                width={50}
                height={50}
                className='hidden md:block h-8 w-auto'
              />
            </Link>
  
          </div>
          <ul className='flex gap-10 uppercase font-albert text-sm'>
            <li>
              <Link href="/models" className='whitespace-nowrap'>
                3D Models
              </Link>
            </li>
            <li>
              <Link href="/about">
                About
              </Link>
            </li>
          </ul>
        </nav>
      </div>
    </header>
  );
}