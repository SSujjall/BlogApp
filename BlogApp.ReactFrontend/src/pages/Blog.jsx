import React from "react";
import { Link } from "react-router-dom";

const Blog = ({ blog }) => {
  return (
    <div className="mb-4 p-4 border border-gray-200 rounded-lg">
      <h2 className="text-xl font-semibold">{blog.title}</h2>
      <p>{blog.description}</p>
      <Link
        to={`/blog/${blog.id}`}
        className="text-blue-500 hover:text-blue-700"
      >
        Read more
      </Link>
    </div>
  );
};

export default Blog;
