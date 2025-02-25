import { makeRequest } from "../../../common/services/api";

export const getBlogs = async ({
  sortBy = "",
  search = "",
  skip = 0,
  take = 10,
}) => {
  const params = new URLSearchParams();

  if (sortBy) params.append("sortBy", sortBy);
  if (search) params.append("search", search);
  params.append("skip", skip);
  params.append("take", take);

  return makeRequest("GET", `/Blog/get-blogs?${params.toString()}`);
};

export const getBlogById = async (blogId) => {
  return makeRequest("GET", `/Blog/get-by-id/${blogId}`);
};

export const createBlog = async (blogData) => {
  return makeRequest("POST", "/Blog/create", blogData);
};
