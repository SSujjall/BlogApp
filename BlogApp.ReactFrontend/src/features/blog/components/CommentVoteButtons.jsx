/* eslint-disable react/prop-types */
import Button from "../../../components/common/Button";

export const CommentVoteButtons = ({
  blogId,
  upVoteCount,
  downVoteCount,
  userReactions,
  onVote,
}) => {
  return (
    <div className="flex flex-row gap-1 items-center ml-8 mt-2">
      <Button
        icon={"arrow_circle_up"}
        className={"p-0 text-sky-500 drop-shadow-md"}
      />
      <p className="-ml-3 text-xs">vote</p>
      <Button
        icon={"arrow_circle_down"}
        className={"p-0 text-red-500 drop-shadow-md -ml-2"}
      />
    </div>
  );
};
