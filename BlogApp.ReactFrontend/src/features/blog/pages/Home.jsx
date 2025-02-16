import { useState, useEffect } from "react";
import { useSearchParams, useParams } from "react-router-dom";
import { getBlogs } from "../service/blogService";
import { showErrorToast } from "../../../common/utils/toastHelper";
import Layout from "../../../components/layout/Layout";
import Button from "../../../components/common/Button";
import { BlogCard } from "../components/BlogCard";
import { useVoting } from "../hooks/useVoting";
import { updateBlogVotes } from "../helpers/voteHelpers";

const Home = () => {
  const [blogs, setBlogs] = useState([]);
  const [totalBlogs, setTotalBlogs] = useState(0);
  const { sortBy } = useParams();
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search") || "";
  const [page, setPage] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const pageSize = 10;

  const { userReactions, handleVote } = useVoting();

  useEffect(() => {
    const fetchBlogs = async () => {
      setIsLoading(true);
      try {
        const data = await getBlogs({
          sortBy,
          search,
          skip: page * pageSize,
          take: pageSize,
        });
        setBlogs(data.data);
        setTotalBlogs(data.totalCount);
      } catch {
        showErrorToast("Error fetching blogs");
      } finally {
        setIsLoading(false);
      }
    };

    fetchBlogs();
  }, [sortBy, search, page, pageSize]);

  const handleVoteClick = async (blogId, reactionType) => {
    await handleVote(
      blogId,
      reactionType,
      (blogId, newReaction, previousReaction) => {
        setBlogs((prevBlogs) =>
          prevBlogs.map((blog) =>
            blog.blogId === blogId
              ? updateBlogVotes(blog, newReaction, previousReaction)
              : blog
          )
        );
      }
    );
  };

  // Calculate total pages
  const totalPages = Math.ceil(totalBlogs / pageSize);

  if (isLoading) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">Loading blogs...</p>
        </div>
      </Layout>
    );
  }

  if (blogs.length === 0 && search) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">
            No blogs found matching your search
          </p>
        </div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-8">
        {blogs.map((blog) => (
          <BlogCard
            key={blog.blogId}
            blog={blog}
            userReactions={userReactions}
            onVote={handleVoteClick}
          />
        ))}
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="mt-4 flex justify-end gap-4 items-center">
          <Button
            icon={"chevron_left"}
            onClick={() => setPage((prev) => Math.max(prev - 1, 0))}
            className={`bg-black text-white p-1 pr-1 ${
              page === 0 ? "opacity-50 cursor-not-allowed" : ""
            }`}
          />
          <div className="text-center">
            <span>
              Page {page + 1} of {totalPages}
            </span>
          </div>
          <Button
            icon={"chevron_right"}
            onClick={() =>
              setPage((prev) => Math.min(prev + 1, totalPages - 1))
            }
            className={`bg-black text-white p-1 pr-1 ${
              page === totalPages - 1 ? "opacity-50 cursor-not-allowed" : ""
            }`}
          />
        </div>
      )}
    </Layout>
  );
};

export default Home;
