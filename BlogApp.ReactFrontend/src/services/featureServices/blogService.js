import { makeRequest } from "../api";

const getBlogs = async () => {
  return makeRequest("GET", "/Blog/get-blogs");
};

const createBlog = async (blogData) => {
  return makeRequest("POST", "/blogs", blogData);
};

export { getBlogs, createBlog };
