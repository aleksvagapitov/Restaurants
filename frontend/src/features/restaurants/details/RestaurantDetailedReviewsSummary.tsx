import React, { useContext, useState } from "react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Item, Segment, Divider, Header, Rating } from "semantic-ui-react";
import _ from "lodash";
import { IRestaurant } from "../../../app/models/restaurant";

interface IProps {
  restaurant: IRestaurant;
}

export const RestaurantDetailedReviewsSummary: React.FC<IProps> = ({
  restaurant,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { reviewsCount } = rootStore.restaurantStore;

  const [starsAverage] = useState(
    Math.round(
      _.sumBy(restaurant.reviews, function (review) {
        return review.stars;
      }) / restaurant.reviews.length
    )
  );

  return (
    <Segment basic>
      <Item.Group>
        <Item>
          <Item.Content>
            <Item.Header>What {reviewsCount} people are saying</Item.Header>
            <Divider />
            <Header size="small">Overall ratings and reviews</Header>
            <Item.Description>
              Reviews can only be made by diners who have eaten at this
              restaurant
            </Item.Description>
            <Item.Description>
              <div>
                <Rating
                  size="large"
                  defaultRating={starsAverage}
                  disabled
                  maxRating={5}
                />
                 based on recent ratings
              </div>
            </Item.Description>
            <Divider/>
          </Item.Content>
        </Item>
      </Item.Group>
    </Segment>
  );
};

export default observer(RestaurantDetailedReviewsSummary);
