import { Montserrat_Alternates, Albert_Sans } from "next/font/google";
import "./globals.css";
import HeaderSwitch from "@/app/components/HeaderSwitch";

export const montserrat_alternates = Montserrat_Alternates({
  subsets: ["latin"],
  display: "swap",
  variable: "--font-mont",
  weight: ["400", "500", "600", "700", "800"],
});

export const albert_sans = Albert_Sans({
  subsets: ["latin"],
  display: "swap",
  variable: "--font-albert",
  weight: ["400", "500", "700"],
});

export default function RootLayout({ children }) {
  return (
    <html
      lang="en"
      className={`${montserrat_alternates.variable} ${albert_sans.variable} antialiased`}
    >
      <body>
        <HeaderSwitch />
        {children}
      </body>
    </html>
  );
}
