import { useState, useEffect } from "react";
import { useSearchParams, useParams, Link } from "react-router-dom";
import { getBlogs } from "../services/featureServices/blogService";
import {
  getUserReactions,
  voteBlog,
} from "../services/featureServices/blogReactionService";
import { showSuccessToast, showErrorToast } from "../utils/toastHelper";
import Layout from "../components/layout/Layout";
import Button from "../components/common/Button";

const HomePage = () => {
  const [blogs, setBlogs] = useState([]);
  const [totalBlogs, setTotalBlogs] = useState(0);
  const [userReactions, setUserReactions] = useState({});
  const { sortBy } = useParams();
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search") || "";
  const [page, setPage] = useState(0);
  const [isLoading, setIsLoading] = useState(true); // Add loading state
  const pageSize = 10;

  useEffect(() => {
    // Fetch blogs
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
        // showSuccessToast("Blogs fetched successfully");
      } catch {
        showErrorToast("Error fetching blogs");
      } finally {
        setIsLoading(false); // Set loading state to false
      }
    };

    // Fetch user reactions
    const fetchUserReactions = async () => {
      try {
        const response = await getUserReactions();
        const reactions = Array.isArray(response.data) ? response.data : [];

        // Map the blogId to reactionType
        const reactionMap = reactions.reduce(
          (map, { blogId, reactionType }) => {
            map[blogId] = reactionType;
            return map;
          },
          {}
        );

        // console.log("Fetched Reactions (reactionMap):", reactionMap);
        setUserReactions(reactionMap);
      } catch {
        showErrorToast("Error fetching user reactions");
      }
    };

    fetchBlogs();
    fetchUserReactions();
  }, [sortBy, search, page]);

  const handleVote = async (blogId, reactionType) => {
    const newReaction =
      userReactions[blogId] === reactionType ? 0 : reactionType;
    setUserReactions((prev) => ({ ...prev, [blogId]: newReaction }));

    const response = await voteBlog(blogId, newReaction);
    if (!response) {
      showErrorToast("Error submitting vote");
    }
  };

  // Calculate total pages
  const totalPages = Math.ceil(totalBlogs / pageSize);

  if (isLoading) {
    return (
      <Layout>
        <p className="text-center">Loading blogs...</p>
      </Layout>
    );
  }

  // When no blogs are found
  if (blogs.length === 0 && search) {
    return (
      <Layout>
        <p className="text-center">No blogs found</p>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {blogs.map((blog) => (
          <div key={blog.blogId} className="border p-4 rounded-lg shadow-md">
            <Link to={`/blog/blogById/${blog.blogId}`}>
              <h2 className="text-2xl font-semibold">{blog.title}</h2>
              <p className="mb-2 text-sm text-gray-500">By {blog.user.name}</p>
              <div className="w-full aspect-video mb-4 border rounded-lg">
                <img
                  src={blog.imageUrl}
                  alt={blog.title}
                  className="w-full h-full object-contain bg-gray-100 rounded-lg"
                />
              </div>
              <p className="text-gray-600">{blog.description}</p>
            </Link>

            {/* Voting Buttons */}
            <div className="flex flex-row mt-3">
              <Button
                icon={
                  userReactions[blog.blogId] === 1
                    ? "thumb_up"
                    : "thumb_up_off_alt"
                }
                className={
                  userReactions[blog.blogId] === 1 ? "text-sky-500 p-0" : "p-0"
                }
                onClick={() => handleVote(blog.blogId, 1)}
              />
              <Button
                icon={
                  userReactions[blog.blogId] === 2
                    ? "thumb_down"
                    : "thumb_down_off_alt"
                }
                className={
                  userReactions[blog.blogId] === 2 ? "text-red-500 p-0" : "p-0"
                }
                onClick={() => handleVote(blog.blogId, 2)}
              />
            </div>
          </div>
        ))}
      </div>

      {/* Pagination */}
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
          onClick={() => setPage((prev) => Math.min(prev + 1, totalPages - 1))}
          className={`bg-black text-white p-1 pr-1 ${
            page === totalPages - 1 ? "opacity-50 cursor-not-allowed" : ""
          }`}
        />
      </div>
    </Layout>
  );
};

export default HomePage;
