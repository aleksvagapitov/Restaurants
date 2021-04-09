import React, { Fragment } from "react";
import { observer } from "mobx-react-lite";
import { Grid, Item, Button } from "semantic-ui-react";
import { useHistory } from "react-router-dom";

export const AdminOwners = () => {
  let history = useHistory();

  return (
    <Fragment>
      <Grid>
        <Grid.Row>
          <Button
            primary
            floated="right"
            onClick={() => history.push(`/admin/owner/create`)}
            type="button"
            content="Create Owner"
            fluid
            color="green"
          ></Button>
        </Grid.Row>
        <Grid.Row>
          <Grid.Column>
            <Item.Group divided>
              {/* {restaurants.map((restaurant) => (
                <OwnerRestaurantDetailedCard key={restaurant.id} restaurant={restaurant} />
              ))} */}
            </Item.Group>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Fragment>
  );
};

export default observer(AdminOwners);
