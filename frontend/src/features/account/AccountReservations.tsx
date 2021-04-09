import React, { useEffect, useContext } from 'react';
import { observer } from 'mobx-react-lite';
import { Tab, Grid, Header, Card, Image, TabProps } from 'semantic-ui-react';
import { Link } from 'react-router-dom';
import { IUserReservation } from '../../app/models/profile';
import { format } from 'date-fns';
import { RootStoreContext } from '../../app/stores/rootStore';
import { IStatus } from '../../app/models/reservation';

const panes = [
  { menuItem: 'Upcoming Reservations', pane: { key: 'upcomingReservations' } },
  { menuItem: 'Past Reservations', pane: { key: 'pastReservations' } },
];

const AccountReservations = () => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadUserReservations,
    loadingReservations,
    userReservations
  } = rootStore.accountStore!;

  const {
    user
  } = rootStore.userStore!;

  useEffect(() => {
    loadUserReservations(user!.username);
  }, [loadUserReservations, user]);

  const handleTabChange = (
    e: React.MouseEvent<HTMLDivElement, MouseEvent>,
    data: TabProps
  ) => {
    let predicate;
    switch (data.activeIndex) {
      case 1:
        predicate = 'past';
        break;
      default:
        predicate = 'future';
        break;
    }
    loadUserReservations(predicate);
  };

  return (
    <Tab.Pane loading={loadingReservations}>
      <Grid>
        <Grid.Column width={16}>
          <Header floated='left' icon='calendar' content={'Reservations'} />
        </Grid.Column>
        <Grid.Column width={16}>
          <Tab
            panes={panes}
            menu={{ secondary: true, pointing: true }}
            onTabChange={(e, data) => handleTabChange(e, data)}
          />
          <br />
          <Card.Group itemsPerRow={4}>
            {userReservations.map((reservation: IUserReservation) => (
              <Card
                as={Link}
                to={`/restaurants/${reservation.restaurantName}`}
                key={reservation.id}
              >
                <Image
                  src={`./assets/categoryImages/sushi.jpg`}
                  style={{ minHeight: 100, objectFit: 'cover' }}
                />
                <Card.Content>
                  <Card.Header textAlign='center'>{reservation.restaurantName}</Card.Header>
                  <Card.Meta textAlign='center'>
                    <div>{format(new Date(reservation.dateTime), 'do LLL')}</div>
                    <div>{format(new Date(reservation.dateTime), 'h:mm a')}</div>
                    {reservation.status === IStatus.Pending ? (
                      <Card.Content>Pending</Card.Content>
                    ) : (
                    reservation.status === IStatus.Cancelled && (
                      <Card.Content>Cancelled</Card.Content>
                    )
                  )}
                  </Card.Meta>
                </Card.Content>
              </Card>
            ))}
          </Card.Group>
        </Grid.Column>
      </Grid>
    </Tab.Pane>
  );
};

export default observer(AccountReservations);
