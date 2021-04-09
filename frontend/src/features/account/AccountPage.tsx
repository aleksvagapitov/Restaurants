import React, { useContext } from "react";
import { Grid, Container } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";
import { RouteComponentProps } from "react-router-dom";
import { observer } from "mobx-react-lite";
import AccountContent from "./AccountContent";

interface RouteParams {
  username: string;
}

interface IProps extends RouteComponentProps<RouteParams> {}

const AccountPage: React.FC<IProps> = ({ match }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    setActiveTab,
  } = rootStore.accountStore;

  // useEffect(() => {
  //   loadProfile(match.params.username);
  // }, [loadProfile, match]);

  // if (loadingProfile) return <LoadingComponent content="Loading profile..." />;

  return (
    <Container>
      <Grid>
        <Grid.Column width={16}>
          {/* <ProfileHeader
            profile={profile!}
            isCurrentUser={isCurrentUser}
            loading={loading}
            follow={follow}
            unfollow={unfollow}
          /> */}
          <AccountContent setActiveTab={setActiveTab} />
        </Grid.Column>
      </Grid>
    </Container>
  );
};

export default observer(AccountPage);
