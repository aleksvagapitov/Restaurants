import React, { useState } from "react";
import { Image, Rating, Item, Segment } from "semantic-ui-react";
import { IRestaurant } from "../../../app/models/restaurant";
import { observer } from "mobx-react-lite";
import _ from "lodash";

export const RestaurantListItem: React.FC<{ restaurant: IRestaurant }> = ({
  restaurant,
}) => {
  const [starsAverage] = useState(
    Math.round(
      _.sumBy(restaurant.reviews, function (review) {
        return review.stars;
      }) / restaurant.reviews.length
    )
  );

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Image
              size="small"
              src={
                restaurant.image ||
                `./assets/categoryImages/${restaurant.categories[0].category}.jpg`
              }
            />
            <Item.Content link href={`/restaurants/${restaurant.id}`}>
              <Item.Header>{restaurant.name}</Item.Header>
              <Item.Meta>
                <Rating
                  size="large"
                  defaultRating={starsAverage}
                  disabled
                  maxRating={5}
                />
                <span> {restaurant.reviews.length} Reviews</span>
              </Item.Meta>
              <Item.Meta>
                {restaurant.city} • $$$ • {restaurant.phone}
              </Item.Meta>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
    </Segment.Group>
  );
};

export default observer(RestaurantListItem);
