import React, { useContext, useState, useEffect } from "react";
import {
  Button,
  Grid,
  Segment,
  Form,
  Container,
} from "semantic-ui-react";
import { Form as FinalForm } from "react-final-form";
import { RouteComponentProps } from "react-router-dom";
import { RootStoreContext } from "../../app/stores/rootStore";
import { Field } from "react-final-form";
import TextInput from "../../app/common/form/TextInput";
import { isRequired, combineValidators } from "revalidate";
import {
  RestaurantFormValues,
  IRestaurant
} from "../../app/models/restaurant";
import LoadingComponent from "../../app/layout/LoadingComponent";
import MultiSelectInput from "../../app/common/form/MultiSelectInput";
import { observer } from "mobx-react-lite";
import TextAreaInput from "../../app/common/form/TextAreaInput";

const validate = combineValidators({
  name: isRequired({ message: "The restaurant name is required" }),
  city: isRequired("City"),
  address: isRequired("Address"),
  postalCode: isRequired("Postal Code"),
  phone: isRequired("Phone Number"),
  categories: isRequired("Categories"),
  description: isRequired("Description")
});

interface DetailParams {
  restaurantId: string;
}

const OwnerRestaurantForm: React.FC<RouteComponentProps<DetailParams>> = ({
  match,
  history,
}) => {
  const rootStore = useContext(RootStoreContext);
  const {
    createRestaurant,
    submitting,
  } = rootStore.ownerStore;

  const {editRestaurant} = rootStore.ownerRestaurant;

  const {loadRestaurant} = rootStore.ownerRestaurant;

  const { categories, loadCategories } = rootStore.restaurantStore;
  const [restaurant, setRestaurant] = useState(new RestaurantFormValues());
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (match.params.restaurantId) {
      setLoading(true);
      loadRestaurant(match.params.restaurantId)
        .then((restaurant: IRestaurant) => {
          setRestaurant(new RestaurantFormValues(restaurant));
        })
        .finally(() => setLoading(false));
    }
  }, [loadRestaurant, match.params.restaurantId]);

  useEffect(() => {
    loadCategories();
  }, [loadCategories]);

  const handleFinalFormSubmit = (values: any) => {
    const { ...restaurant } = values;
    if (!restaurant.id) {
      createRestaurant(restaurant);
    } else {
      editRestaurant(restaurant);
    }
  };

  if (loading) return <LoadingComponent content="Loading restaurant..." />;

  // if (!restaurant) {

  // }

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
                <Form onSubmit={handleSubmit} loading={loading}>
                  <Field
                    name="name"
                    placeholder="Title"
                    value={restaurant.name}
                    component={TextInput}
                  />
                  <Field
                    component={TextInput}
                    name="city"
                    placeholder="City"
                    value={restaurant.city}
                  />
                  <Field
                    component={TextInput}
                    name="address"
                    placeholder="Address"
                    value={restaurant.address}
                  />
                  <Field
                    component={TextInput}
                    name="postalCode"
                    placeholder="Postal Code"
                    value={restaurant.postalCode}
                  />
                  <Field
                    component={TextInput}
                    name="phone"
                    placeholder="Phone Number"
                    value={restaurant.phone}
                  />
                  <Field
                    component={TextAreaInput}
                    name="description"
                    placeholder="Description"
                    value={restaurant.description}
                  />
                  {!restaurant.id && (
                    <Field
                      component={MultiSelectInput}
                      placeholder="Categories"
                      name="categories"
                      fluid
                      multiple
                      search
                      selection
                      options={categories}
                    />
                  )}
                  <Button
                    loading={submitting}
                    disabled={loading || invalid || pristine}
                    floated="right"
                    positive
                    type="submit"
                    content="Submit"
                  />
                  <Button
                    onClick={
                      restaurant.id
                        ? () => history.push(`/owner`)
                        : () => history.push("/owner")
                    }
                    disabled={loading}
                    floated="right"
                    type="button"
                    content="Cancel"
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

export default observer(OwnerRestaurantForm);
