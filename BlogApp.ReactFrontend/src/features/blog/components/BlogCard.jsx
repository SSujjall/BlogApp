/* eslint-disable react/prop-types */
import { Link } from "react-router-dom";
import { useState } from "react";
import { VoteButtons } from "./VoteButtons";
import Button from "../../../components/common/Button";
import ConfirmModal from "../components/ConfirmModal";
import { deleteBlog } from "../service/blogService";
import { showErrorToast, showSuccessToast } from "../../../common/utils/toastHelper";

export const BlogCard = ({
  blog,
  userReactions,
  onVote,
  showFullContent = false,
  ownBlog = false,
  onDeleted // used in MyBlogPosts.jsx to remove the deleted blog from the list
}) => {
  const [showConfirm, setShowConfirm] = useState(false);

  const handleBlogDelete = async () => {
    try {
      const res = await deleteBlog(blog.blogId);

      if (res?.statusCode === 200) {
        if (onDeleted) onDeleted(blog.blogId);
        setShowConfirm(false);
        showSuccessToast(res.message || "Blog deleted successfully.");
      }
    } catch (error) {
      // TODO: Need to update the error handling, this is not working properly
      // TODO: the axios is not letting the response to reach here because of 4** errors thrown.
      const errMsg =
      error?.response?.data?.message || // backend message
      error?.message || // JS error message
      "Error occurred when deleting blog. Please try again.";

      showErrorToast(errMsg);
    }
  }

  return (
    <div className="border p-4 rounded-lg shadow-sm">
      {/* Blog title, description and image. */}
      <>
        {!showFullContent ? (
          <Link to={`/blog/blogById/${blog.blogId}`}>
            <h2
              className={`font-semibold 
            ${showFullContent ? "text-3xl" : "text-2xl"}
            ${!showFullContent ? "line-clamp-1" : ""}`}
            >
              {blog.title}
            </h2>
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
        ) : (
          <>
            <h2
              className={`font-semibold 
            ${showFullContent ? "text-3xl" : "text-2xl"}
            ${!showFullContent ? "line-clamp-1" : ""}`}
            >
              {blog.title}
            </h2>
            <p className="mb-2 text-sm text-gray-500">By {blog.user.name}</p>
            <div className="w-full aspect-video mb-4 border rounded-lg">
              <img
                src={blog.imageUrl}
                alt={blog.title}
                className="w-full h-full object-contain bg-gray-100 rounded-lg"
              />
            </div>
            <p className="text-gray-600">{blog.description}</p>
          </>
        )}
      </>
      
      {/* Vote and comment buttons */}
      <div className="flex flex-row mt-3 items-center justify-between">
        <section className="flex items-center">
          <VoteButtons
            blogId={blog.blogId}
            upVoteCount={blog.upVoteCount}
            downVoteCount={blog.downVoteCount}
            userReactions={userReactions}
            onVote={onVote}
          />

          {!showFullContent ? (
            <Link to={`/blog/blogById/${blog.blogId}`}>
              <Button icon={"forum"} />
            </Link>
          ) : (
            <Button icon={"forum"} />
          )}
          <span className="-ml-2 text-gray-700">
            {blog.commentCount}{" "}
            {blog.commentCount === 1 ? "Comment" : "Comments"}
          </span>
        </section>

        <section>
          {ownBlog && (
            <div className="flex gap-2">
              <Link to={`/blog/edit/${blog.blogId}`}>
                <Button
                  icon={"edit"}
                  text={"edit"}
                  className={"hover:bg-gray-200"}
                />
              </Link>

              <Button
                icon={"delete"}
                text={"delete"}
                className={"hover:bg-red-500 hover:text-white"}
                onClick={() => setShowConfirm(true)}
              />
            </div>
          )}

          {/* Blog Delete confirmation modal */}
          <ConfirmModal
            isOpen={showConfirm}
            title="Delete Blog"
            message="Are you sure you want to delete this blog? This action cannot be undone."
            onConfirm={handleBlogDelete}
            onCancel={() => setShowConfirm(false)}
          />
        </section>
      </div>
    </div>
  );
};
