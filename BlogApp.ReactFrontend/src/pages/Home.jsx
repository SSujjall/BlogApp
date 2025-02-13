import { useState, useEffect } from "react";
import { useSearchParams, useParams } from "react-router-dom";
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

  return (
    <Layout>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {blogs.map((blog) => (
          <div key={blog.blogId} className="border p-4 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold">{blog.title}</h2>
            <p className="mb-2 text-sm text-gray-500">By {blog.user.name}</p>
            <img
              src={blog.imageUrl}
              alt={blog.title}
              className="w-full h-64 object-cover mb-4 rounded-lg"
            />
            <p className="text-gray-600">{blog.description}</p>
          </div>
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
