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

  return await makeRequest("GET", `/Blog/get-blogs?${params.toString()}`);
};

export const getBlogById = async (blogId) => {
  return await makeRequest("GET", `/Blog/get-by-id/${blogId}`);
};

export const createBlog = async (blogData) => {
  // console.log(blogData);
  const formData = new FormData();
  formData.append("Title", blogData.title);
  formData.append("Description", blogData.description);
  if (blogData.imageFile) {
    formData.append("ImageUrl", blogData.imageFile); // Ensure this matches DTO
  }

  return await makeRequest("POST", "/Blog/create", formData, true);
};

export const getUserBlogs = async () => {
  return await makeRequest("GET", "/Blog/get-user-blogs", null, true);
};

export const updateBlog = async (paramBlogId, blogUpdatedData) => {
  const formData = new FormData();
  formData.append("BlogId", paramBlogId);
  formData.append("Title", blogUpdatedData.title);
  formData.append("Description", blogUpdatedData.description);
  if (blogUpdatedData.imageFile) {
    formData.append("ImageUrl", blogUpdatedData.imageFile); // Ensure this matches DTO
  }

  return await makeRequest("PUT", "Blog/update", formData, true);
};
