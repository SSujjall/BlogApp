import { makeRequest } from "./api";

const getComments = async (blogId) => {
  return makeRequest("GET", `/blogs/${blogId}/comments`);
};

const createComment = async (blogId, commentData) => {
  return makeRequest("POST", `/blogs/${blogId}/comments`, commentData);
};

export { getComments, createComment };
