

export type SiteConfig = typeof siteConfig

// --- LINKS ---
// Chỉ giữ lại các link hoạt động và có ý nghĩa
const links = {
  githubRepo: "https://github.com/sadmann7/skateshop",
  githubAccount: "https://github.com/sadmann7",
}

export const siteConfig = {
  name: "Movie",
  description: "An open source Movie Ticket platform.",
  url: "https://localhost:62111/", // Nhớ thay bằng domain thật khi deploy
  ogImage: "https://ui.shadcn.com/og.jpg", // Có thể thay bằng ảnh của bạn sau
  links,

  // --- MAIN NAV (Đơn giản hóa tối đa - không có items con) ---
  mainNav: [
    {
      title: "Home",
      to: "/",
    },
    {
      title: "About",
      to: "/about",
    },
    {
      title: "Movies",
      to: "/movies",
    },
    {
      title: "Theaters",
      to: "/theaters",
    },
  ],
}

export const META_THEME_COLORS = {
  light: "#ffffff",
  dark: "#09090b",
}