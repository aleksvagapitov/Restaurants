import React, { useContext, useEffect, Fragment } from "react";
import {
  Grid,
  Container,
  Image,
  Segment,
  Icon,
  Item,
  List,
} from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps } from "react-router";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { RestaurantDetailedSidebarBooking } from "./RestaurantDetailedSidebarBooking";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { RestaurantDetailedDescription } from "./RestaurantDetailedDescription";
import { RestaurantDetailedPhotos } from "./RestaurantDetailedPhotos";
import { IDay } from "../../../app/models/restaurant";
import { YMaps, Map, Placemark } from "react-yandex-maps";
import RestaurantDetailedReviews from "./RestaurantDetailedReviews";

interface DetailParams {
  id: string;
}

const RestaurantDetails: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    restaurant,
    loadRestaurant,
    loadingInitial,
  } = rootStore.restaurantStore;

  useEffect(() => {
    loadRestaurant(match.params.id);
  }, [loadRestaurant, match.params.id, history]);

  if (loadingInitial)
    return <LoadingComponent content="Loading restaurant..." />;
  else if (!restaurant) 
    return <h2>Restaurant not found</h2>;

  return (
    <Fragment>
      <Grid>
        <Grid.Row columns={4}>
          {restaurant.photos.slice(0, 4).map((photo) => (
            <Grid.Column key={photo.id}>
              <Image size="medium" fluid src={photo.url} />
            </Grid.Column>
          ))}
        </Grid.Row>
      </Grid>
      <Container>
        <Grid>
          <Grid.Column width={10}>
            <RestaurantDetailedDescription restaurant={restaurant} />
            <RestaurantDetailedPhotos restaurant={restaurant} />
            <RestaurantDetailedReviews restaurant={restaurant} />
          </Grid.Column>

          <Grid.Column width={6}>
            <RestaurantDetailedSidebarBooking id={restaurant.id} />
            <YMaps>
              <Map
                width="100%"
                defaultState={{ center: [restaurant.latitude, restaurant.longitude], zoom: 17 }}
              >
                <Placemark geometry={[restaurant.latitude, restaurant.longitude]} />
              </Map>
            </YMaps>
            <Segment.Group attached="bottom">
              <Segment>{restaurant.phone}</Segment>
              <Segment>
                <Icon name="clock outline" />
                Hours of Operation
                {restaurant.workHours.map((item) => (
                  <Item>
                    {IDay[item.dayOfWeek]} {item.startTime} - {item.endTime}
                  </Item>
                ))}
              </Segment>
              <Segment>
                <Icon name="food" />
                Cuisines
                <Container>
                  <List bulleted horizontal>
                    {restaurant.categories.map((item) => (
                      <List.Item>{item.category}</List.Item>
                    ))}
                  </List>
                </Container>
              </Segment>
            </Segment.Group>
          </Grid.Column>
        </Grid>
      </Container>
    </Fragment>
  );
};

export default observer(RestaurantDetails);
