import { useState, useEffect } from "react";
import { useSearchParams, useParams } from "react-router-dom";
import { getBlogs } from "../service/blogService";
// import { showErrorToast } from "../../../common/utils/toastHelper";
import Layout from "../../../components/layout/Layout";
import Button from "../../../components/common/Button";
import { BlogCard } from "../components/BlogCard";
import { useVoting } from "../hooks/useVoting";
import { updateBlogVotes } from "../helpers/voteHelpers";

const Home = () => {
  const [blogs, setBlogs] = useState([]);
  const [totalBlogs, setTotalBlogs] = useState(0);
  const { sortBy } = useParams();
  const [searchParams, setSearchParams] = useSearchParams();
  const search = searchParams.get("search") || "";
  const pageSize = 10;

  // Read page from URL (default to 0)
  const initialPage = parseInt(searchParams.get("page")) || 0;
  const [page, setPage] = useState(initialPage);
  const [isLoading, setIsLoading] = useState(true);

  const { userReactions, handleVote } = useVoting();

  useEffect(() => {
    const fetchBlogs = async () => {
      setIsLoading(true);
      try {
        const apiRes = await getBlogs({
          sortBy,
          search,
          skip: page * pageSize,
          take: pageSize,
        });
        setBlogs(Array.isArray(apiRes?.data) ? apiRes.data : []);
        setTotalBlogs(typeof apiRes?.totalCount === "number" ? apiRes.totalCount : 0);
      } catch {
        // showErrorToast("Error fetching blogs");
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

  // Handle page change and update URL
  const changePage = (newPage) => {
    setPage(newPage);
    setSearchParams((prev) => {
      const params = new URLSearchParams(prev);
      params.set("page", newPage);
      return params;
    });
  };

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

  if (blogs.length === 0) {
    return (
      <Layout>
        <div className="flex justify-center items-center min-h-[50vh]">
          <p className="text-lg text-gray-600">
            No blogs found.
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
            onClick={() => changePage(Math.max(page - 1, 0))}
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
            onClick={() => changePage(Math.min(page + 1, totalPages - 1))}
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
