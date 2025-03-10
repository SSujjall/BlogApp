import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Home from "../features/blog/pages/Home";
import NotFound from "../common/pages/NotFound";
import Login from "../features/auth/pages/Login";
import BlogDetail from "../features/blog/pages/BlogDetail";
import Signup from "../features/auth/pages/Signup";
import ForgotPassword from "../features/auth/pages/ForgotPassword";
import AddBlog from "../features/blog/pages/AddBlog";
import MyBlogPosts from "../features/blog/pages/MyBlogPosts";

const AppRoutes = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />

        {/* Blog Routes */}
        <Route path="/blog/filter/:sortBy" element={<Home />} />
        <Route path="/blog/blogById/:blogId" element={<BlogDetail />} />
        <Route path="/blog/addBlog" element={<AddBlog />} />
        <Route path="/blog/my-posts" element={<MyBlogPosts />} />

        {/* Auth Routes */}
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<Signup />} />
        <Route path="/forgot-password" element={<ForgotPassword />} />

        {/* 404 Not Found */}
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;
