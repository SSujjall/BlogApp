import { makeRequest } from "../api";

export const getCommentsByBlogId = async (blogId) => {
  return makeRequest("GET", `/Comment/get-all?blogId=${blogId}`);
};

export const createComment = async (blogId, commentData) => {
  return makeRequest("POST", `/blogs/${blogId}/comments`, commentData);
};
