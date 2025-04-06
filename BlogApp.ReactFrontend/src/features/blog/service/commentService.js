import { makeRequest } from "../../../common/services/api";

export const getCommentsByBlogId = async (blogId) => {
  return await makeRequest("GET", `/Comment/get-all/${blogId}`);
};

export const createComment = async (blogId, commentData) => {
  try {
    return await makeRequest(
      "POST",
      "/Comment/create",
      { blogId: blogId, commentDescription: commentData },
      true
    );
  } catch (error) {
    console.error("Error creating comment:", error);
    return null;
  }
};
