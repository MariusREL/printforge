import CategoryBar from "@/app/components/CategoryBar";

export default function categoryLayout({ children }) {
  return (
    <>
      <CategoryBar />
      {children}
    </>
  );
}
