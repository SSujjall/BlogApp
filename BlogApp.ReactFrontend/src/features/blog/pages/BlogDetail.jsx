import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getBlogById } from "../service/blogService";
import { getCommentsByBlogId } from "../service/commentService";
import { showErrorToast } from "../../../common/utils/toastHelper";
import Layout from "../../../components/layout/Layout";
import CommonInputField from "../../../components/common/CommonInputField";
import { BlogCard } from "../components/BlogCard";
import { useVoting } from "../hooks/useVoting";
import { updateBlogVotes } from "../helpers/voteHelpers";
import Button from "../../../components/common/Button";

const BlogDetail = () => {
  const { blogId } = useParams();
  const [blog, setBlog] = useState(null);
  const [comments, setComments] = useState([]);
  const [newComment, setNewComment] = useState("");
  const { userReactions, handleVote } = useVoting();
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      setIsLoading(true);
      try {
        const [blogData, commentsData] = await Promise.all([
          getBlogById(blogId),
          getCommentsByBlogId(blogId),
        ]);

        setBlog(blogData.data);
        setComments(commentsData.data);
      } catch {
        showErrorToast("Error loading blog content");
      } finally {
        setIsLoading(false);
      }
    };

    fetchData();
  }, [blogId]);

  const handleVoteClick = async (blogId, reactionType) => {
    await handleVote(
      blogId,
      reactionType,
      // dont remove blog id below, it creates a glitch that shows the vote negative
      (blogId, newReaction, previousReaction) => {
        setBlog((prevBlog) =>
          updateBlogVotes(prevBlog, newReaction, previousReaction)
        );
      }
    );
  };

  const handleCommentSubmit = (e) => {
    e.preventDefault();
    // Implement comment submission logic here
    setNewComment("");
  };

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading blog...</p>
        </div>
      </Layout>
    );
  }

  if (!blog) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Blog not found</p>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="mx-auto max-w-4xl">
        <BlogCard
          blog={blog}
          userReactions={userReactions}
          onVote={handleVoteClick}
          showFullContent={true}
        />

        {/* Comments Section */}
        <div className="mt-8">
          <h2 className="text-2xl font-semibold mb-4">Comments</h2>

          {/* Comment Input */}
          <div className="mb-6">
            <CommonInputField
              icon="comment"
              placeholder="Join the conversation"
              value={newComment}
              onChange={(e) => setNewComment(e.target.value)}
              classProp="mb-2"
            />

            <Button
              icon={"add_comment"}
              text={"Add Comment"}
              className={`text-white ${
                newComment.trim()
                  ? "bg-blue-600 hover:bg-blue-700"
                  : "bg-gray-400 cursor-not-allowed"
              }`}
              disabled={!newComment.trim()}
              onClick={handleCommentSubmit}
            />
          </div>

          {/* Comments List */}
          <div className="space-y-4">
            {comments.length === 0 ? (
              <p className="text-gray-500 text-center py-4">
                No comments yet. Be the first to comment!
              </p>
            ) : (
              comments.map((comment) => (
                <div
                  key={comment.commentId}
                  className="border p-4 rounded-lg shadow-sm"
                >
                  <div className="flex items-center gap-3 mb-2">
                    <div className="w-8 h-8 bg-gray-500 text-white flex items-center justify-center rounded-full">
                      {comment.user.name.charAt(0).toUpperCase()}
                    </div>
                    <div>
                      <p className="font-medium">{comment.user.name}</p>
                      <p className="text-sm text-gray-500">
                        {new Date(comment.createdAt).toLocaleDateString()}
                      </p>
                    </div>
                  </div>
                  <p className="text-gray-700 ml-11">
                    {comment.commentDescription}
                  </p>
                </div>
              ))
            )}
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default BlogDetail;
