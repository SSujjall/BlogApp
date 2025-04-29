import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Home from "../features/blog/pages/Home";
import NotFound from "../common/pages/NotFound";
import Login from "../features/auth/pages/Login";
import BlogDetail from "../features/blog/pages/BlogDetail";
import Signup from "../features/auth/pages/Signup";
import ForgotPassword from "../features/auth/pages/ForgotPassword";
import AddBlog from "../features/blog/pages/AddBlog";
import MyBlogPosts from "../features/blog/pages/MyBlogPosts";
import RedirectIfNotAuthenticated from "./RedirectIfNotAuthenticated";
import RedirectOnlyIfAuthenticated from "./RedirectOnlyIfAuthenticated";
import NotificationView from "../features/notification/pages/NotificationView";

const AppRoutes = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route
          path="/notifications"
          element={
            <RedirectOnlyIfAuthenticated>
              <NotificationView />
            </RedirectOnlyIfAuthenticated>
          }
        />

        {/* Blog Routes */}
        <Route path="/blog/filter/:sortBy" element={<Home />} />
        <Route path="/blog/blogById/:blogId" element={<BlogDetail />} />
        <Route path="/blog/addBlog" element={<AddBlog />} />
        <Route path="/blog/my-posts" element={<MyBlogPosts />} />

        {/* Auth Routes */}
        <Route
          path="/login"
          element={
            <RedirectIfNotAuthenticated>
              <Login />
            </RedirectIfNotAuthenticated>
          }
        />
        <Route
          path="/signup"
          element={
            <RedirectIfNotAuthenticated>
              <Signup />
            </RedirectIfNotAuthenticated>
          }
        />
        <Route
          path="/forgot-password"
          element={
            <RedirectIfNotAuthenticated>
              <ForgotPassword />
            </RedirectIfNotAuthenticated>
          }
        />

        {/* 404 Not Found */}
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;
