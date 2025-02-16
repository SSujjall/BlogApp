/* eslint-disable react/prop-types */
import Button from '../../../components/common/Button';

export const VoteButtons = ({ 
  blogId, 
  upVoteCount, 
  downVoteCount, 
  userReactions, 
  onVote 
}) => {
  return (
    <>
      <Button
        icon={userReactions[blogId] === 1 ? "thumb_up" : "thumb_up_off_alt"}
        className={
          userReactions[blogId] === 1
            ? "text-sky-500 pr-0 drop-shadow-md"
            : "pr-0"
        }
        onClick={() => onVote(blogId, 1)}
      />
      <span className="-ml-2 text-gray-600">{upVoteCount}</span>

      <Button
        icon={userReactions[blogId] === 2 ? "thumb_down" : "thumb_down_off_alt"}
        className={
          userReactions[blogId] === 2
            ? "text-red-500 pr-0 drop-shadow-md"
            : "pr-0"
        }
        onClick={() => onVote(blogId, 2)}
      />
      <span className="-ml-2 mr-2 text-gray-700">{downVoteCount}</span>
    </>
  );
};