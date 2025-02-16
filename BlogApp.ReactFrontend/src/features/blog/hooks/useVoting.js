import { useState, useEffect } from 'react';
import { getUserReactions, voteBlog } from '../service/blogReactionService';
import { showErrorToast } from '../../../common/utils/toastHelper';

export const useVoting = () => {
  const [userReactions, setUserReactions] = useState({});

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
        showErrorToast("Error fetching user reactions");
      }
    };

    fetchUserReactions();
  }, []);

  const handleVote = async (blogId, reactionType, onVoteSuccess) => {
    const newReaction = userReactions[blogId] === reactionType ? 0 : reactionType;
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