import React from "react";
import { Grid, Container } from "semantic-ui-react";
import { RouteComponentProps } from "react-router-dom";
import { observer } from "mobx-react-lite";
import OwnerTabs from "./OwnerTabs";

interface IProps extends RouteComponentProps {}

const OwnerPage: React.FC<IProps> = () => {
  return (
    <Container>
        <Grid>
          <Grid.Column width={16}>
            <OwnerTabs />
          </Grid.Column>
        </Grid>
    </Container>
  );
};

export default observer(OwnerPage);
