import { useState, useEffect } from "react";
import { getUserReactions, voteBlog } from "../service/blogReactionService";
import {
  showErrorToast,
  showWarningToast,
} from "../../../common/utils/toastHelper";
import { useAuth } from "../../../common/contexts/AuthContext";
// import { useNavigate } from "react-router-dom";

export const useVoting = () => {
  const [userReactions, setUserReactions] = useState({});
  const { isAuthenticated } = useAuth();
  // const navigate = useNavigate();

  useEffect(() => {
    const fetchUserReactions = async () => {
      try {
        const response = await getUserReactions();
        const reactions = Array.isArray(response.data) ? response.data : [];
        const reactionMap = reactions.reduce(
          (map, { blogId, reactionType }) => {
            map[blogId] = reactionType;
            return map;
          },
          {}
        );
        setUserReactions(reactionMap);
      } catch {
        // showErrorToast("Error fetching user reactions");
      }
    };

    fetchUserReactions();
  }, []);


  // * Reset user reactions when the user logs out
  // * This is important to avoid showing the previous user's reactions
  useEffect(() => {
    if (!isAuthenticated) {
      setUserReactions({});
    }
  }, [isAuthenticated]);

  const handleVote = async (blogId, reactionType, onVoteSuccess) => {
    // Check if the user is autneticated or not
    if (!isAuthenticated) {
      showWarningToast("Please login to vote");
      return false;
      // navigate('/login');
    }

    const newReaction =
      userReactions[blogId] === reactionType ? 0 : reactionType;
    setUserReactions((prev) => ({ ...prev, [blogId]: newReaction }));

    const response = await voteBlog(blogId, newReaction);
    if (!response) {
      showErrorToast("Error submitting vote");
      return false;
    }

    if (onVoteSuccess) {
      onVoteSuccess(blogId, newReaction, userReactions[blogId]);
    }

    return true;
  };

  return { userReactions, handleVote };
};
