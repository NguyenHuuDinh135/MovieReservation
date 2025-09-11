import type { FooterItem, MainNavItem } from "@/types"

export type SiteConfig = typeof siteConfig

const links = {
  x: "https://twitter.com/sadmann17",
  github: "https://github.com/sadmann7/skateshop",
  githubAccount: "https://github.com/sadmann7",
  discord: "https://discord.com/users/sadmann7",
  calDotCom: "https://cal.com/sadmann7",
}

export const siteConfig = {
  name: "Skateshop",
  description:
    "An open source e-commerce skateshop build with everything new in Next.js.",
  url: "https://skateshop.sadmn.com",
  ogImage: "https://skateshop.sadmn.com/opengraph-image.png",
  links,
  mainNav: [
    {
      title: "Page",
      items: [
        {
          title: "Home",
          to: "/",
          description: "Learn more about our company.",
          items: [],
        },
        {
          title: "About Us",
          to: "/about",
          description: "Learn more about our company.",
          items: [],
        }
      ],
    },
    {
      title: "Movies",
      items: [
        {
          title: "All Movies",
          to: "/movies",
          description: "All the movies we have to offer.",
          items: [],
        },
        {
          title: "Now Showing",
          to: "/now-showing",
          description: "Check out the latest movies in theaters.",
          items: [],
        },
        {
          title: "Coming Soon",
          to: "/coming-soon",
          description: "Stay tuned for our upcoming features.",
          items: [],
        }
      ],

    },
    {
      title: "Theaters",
      to: "/theaters",
      description: "Browse our theater locations.",
      items: [
        {
          title: "All Theaters",
          to: "/theaters/all",
          description: "Browse all theater locations.",
          items: [],
        },
        {
          title: "Special Cinema",
          to: "/theaters/special-cinema",
          description: "The second theater location.",
          items: [],
        },
        {
          title: "3D Cinema",
          to: "/theaters/3d-cinema",
          description: "The third theater location.",
          items: [],
        }
      ],
    },
    

  ] satisfies MainNavItem[],
  footerNav: [
    {
      title: "Credits",
      items: [
        {
          title: "OneStopShop",
          to: "https://onestopshop.jackblatch.com",
          external: true,
        },
        {
          title: "Acme Corp",
          to: "https://acme-corp.jumr.dev",
          external: true,
        },
        {
          title: "craft.mxkaske.dev",
          to: "https://craft.mxkaske.dev",
          external: true,
        },
        {
          title: "Taxonomy",
          to: "https://tx.shadcn.com/",
          external: true,
        },
        {
          title: "shadcn/ui",
          to: "https://ui.shadcn.com",
          external: true,
        },
      ],
    },
    {
      title: "Help",
      items: [
        {
          title: "About",
          to: "/about",
          external: false,
        },
        {
          title: "Contact",
          to: "/contact",
          external: false,
        },
        {
          title: "Terms",
          to: "/terms",
          external: false,
        },
        {
          title: "Privacy",
          to: "/privacy",
          external: false,
        },
      ],
    },
    {
      title: "Social",
      items: [
        {
          title: "X",
          to: links.x,
          external: true,
        },
        {
          title: "GitHub",
          to: links.githubAccount,
          external: true,
        },
        {
          title: "Discord",
          to: links.discord,
          external: true,
        },
        {
          title: "cal.com",
          to: links.calDotCom,
          external: true,
        },
      ],
    },
    {
      title: "Lofi",
      items: [
        {
          title: "beats to study to",
          to: "https://www.youtube.com/watch?v=jfKfPfyJRdk",
          external: true,
        },
        {
          title: "beats to chill to",
          to: "https://www.youtube.com/watch?v=rUxyKA_-grg",
          external: true,
        },
        {
          title: "a fresh start",
          to: "https://www.youtube.com/watch?v=rwionZbOryo",
          external: true,
        },
        {
          title: "coffee to go",
          to: "https://www.youtube.com/watch?v=2gliGzb2_1I",
          external: true,
        },
      ],
    },
  ] satisfies FooterItem[],
}