import React, { Fragment, useContext, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Grid, Item, Button } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import OwnerRestaurantDetailedCard from "./OwnerRestaurantDetailedCard";
import { useHistory } from "react-router-dom";
import LoadingComponent from "../../app/layout/LoadingComponent";

export const OwnerRestaurants = () => {
  let history = useHistory();
  const rootStore = useContext(RootStoreContext);
  const { restaurants, listRestaurants, loadingRestaurants } = rootStore.ownerStore;

  // Selected Tab and Selected Restaurant in the ownerStore

  useEffect(() => {
    listRestaurants();
  }, [listRestaurants]);

  if (loadingRestaurants) return <LoadingComponent content="Loading restaurants..." />;

  return (
    <Fragment>
      <Grid>
        <Grid.Row>
          <Button
            primary
            floated="right"
            onClick={() => history.push(`/owner/restaurant/create`)}
            type="button"
            content="Add Restaurant"
            fluid
            color="green"
          ></Button>
        </Grid.Row>
        <Grid.Row>
          <Grid.Column>
            <Item.Group divided>
              {restaurants.map((restaurant) => (
                <OwnerRestaurantDetailedCard key={restaurant.id} restaurant={restaurant} />
              ))}
            </Item.Group>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Fragment>
  );
};

export default observer(OwnerRestaurants);
