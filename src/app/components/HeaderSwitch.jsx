'use client';
import { usePathname } from 'next/navigation';
import Header from './Header.jsx';

export default function HeaderSwitch() {
  const pathname = usePathname() || '';

  const isModelDetailPage =
    pathname.startsWith('/3d-models/') &&
    pathname !== '/3d-models' &&
    pathname !== '/3d-models/';

  if (isModelDetailPage) return <Header className="hidden md:block" />;

  return <Header />;
}