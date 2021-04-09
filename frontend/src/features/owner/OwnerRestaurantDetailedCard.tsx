import React from "react";
import { Image, Button, Item, Segment } from "semantic-ui-react";
import { IRestaurant } from "../../app/models/restaurant";
import { useHistory } from "react-router-dom";
import { observer } from "mobx-react-lite";

export const OwnerRestaurantDetailedCard: React.FC<{
  restaurant: IRestaurant;
}> = ({ restaurant }) => {
  let history = useHistory();

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Image size="small" src={restaurant.image || `./assets/categoryImages/${restaurant.categories[0].category}.jpg`} />
            <Item.Content>
              <Item.Header>{restaurant.name}</Item.Header>
              <Item.Meta>
                {restaurant.city} • {restaurant.address}
              </Item.Meta>
              <Item.Extra>
                <Button
                  primary
                  floated="right"
                  onClick={() =>
                    history.push(`/owner/${restaurant.id}`)
                  }
                  type="button"
                  content="Edit Restaurant"
                  fluid
                  color="green"
                />
                <Button
                  primary
                  floated="right"
                  onClick={() =>
                    history.push(`/owner/${restaurant.id}/reservations`)
                  }
                  type="button"
                  content="View Reservations"
                  fluid
                  color="green"
                />
              </Item.Extra>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>
    </Segment.Group>
  );
};

export default observer(OwnerRestaurantDetailedCard);
