import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Home from "../features/blog/pages/Home";
import NotFound from "../common/pages/NotFound";
import Login from "../features/auth/pages/Login";
import BlogDetail from "../features/blog/pages/BlogDetail";
import Signup from "../features/auth/pages/Signup";
import ForgotPassword from "../features/auth/pages/ForgotPassword";
import AddBlog from "../features/blog/pages/AddBlog";
import MyBlogPosts from "../features/blog/pages/MyBlogPosts";
import RedirectIfAuthenticated from "../routes/RedirectIfAuthenticated";
import EditBlog from "../features/blog/pages/EditBlog";
import ProtectedRoute from "./ProtectedRoutes";

const AppRoutes = () => {
  return (
    <Router>
      <Routes>
        {/** Protected Routes **/}
        <Route element={<ProtectedRoute />}>
          <Route path="/" element={<Home />} />

          {/* Blog Routes */}
          <Route path="/blog/filter/:sortBy" element={<Home />} />
          <Route path="/blog/blogById/:blogId" element={<BlogDetail />} />
          <Route path="/blog/addBlog" element={<AddBlog />} />
          <Route path="/blog/my-posts" element={<MyBlogPosts />} />
          <Route path="/blog/edit/:blogId" element={<EditBlog />} />
        </Route>

        {/* Public/Auth Routes */}
        <Route
          path="/login"
          element={
            <RedirectIfAuthenticated>
              <Login />
            </RedirectIfAuthenticated>
          }
        />
        <Route
          path="/signup"
          element={
            <RedirectIfAuthenticated>
              <Signup />
            </RedirectIfAuthenticated>
          }
        />
        <Route
          path="/forgot-password"
          element={
            <RedirectIfAuthenticated>
              <ForgotPassword />
            </RedirectIfAuthenticated>
          }
        />

        {/* 404 Not Found */}
        <Route path="*" element={<NotFound />} />
      </Routes>
    </Router>
  );
};

export default AppRoutes;
