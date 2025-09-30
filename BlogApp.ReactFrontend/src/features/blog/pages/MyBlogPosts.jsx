import { useEffect, useState } from "react";
import { useVoting } from "../hooks/useVoting";
import Layout from "../../../components/layout/Layout";
import { getUserBlogs } from "../service/blogService";
import { BlogCard } from "../components/BlogCard";
import { updateBlogVotes } from "../helpers/voteHelpers";
import { useAuth } from "../../../common/contexts/AuthContext";

const MyBlogPosts = () => {
  const [myBlogs, setMyBlogs] = useState([]);
  const { userReactions, handleVote } = useVoting();
  const [isLoading, setIsLoading] = useState(true);
  const { isAuthenticated } = useAuth();

  useEffect(() => {
    const fetchMyBlogs = async () => {
      setIsLoading(true);
      try {
        const data = await getUserBlogs();
        setMyBlogs(data.data);
      } catch {
        console.log("Error fetching blogs");
      } finally {
        setIsLoading(false);
      }
    };

    fetchMyBlogs();
  }, []);

  const handleVoteClick = async (blogId, reactionType) => {
    await handleVote(
      blogId,
      reactionType,
      (blogId, newReaction, previousReaction) => {
        setMyBlogs((prevBlogs) =>
          prevBlogs.map((blog) =>
            blog.blogId === blogId
              ? updateBlogVotes(blog, newReaction, previousReaction)
              : blog
          )
        );
      }
    );
  };

  // A callback to remove deleted blog from state
  const handleBlogDeleted = (deletedBlogId) => {
    setMyBlogs((prevBlogs) =>
      prevBlogs.filter((blog) => blog.blogId !== deletedBlogId)
    );
  };

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading your blog posts...</p>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <h1 className="text-3xl font-bold mb-5">My Posts</h1>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
        {myBlogs &&
          myBlogs.map((blog) => (
            <BlogCard
              key={blog.blogId}
              blog={blog}
              userReactions={userReactions}
              onVote={handleVoteClick}
              ownBlog={isAuthenticated}
              onDeleted={handleBlogDeleted}
            />
          ))}
      </div>
    </Layout>
  );
};

export default MyBlogPosts;
