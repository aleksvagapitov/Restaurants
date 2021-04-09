import React, { useContext } from "react";
import { observer } from "mobx-react-lite";
import { Segment, Item, Image, Button, Label } from "semantic-ui-react";
import { IReservation, IStatus } from "../../app/models/reservation";
import { RootStoreContext } from "../../app/stores/rootStore";

export const OwnerReservationDetailedCard: React.FC<{
  reservation: IReservation;
  restaurantId: string;
}> = ({ reservation, restaurantId }) => {
  const rootStore = useContext(RootStoreContext);
  const { editReservation } = rootStore.ownerStore;

  const handleApprove = () => {
    editReservation(reservation, restaurantId, IStatus.Approved);
  }

  const handleReject = () => {
    editReservation(reservation, restaurantId, IStatus.Cancelled);
  }

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Image size="small" src="./assets/user.png" />
            <Item.Content>
              <Item.Header>{reservation.firstName}</Item.Header>
              <Item.Meta>{reservation.lastName}</Item.Meta>
              {reservation.status === IStatus.Pending ? (
                <Label color="grey">Pending</Label>
              ) : (
                reservation.status === IStatus.Cancelled && (
                  <Item.Content>Cancelled</Item.Content>
                )
              )}
            </Item.Content>
              {reservation.status === IStatus.Pending && (
                <Item.Extra>
                <Button
                primary
                floated="right"
                onClick={handleApprove}
              >
                Approve
              </Button>
              <Button
                secondary
                floated="right"
                onClick={handleReject}
              >
                Reject
              </Button>
              </Item.Extra>
              )}
              
          </Item>
        </Item.Group>
      </Segment>
    </Segment.Group>
  );
};

export default observer(OwnerReservationDetailedCard);
