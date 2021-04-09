import React, { Fragment, useContext, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Container, Grid, Item } from "semantic-ui-react";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import OwnerReservationDetailedCard from "./OwnerReservationDetailedCard";
import LoadingComponent from "../../app/layout/LoadingComponent";

interface DetailParams {
  restaurantId: string;
}

export const OwnerReservations: React.FC<RouteComponentProps<
  DetailParams
>> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const { reservations, listReservations, loadingReservations } = rootStore.ownerStore;

  useEffect(() => {
    listReservations(match.params.restaurantId);
  }, [listReservations, match.params.restaurantId]);

  if (loadingReservations) return <LoadingComponent content="Loading restaurant..." />;

  return (
    <Fragment>
      <Container>
        <Grid>
          <Grid.Row>
            <Grid.Column>
              <Item.Group divided>
                {reservations.map((reservation) => (
                  <OwnerReservationDetailedCard key={reservation.id} reservation={reservation} restaurantId={match.params.restaurantId} />
                ))}
              </Item.Group>
            </Grid.Column>
          </Grid.Row>
        </Grid>
      </Container>
    </Fragment>
  );
};

export default observer(OwnerReservations);
