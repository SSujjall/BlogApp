/* eslint-disable react/prop-types */
import { Link } from "react-router-dom";
import { VoteButtons } from "./VoteButtons";
import Button from "../../../components/common/Button";

export const BlogCard = ({
  blog,
  userReactions,
  onVote,
  showFullContent = false,
  ownBlog = false,
}) => {
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
            <Link to={`/blog/edit/${blog.blogId}`}>
              <Button
                icon={"edit"}
                text={"edit"}
                className={"hover:bg-gray-200"}
              />
            </Link>
          )}
        </section>
      </div>
    </div>
  );
};
