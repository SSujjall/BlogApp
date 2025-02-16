import { makeRequest } from "../../../common/services/api";

export const getCommentsByBlogId = async (blogId) => {
  return makeRequest("GET", `/Comment/get-all?blogId=${blogId}`);
};

export const createComment = async (blogId, commentData) => {
  return makeRequest("POST", `/blogs/${blogId}/comments`, commentData);
};
