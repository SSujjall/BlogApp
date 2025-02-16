export const updateBlogVotes = (blog, newReaction, previousReaction) => {
    return {
      ...blog,
      upVoteCount:
        newReaction === 1
          ? blog.upVoteCount + 1
          : previousReaction === 1
          ? blog.upVoteCount - 1
          : blog.upVoteCount,
      downVoteCount:
        newReaction === 2
          ? blog.downVoteCount + 1
          : previousReaction === 2
          ? blog.downVoteCount - 1
          : blog.downVoteCount,
    };
  };