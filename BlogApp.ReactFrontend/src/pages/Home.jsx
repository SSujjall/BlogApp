import { useState, useEffect } from "react";
import { useSearchParams, useParams, Link } from "react-router-dom";
import { getBlogs } from "../services/featureServices/blogService";
import { showSuccessToast, showErrorToast } from "../utils/toastHelper";
import Layout from "../components/layout/Layout";
import Button from "../components/common/Button";

const HomePage = () => {
  const [blogs, setBlogs] = useState([]);
  const { sortBy } = useParams();
  const [searchParams] = useSearchParams();
  const search = searchParams.get("search") || "";
  const [page, setPage] = useState(0);
  const pageSize = 10;

  useEffect(() => {
    const fetchBlogs = async () => {
      try {
        const data = await getBlogs({
          sortBy,
          search,
          skip: page * pageSize,
          take: pageSize,
        });
        setBlogs(data.data);
        // showSuccessToast("Blogs fetched successfully");
      } catch {
        showErrorToast("Error fetching blogs");
      }
    };

    fetchBlogs();
  }, [sortBy, search, page]);

  if (blogs.length === 0) {
    return (
      <Layout>
        <p className="text-center">Loading blogs...</p>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {blogs.map((blog) => (
          <Link to={`/blog/blogById/${blog.blogId}`} key={blog.blogId}>
            <div key={blog.blogId} className="border p-4 rounded-lg shadow-md">
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
            </div>
          </Link>
        ))}
      </div>

      {/* Pagination TODO HERE */}
      <Button
        text="next"
        onClick={() => setPage((prev) => prev + 1)}
        className={"mt-4 bg-black text-white"}
      />
    </Layout>
  );
};

export default HomePage;
