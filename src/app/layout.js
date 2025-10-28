import { Montserrat_Alternates, Albert_Sans } from "next/font/google";

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

import "./globals.css";
import Header from "@/app/components/Header.jsx";

export default function RootLayout({ children }) {
  return (
    <html
      lang="en"
      className={`${montserrat_alternates.variable} ${albert_sans.variable} antialiased`}
    >
      <body>
        <Header />
        {children}
      </body>
    </html>
  );
}
