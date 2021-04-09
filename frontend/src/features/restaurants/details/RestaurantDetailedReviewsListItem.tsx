import React, { useState } from "react";
import { IReview } from "../../../app/models/restaurant";
import { Segment, Rating, Comment } from "semantic-ui-react";

export const RestaurantDetailedReviewsListItem: React.FC<{
  review: IReview;
}> = ({ review }) => {
  const [reviewDate] = useState(new Date(review.createdAt).toDateString());

  return (
      <Segment>
        <Comment.Group>
          <Comment>
            <Comment.Avatar
              as="a"
              src={"./assets/user.png"}
            />
            <Comment.Content>
              <Comment.Author>{review.displayName}</Comment.Author>
              <Comment.Metadata>
                <div>{reviewDate}</div>
                <div>
                  <Rating
                    size="large"
                    defaultRating={review.stars}
                    disabled
                    maxRating={5}
                  />
                </div>
              </Comment.Metadata>
              <Comment.Text>
                {review.body}
              </Comment.Text>
            </Comment.Content>
          </Comment>
        </Comment.Group>
      </Segment>
  );
};
