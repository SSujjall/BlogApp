const menuData = [
  {
    name: "Home",
    link: "/",
    type: "single"
  },
  {
    name: "Blog",
    type: "parent",
    children: [
      { name: "All Posts", link: "/blog/posts" },
      { name: "Categories", link: "/blog/categories" },
      { name: "Tags", link: "/blog/tags" }
    ]
  },
  {
    name: "Settings",
    link: "/settings",
    type: "single"
  },
  {
    name: "User Management",
    type: "parent",
    children: [
      { name: "Users", link: "/users" },
      { name: "Roles", link: "/roles" },
      { name: "Permissions", link: "/permissions" }
    ]
  }
];

export default menuData;