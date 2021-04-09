import React, { useState } from "react";
import { Card, Image, Rating } from "semantic-ui-react";
import { IRestaurant } from "../../app/models/restaurant";
import _ from "lodash";

export const RestaurantCard: React.FC<{ restaurant: IRestaurant }> = ({
  restaurant,
}) => {
  const [starsAverage] = useState(
    Math.round(
      _.sumBy(restaurant.reviews, function (review) {
        return review.stars;
      }) / 
      restaurant.reviews.length
    )
  );

  return (
    <Card key={restaurant.id}>
      <Image
        size="medium"
        src={
          restaurant.image ||
          `./assets/categoryImages/${restaurant.categories[0].category}.jpg`
        }
        link
        href={`/restaurants/${restaurant.id}`}
      />
      <Card.Content>
        <Card.Header>{restaurant.name}</Card.Header>
        <Card.Description>
          <Rating
            size="large"
            defaultRating={starsAverage}
            disabled
            maxRating={5}
          />
          <span> {restaurant.reviews.length} Reviews</span>
        </Card.Description>
        <Card.Meta>
          {restaurant.city} • $$$ • {restaurant.phone}
        </Card.Meta>
      </Card.Content>
      {/* <Card.Content extra>
          <Button onClick={() => history.push(`/book/${restaurant.id}`)} type='button'
                  content='Book' fluid color="green" />
        </Card.Content> */}
    </Card>
  );
};
