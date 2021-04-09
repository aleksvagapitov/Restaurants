import React, { useContext } from 'react'
import { observer } from 'mobx-react-lite'
import {Form as FinalForm, Field} from 'react-final-form'
import { Segment, Container, Header, Divider, Form, Button, Grid } from 'semantic-ui-react'
import { useHistory } from 'react-router-dom'
import DateInput from '../../../app/common/form/DateInput'
import SelectInput from '../../../app/common/form/SelectInput'
import { RootStoreContext } from '../../../app/stores/rootStore'
import { combineDateAndTime } from '../../../app/common/util/util'

interface IProps {
    id: string;
  }

const peopleOptions = [
  {
    key: "people 1",
    text: "1 person",
    value: "1",
  },
  {
    key: "people 2",
    text: "2 people",
    value: "2",
  },
];

export const RestaurantDetailedSidebarBooking: React.FC<IProps> = ({ id }) => {
  const rootStore = useContext(RootStoreContext);
  const {searchParams} = rootStore.restaurantStore;
  const {setReservationParameters} = rootStore.reservationStore;
  
  let history = useHistory();

  const handleFinalFormSubmit = (values: any) => {
    const { date, time, people } = values;
    const dateTime = combineDateAndTime(date, time);
    setReservationParameters(dateTime, people);
    history.push(
      `/book/${id}?dateTime=${dateTime}&people=${people}`
    );
  };


  return (
    <Segment>
      <Header textAlign="center">Make a Reservation</Header>
      <Divider />
      <FinalForm
        onSubmit={handleFinalFormSubmit}
        render={({ handleSubmit, invalid, pristine }) => (
          <Form onSubmit={handleSubmit}>
            <Grid columns="equal">
              <Grid.Row>
                <Grid.Column>
                  <Container>Party size</Container>
                  <Field
                    component={SelectInput}
                    defaultValue={searchParams.get("people")}
                    fluid
                    name="people"
                    selection
                    options={peopleOptions}
                  />
                </Grid.Column>
              </Grid.Row>
              <Grid.Row>
                <Grid.Column>
                  <Container>Date</Container>
                  <Field
                    component={DateInput}
                    defaultValue={searchParams.get("searchDate")}
                    name="date"
                    date={true}
                  />
                </Grid.Column>
                <Grid.Column>
                  <Container>Time</Container>
                  <Field
                    component={DateInput}
                    defaultValue={searchParams.get("searchDate")}
                    name="time"
                    time={true}
                  />
                </Grid.Column>
              </Grid.Row>
              <Grid.Row>
                <Grid.Column>
                  <Button fluid positive type="submit" content="Submit" />
                </Grid.Column>
              </Grid.Row>
            </Grid>
          </Form>
        )}
      ></FinalForm>
    </Segment>
  );
};

export default observer(RestaurantDetailedSidebarBooking);