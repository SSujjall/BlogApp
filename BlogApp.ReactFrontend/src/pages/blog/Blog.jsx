import { useState, useEffect } from "react";
import { useParams } from "react-router-dom"; // Import useParams to get the blogId from URL
import { getBlogById } from "../../services/featureServices/blogService";
import { getCommentsByBlogId } from "../../services/featureServices/commentService";
import { showErrorToast } from "../../utils/toastHelper";
import Layout from "../../components/layout/Layout";
import CommonInputField from "../../components/common/CommonInputField";

const Blog = () => {
  const { blogId } = useParams(); // Get the blogId from URL

  const [blog, setBlog] = useState(null);
  const [comments, setComments] = useState([]);

  useEffect(() => {
    const fetchBlogDetails = async () => {
      try {
        const data = await getBlogById(blogId); // Fetch the blog by its ID
        setBlog(data.data);
      } catch {
        showErrorToast("Error fetching blog details");
      }
    };

    const fetchComments = async () => {
      try {
        const data = await getCommentsByBlogId(blogId); // Fetch comments for the blog
        setComments(data.data);
      } catch {
        showErrorToast("Error fetching comments");
      }
    };

    fetchBlogDetails();
    fetchComments();
  }, [blogId]); // Re-fetch if the blogId changes

  if (!blog) {
    return (
      <Layout>
        <p>Loading blog...</p>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="border p-4 rounded-lg shadow-md max-w-4xl m-auto">
        <h1 className="text-3xl font-semibold mb-1">{blog.title}</h1>
        <p className="mb-4 text-sm text-gray-500">By {blog.user.name}</p>
        <p className="text-gray-600 mb-2">{blog.description}</p>
        <div className="w-full aspect-video mb-4 border rounded-lg">
          <img
            src={blog.imageUrl}
            alt={blog.title}
            className="w-full h-full object-contain bg-gray-100 rounded-lg"
          />
        </div>

        <hr className="mt-2 mb-2" />

        <CommonInputField
          icon={"comment"}
          placeholder={"Join the conversation"}
          classProp={"mb-4"}
        />

        <div className="border rounded-md shadow-md">
          {comments.map((comment) => (
            <div
              key={comment.commentId}
              className="border p-2 rounded-md m-3 flex flex-row gap-2 items-center"
            >
              <p className="w-8 h-8 bg-gray-500 text-white flex items-center justify-center rounded-full">
                {comment.user.name.charAt(0).toUpperCase()}
              </p>
              <p>{comment.commentDescription}</p>
            </div>
          ))}
        </div>
      </div>
    </Layout>
  );
};

export default Blog;
