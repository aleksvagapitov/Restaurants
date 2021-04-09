import React, { useContext } from "react";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Item, Container } from "semantic-ui-react";
import { RestaurantDetailedReviewsListItem } from "./RestaurantDetailedReviewsListItem";
import { observer } from "mobx-react-lite";

export const RestaurantDetailedReviewsList = () => {
  const rootStore = useContext(RootStoreContext);
  const { reviews } = rootStore.restaurantStore;
  return (
    <Container>
      <Item.Group divided>
        {reviews.map((review) => (
          <RestaurantDetailedReviewsListItem key={review.id} review={review} />
        ))}
      </Item.Group>
    </Container>
  );
};

export default observer(RestaurantDetailedReviewsList);