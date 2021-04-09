import React, { useContext, useEffect, Fragment, useState } from "react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { Form as FinalForm, Field } from "react-final-form";
import { RouteComponentProps, useHistory } from "react-router-dom";
import {
  Container,
  Grid,
  Breadcrumb,
  Header,
  Divider,
  Item,
  Form,
  Button,
  Icon,
  List,
} from "semantic-ui-react";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import TextInput from "../../../app/common/form/TextInput";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import SelectInput from "../../../app/common/form/SelectInput";
import { isRequired, combineValidators } from "revalidate";
import RegisterForm from "../../user/RegisterForm";
import { LoginForm } from "../../user/LoginForm";
import NotFound from "../../../app/layout/NotFound";
import { ErrorMessage } from "../../../app/common/form/ErrorMessage";
import { IReservation } from "../../../app/models/reservation";
import { FORM_ERROR } from "final-form";

const validate = combineValidators({
  firstName: isRequired({ message: "First name is Required" }),
  lastName: isRequired({ message: "Last name is Required" }),
  phoneNumber: isRequired({ message: "Phone number is Required" }),
  email: isRequired({ message: "Email is Required" }),
});

interface DetailParams {
  restaurantId: string;
}

class ParsedParameters {
  date: string = "";
  time: string = "";
  people: string = "";

  constructor(dateTime: string, people: string){
    this.date = new Date(dateTime).toDateString();
    this.time = new Date(dateTime).toLocaleTimeString();
    this.people = people;
  }
}

export const RestaurantBooking: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  location,
}) => {
  const rootStore = useContext(RootStoreContext);
  const { openModal } = rootStore.modalStore;
  const { isLoggedIn, user } = rootStore.userStore;
  const { ReserveAction } = rootStore.reservationStore;
  const {
    restaurant,
    loadingInitial,
    loadRestaurant,
  } = rootStore.restaurantStore;
  const {reservationParameters} = rootStore.reservationStore;
  const [parameters] = useState(new ParsedParameters(reservationParameters.dateTime, reservationParameters.people));

  let history = useHistory();
  const [loading, setLoading] = useState(false);
  
  useEffect(() => {
    if (match.params.restaurantId) {
      setLoading(true);
      loadRestaurant(match.params.restaurantId)
        .finally(() => setLoading(false));
    }
  }, [loadRestaurant, match.params.restaurantId, location.search]);


  if (loadingInitial) return <LoadingComponent content="Loading restaurant..." />;

  if (!restaurant) return <NotFound />;

  return (
    <Fragment>
      <Breadcrumb>
        <Breadcrumb.Section link>Home</Breadcrumb.Section>
        <Breadcrumb.Divider icon="right chevron" />
        <Breadcrumb.Section link>Moscow</Breadcrumb.Section>
        <Breadcrumb.Divider icon="right chevron" />
        <Breadcrumb.Section active>Mayakovskaya Station</Breadcrumb.Section>
      </Breadcrumb>
      <Divider hidden />
      <Container>
        <Grid>
          <Grid.Column width={11}>
            <Header size="medium">You are almost done!</Header>
            <Item.Group>
              <Item>
                
                <Item.Image
                  size="tiny"
                  src={restaurant.image || `./assets/categoryImages/${restaurant.categories[0].category}.jpg`}
                />

                <Item.Content>
                  <Item.Header>{restaurant.name}</Item.Header>
                  <Item.Description>
                    <List horizontal>
                      <Item><Icon name="calendar"/> {parameters.date}  </Item>
                      <Item><Icon name="time"/> {parameters.time}</Item>
                      <Item><Icon name="user outline"/> {parameters.people}</Item>
                    </List>
                  </Item.Description>
                </Item.Content>
              </Item>
            </Item.Group>
            <Fragment>
              <Header>Diner details</Header>
              {!isLoggedIn && !user && (
                <Container>
                  <Button onClick={() => openModal(<LoginForm />)}>
                    Sign in
                  </Button>
                  or
                  <Button onClick={() => openModal(<RegisterForm />)}>
                    Sign up
                  </Button>
                  If you have an account
                </Container>
              )}
              <Divider hidden />
              <FinalForm
                validate={validate}
                onSubmit={(values: IReservation) =>
                  ReserveAction(values, reservationParameters, match.params.restaurantId ).catch(error => ({
                    [FORM_ERROR]: error
                  }))}
                render={({ handleSubmit, submitting,  submitError, invalid, pristine, dirtySinceLastSubmit }) => (
                  <Form onSubmit={handleSubmit} error>
                    <Form.Group widths="equal">
                      <Field
                        name="firstName"
                        placeholder="First name"
                        defaultValue={!!user ? user.displayName : ""}
                        component={TextInput}
                      />
                      <Field
                        name="lastName"
                        placeholder="Last name"
                        component={TextInput}
                      />
                    </Form.Group>
                    <Form.Group widths="equal">
                      <Field
                        name="phoneNumber"
                        placeholder="Phone number"
                        component={TextInput}
                      />
                      <Field
                        name="email"
                        placeholder="Email"
                        component={TextInput}
                      />
                    </Form.Group>
                    <Field
                      name="occasion"
                      placeholder="Select an occasion (optional)"
                      component={SelectInput}
                    />
                    <Field
                      name="specialRequest"
                      placeholder="Add a special request(optional)"
                      component={TextAreaInput}
                    />
                    {submitError && !dirtySinceLastSubmit && (
                      <ErrorMessage
                        error={submitError}
                        text="Invalid email"
                      />
                    )}
                    <Form.Group>
                      <Button
                        onClick={
                          restaurant.id
                            ? () =>
                                history.push(`/restaurants/${restaurant.id}`)
                            : () => history.push("/restaurants")
                        }
                        disabled={loading}
                        fluid
                        type="button"
                        content="Cancel"
                      />

                      <Button
                        loading={submitting}
                        disabled={loading || invalid || pristine}
                        positive
                        fluid
                        type="submit"
                        content="Complete Reservation"
                      />
                    </Form.Group>
                  </Form>
                )}
              />
            </Fragment>
          </Grid.Column>

          <Grid.Column width={5}>
            <Header>Important dining information</Header>
            <Container>
              We have a 15 minute grace period. Please call us if you are
              running later than 15 minutes after your reservation time.
            </Container>
          </Grid.Column>
        </Grid>
      </Container>
    </Fragment>
  );
};

export default observer(RestaurantBooking);
