import React from "react";
import { Grid, Container } from "semantic-ui-react";
import { RouteComponentProps } from "react-router-dom";
import { observer } from "mobx-react-lite";
import AdminTabs from "./AdminTabs";

interface IProps extends RouteComponentProps {}

const AdminPage: React.FC<IProps> = () => {
  return (
    <Container>
        <Grid>
          <Grid.Column width={16}>
            <AdminTabs />
          </Grid.Column>
        </Grid>
    </Container>
  );
};

export default observer(AdminPage);
