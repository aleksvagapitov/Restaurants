import React, { useContext, useEffect } from "react";
import { Grid, Container } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import OwnerRestaurantTabs from "./OwnerRestaurantTabs";
import { RootStoreContext } from "../../../app/stores/rootStore";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { RouteComponentProps } from "react-router-dom";

interface RouteParams {
  restaurantId: string;
}

const ProfilePage: React.FC<RouteComponentProps<RouteParams>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadRestaurant,
    loadingInitial,
    setActiveTab
  } = rootStore.ownerRestaurant;

  useEffect(() => {
    console.log(match.params.restaurantId)
    loadRestaurant(match.params.restaurantId);
  }, [loadRestaurant, match]);

  if (loadingInitial) return <LoadingComponent content="Loading profile..." />;

  return (
    <Container>
      <Grid>
        <Grid.Column width={16}>
          <OwnerRestaurantTabs setActiveTab={setActiveTab} />
        </Grid.Column>
      </Grid>
    </Container>
  );
};

export default observer(ProfilePage);
