import React, { useEffect, useContext } from 'react'
import { observer } from 'mobx-react-lite';
import { Card, Container, Header, Divider } from 'semantic-ui-react';
import { RootStoreContext } from '../../app/stores/rootStore';
import { isNullOrUndefined } from 'util';
import { RestaurantCard } from './RestaurantCard';

interface IProps {
    term: string
}

export const RestaurantsShowcase: React.FC<IProps> = ({term}) => {
    const rootStore = useContext(RootStoreContext);
    const { frontPageRestaraunts ,listFrontPageRestaurants } = rootStore.restaurantStore;

    useEffect(() => {
      const today = new Date(Date.now());
      listFrontPageRestaurants(today, "1", term);
    }, [listFrontPageRestaurants, term]);

    return (
      <Container>
        <Header as="h2"  textAlign="left">
          {term} 
        </Header>
        <Divider />
        <Card.Group>
          {!isNullOrUndefined(frontPageRestaraunts.get(term)) ? (
            frontPageRestaraunts
              .get(term)!
              .map((restaurant) => (
                <RestaurantCard key={restaurant.id} restaurant={restaurant} />
              ))
          ) : (
            <Container></Container>
          )}
        </Card.Group>
        <br />
      </Container>
    );
}

export default observer(RestaurantsShowcase);
