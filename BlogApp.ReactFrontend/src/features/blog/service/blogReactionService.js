import { makeRequest } from "../../../common/services/api";

// Fetch user reactions (only if logged in)
export const getUserReactions = async () => {
  try {
    return await makeRequest(
      "GET",
      "/BlogReaction/get-all-user-reactions",
      null,
      true
    );
  } catch (error) {
    console.error("Error fetching user reactions:", error);
    return [];
  }
};

// Vote on a blog (toggle upvote/downvote)
export const voteBlog = async (blogId, reactionType) => {
  try {
    return await makeRequest(
      "POST",
      "/BlogReaction/vote",
      { blogId, reactionType },
      true
    );
  } catch (error) {
    console.error("Error voting on blog:", error);
    return null;
  }
};
