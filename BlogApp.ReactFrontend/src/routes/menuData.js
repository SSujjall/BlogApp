const menuData = [
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
      { icon: "shuffle", name: "Random", link: "/blog/random" },
      { icon: "schedule", name: "Recent", link: "/blog/recency" },
      { icon: "trending_up", name: "Popular", link: "/blog/popularity" },
    ],
  },
  {
    parentIcon: "settings",
    name: "Settings",
    type: "parent",
    children: [
      { icon: "account_circle", name: "Profile", link: "/user/profile" },
      { icon: "lock", name: "Password", link: "/user/password" },
    ],
  },
];

export default menuData;
