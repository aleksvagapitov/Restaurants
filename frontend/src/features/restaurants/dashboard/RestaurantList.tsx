import React, { useContext, Fragment } from "react";
import { Item } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import RestaurantListItem from "./RestaurantListItem";

const RestaurantList: React.FC = () => {
  const rootStore = useContext(RootStoreContext);
  const { restaurants } = rootStore.restaurantStore;
  return (
    <Fragment>
      <Item.Group divided>
        {restaurants.map((restaurant) => (
          <RestaurantListItem key={restaurant.id} restaurant={restaurant} />
        ))}
      </Item.Group>
    </Fragment>
  );
};

export default observer(RestaurantList);
