import React, { useContext, useState } from "react";
import { observer } from "mobx-react-lite";
import {
  Header,
  Container,
  Button,
  Form,
} from "semantic-ui-react";
import { Form as FinalForm, Field } from 'react-final-form';
import { RouteComponentProps } from "react-router-dom";
import DateInput from "../../../app/common/form/DateInput";
import SelectInput from "../../../app/common/form/SelectInput";
import TextInput from "../../../app/common/form/TextInput";
import { RootStoreContext } from "../../../app/stores/rootStore";
import { combineDateAndTime } from "../../../app/common/util/util";

const peopleOptions = [
  {
    key: 'people 1',
    text: '1 person',
    value: '1',
  },
  {
    key: 'people 2',
    text: '2 people',
    value: '2',
  }
]
export const SearchForm: React.FC<RouteComponentProps> = ({history, match, location}) => {
  const rootStore = useContext(RootStoreContext);
  const { searchParams, setPredicate} = rootStore.restaurantStore;

  const [dateTime] = useState(new Date(searchParams.get("searchDate")));

  const handleFinalFormSubmit = (values: any) => {
     const { date, time, people, term } = values;
     const dateTime = combineDateAndTime(date, time);
     setPredicate("searchDate", dateTime);
     setPredicate("people", people);
     setPredicate("term", term || "")

     history.push(`/search`);
  };
  

  return (
    <Container>
      <Container>
        <Header as="h1" textAlign="center">
          Find your table for any occasion
        </Header>
        <FinalForm
          onSubmit={handleFinalFormSubmit}
          render={({ handleSubmit, invalid, pristine }) => (
            <Form onSubmit={handleSubmit}>
              <Form.Group>
                <Field
                  component={DateInput}
                  // defaultValue={searchValues.dateTime}
                  defaultValue={dateTime}
                  name="date"
                  date={true}
                  width={4}
                />
                <Field
                  component={DateInput}
                  // defaultValue={searchValues.dateTime}
                  defaultValue={dateTime}
                  timeIntervals={30}
                  name="time"
                  time={true}
                  width={4}
                />
                <Field
                  component={SelectInput}
                  placeholder="Select How Many People"
                  // defaultValue={searchValues.people}
                  defaultValue={searchParams.get("people") || "2"}
                  fluid
                  name="people"
                  selection
                  options={peopleOptions}
                  width={4}
                />
                <Field 
                  // defaultValue={searchValues.term} 
                  defaultValue={searchParams.get("term") || ""} 
                  component={TextInput} name="term" width={8} />
                <Button
                  floated="right"
                  positive
                  type="submit"
                  content="Submit"
                />
              </Form.Group>
            </Form>
          )}
        ></FinalForm>
      </Container>
    </Container>
  );
};

export default observer(SearchForm);
