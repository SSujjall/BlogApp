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
import ResendVerification from "../features/auth/pages/ResendVerification";
import Subscriptions from "../features/subscriptions/pages/Subscriptions";
import OrderPage from "../features/subscriptions/pages/OrderPage";
import PaymentResult from "../features/subscriptions/pages/PaymentResult";
import OrderHistory from "../features/subscriptions/pages/OrderHistory";
import OrderDetail from "../features/subscriptions/pages/OrderDetail";

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

          {/* Subscription Routes */}
          <Route path="/subscriptions" element={<Subscriptions />} />
          <Route path="/subscriptions/order/:subscriptionId" element={<OrderPage />} />
          <Route path="/payments/result" element={<PaymentResult />} />
          <Route path="/settings/order-history" element={<OrderHistory />} />
          <Route path="/settings/order-detail/:orderId" element={<OrderDetail />} />
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
        <Route
          path="/resend-verification"
          element={
            <RedirectIfAuthenticated>
              <ResendVerification />
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
