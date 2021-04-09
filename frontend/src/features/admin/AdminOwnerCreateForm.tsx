import React, { useContext } from "react";
import { Form as FinalForm, Field } from "react-final-form";
import {
  Form,
  Button,
  Segment,
  Container,
  Grid,
} from "semantic-ui-react";
import TextInput from "../../app/common/form/TextInput";
import { RootStoreContext } from "../../app/stores/rootStore";
import { IUserFormValues } from "../../app/models/user";
import { FORM_ERROR } from "final-form";
import { combineValidators, isRequired } from "revalidate";
import { ErrorMessage } from "../../app/common/form/ErrorMessage";
import { observer } from "mobx-react-lite";

const validate = combineValidators({
  email: isRequired("email"),
});

export const AdminOwnerCreateForm = () => {
  const rootStore = useContext(RootStoreContext);
  const { create } = rootStore.adminStore;
  return (
    <Container>
      <Grid>
        <Grid.Column width={10}>
          <Segment clearing>
            <FinalForm
              onSubmit={(values: IUserFormValues) =>
                create(values).catch((error) => ({
                  [FORM_ERROR]: error,
                }))
              }
              validate={validate}
              render={({
                handleSubmit,
                submitting,
                submitError,
                invalid,
                pristine,
                dirtySinceLastSubmit,
              }) => (
                <Form onSubmit={handleSubmit} error>
                  <Field
                    name="email"
                    component={TextInput}
                    placeholder="Email"
                  />
                  {submitError && !dirtySinceLastSubmit && (
                    <ErrorMessage error={submitError} />
                  )}
                  <Button
                    disabled={(invalid && !dirtySinceLastSubmit) || pristine}
                    loading={submitting}
                    color="teal"
                    content="Register"
                    fluid
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

export default observer(AdminOwnerCreateForm);
