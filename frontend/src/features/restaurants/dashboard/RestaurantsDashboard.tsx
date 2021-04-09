import React, { useContext, useEffect, Fragment, useState } from "react";
import { observer } from "mobx-react-lite";
import { RouteComponentProps, useHistory } from "react-router-dom";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Grid } from "semantic-ui-react";
import SearchForm from "./SearchForm";
import { YMaps, Map, Clusterer, Placemark } from "react-yandex-maps";
import InfiniteScroll from "react-infinite-scroller";
import RestaurantList from "./RestaurantList";
import RestaurantFilters from "./RestaurantFilters";
import RestaurantListItemPlaceholder from "./RestaurantListItemPlaceholder";

const RestaurantsFilter: React.FC<RouteComponentProps> = ({
  match,
  location,
}) => {
  let history = useHistory();
  const rootStore = useContext(RootStoreContext);
  const {
    restaurants,
    loadingInitial,
    page,
    searchParams,
    searchParamsValues,
    setPage,
    totalPages,
    loadRestaurantsFiltered,
  } = rootStore.restaurantStore;

  const [loadingNext, setLoadingNext] = useState(false);

  const handleGetNext = () => {
    setLoadingNext(true);
    setPage(page + 1);
    loadRestaurantsFiltered().then(() => setLoadingNext(false));
  };

  useEffect(() => {  
    loadRestaurantsFiltered();
  }, [loadRestaurantsFiltered, searchParamsValues])

  return (
    <Fragment>
      <Grid>
        <Grid.Row>
          <SearchForm match={match} location={location} history={history} />
          <RestaurantFilters />
        </Grid.Row>
        <Grid.Row columns={2}>
          <Grid.Column>
            {loadingInitial && page === 0 ? (
                <RestaurantListItemPlaceholder/>
            ) : (
              <InfiniteScroll
                pageStart={0}
                loadMore={handleGetNext}
                hasMore={!loadingNext && page + 1 < totalPages}
                initialLoad={false}
              >
                <RestaurantList />
              </InfiniteScroll>
            )}
            {/* <Item.Group divided>
              {restaurants.map((restaurant) => (
                <RestaurantDetailedCard
                  key={restaurant.id}
                  restaurant={restaurant}
                />
              ))}
            </Item.Group> */}
          </Grid.Column>
          <Grid.Column>
            <YMaps>
              <Map
                width="100%"
                height="50em"
                state={{ center: [searchParams.get("latitude"), searchParams.get("longitude")], zoom: 12 }}
              >
                <Clusterer
                  options={{
                    preset: "islands#invertedVioletClusterIcons",
                    groupByCoordinates: false,
                  }}
                >
                  {restaurants.map((restaurant) => (
                    <Placemark
                      key={restaurant.id}
                      geometry={[restaurant.latitude, restaurant.longitude]}
                    />
                  ))}
                </Clusterer>
              </Map>
            </YMaps>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Fragment>
  );
};

export default observer(RestaurantsFilter);
