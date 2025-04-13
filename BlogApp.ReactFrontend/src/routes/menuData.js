const getMenuData =  (isAuthenticated) =>[
  {
    icon: "home",
    name: "Home",
    link: "/",
    type: "single",
  },
  {
    parentIcon: "book",
    name: "Posts",
    type: "parent",
    children: [
      { icon: "shuffle", name: "Random", link: "/blog/filter/random" },
      { icon: "schedule", name: "Recent", link: "/blog/filter/recency" },
      { icon: "trending_up", name: "Popular", link: "/blog/filter/popularity" },
    ],
  },
  isAuthenticated && {
    icon: "article",
    name: "My Posts",
    link: "/blog/my-posts",
    type: "single",
  },
  isAuthenticated && {
    parentIcon: "settings",
    name: "Settings",
    type: "parent",
    children: [
      { icon: "account_circle", name: "Profile", link: "/user/profile" },
      { icon: "lock", name: "Password", link: "/user/password" },
    ],
  },
].filter(Boolean);

export default getMenuData;
