import { useState, useEffect } from "react";
import { getBlogs } from "../services/blogService";
import { showSuccessToast, showErrorToast } from "../utils/toastHelper";
import Layout from "../components/layout/Layout";

const HomePage = () => {
  const [blogs, setBlogs] = useState([]);

  useEffect(() => {
    const fetchBlogs = async () => {
      try {
        const data = await getBlogs();
        setBlogs(data);
        showSuccessToast("Blogs fetched successfully");
      } catch  {
        showErrorToast("Error fetching blogs");
      }
    };

    fetchBlogs();
  }, []);

  return (
    <Layout>
      <h1 className="text-3xl font-semibold mb-4">Blogs</h1>
    </Layout>
  );
};

export default HomePage;
