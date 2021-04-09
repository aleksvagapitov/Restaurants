import React, { useContext, useState } from "react";
import {
  Button,
  Grid,
  Segment,
  Form,
  Container,
} from "semantic-ui-react";
import { Form as FinalForm, Field } from "react-final-form";
import { combineValidators, isRequired } from "revalidate";
import { RootStoreContext } from "../../../app/stores/rootStore";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import TextInput from "../../../app/common/form/TextInput";
import TextAreaInput from "../../../app/common/form/TextAreaInput";
import { observer } from "mobx-react-lite";


const validate = combineValidators({
  name: isRequired({ message: "The restaurant name is required" }),
  city: isRequired("City"),
  address: isRequired("Address"),
  postalCode: isRequired("Postal Code"),
  phone: isRequired("Phone Number"),
  categories: isRequired("Categories"),
  description: isRequired("Description")
});
const OwnerRestaurantEditForm = () => {
  const rootStore = useContext(RootStoreContext);

  const {restaurant, editRestaurant, submitting, loadingInitial} = rootStore.ownerRestaurant;

  const [editMode, setEditMode] = useState(false);

  const handleFinalFormSubmit = (values: any) => {
    const { ...restaurant } = values;
    if (restaurant.id) {
      editRestaurant(restaurant);
    }
  };

  if (loadingInitial) return <LoadingComponent content="Loading profile..." />;

  return (
    <Container>
      <Grid>
        <Grid.Column width={10}>
          <Segment clearing>
            <FinalForm
              validate={validate}
              initialValues={restaurant}
              onSubmit={handleFinalFormSubmit}
              render={({ handleSubmit, invalid, pristine }) => (
                <Form onSubmit={handleSubmit}>
                  <Field
                    name="name"
                    placeholder="Title"
                    value={" " || restaurant!.name}
                    component={TextInput}
                  />
                  <Field
                    component={TextInput}
                    name="city"
                    placeholder="City"
                    value={" " || restaurant!.city}
                  />
                  <Field
                    component={TextInput}
                    name="address"
                    placeholder="Address"
                    value={" " || restaurant!.address}
                  />
                  <Field
                    component={TextInput}
                    name="postalCode"
                    placeholder="Postal Code"
                    value={" " || restaurant!.postalCode}
                  />
                  <Field
                    component={TextInput}
                    name="phone"
                    placeholder="Phone Number"
                    value={" " || restaurant!.phone}
                  />
                  <Field
                    component={TextAreaInput}
                    name="description"
                    placeholder="Description"
                    value={" " || restaurant!.description}
                  />
                  <Button
                    loading={submitting}
                    disabled={invalid || pristine}
                    floated="right"
                    positive
                    type="submit"
                    content="Submit"
                  />
                  <Button
                    floated='right'
                    basic
                    content={editMode ? 'Cancel' : 'Edit Profile'}
                    onClick={() => setEditMode(!editMode)}
                  />
                </Form>
              )}
            />
          </Segment>
        </Grid.Column>
      </Grid>
    </Container>
  );
};

export default observer(OwnerRestaurantEditForm);
