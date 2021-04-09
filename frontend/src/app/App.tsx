import React, { Fragment, useContext, useEffect, useState } from "react";
import NavBar from "../features/nav/NavBar";
import { observer } from "mobx-react-lite";
import {
  Route,
  withRouter,
  RouteComponentProps,
  Switch,
} from "react-router-dom";
import HomePage from "../features/home/HomePage";
import NotFound from "./layout/NotFound";
import { ToastContainer } from "react-toastify";
import { RootStoreContext } from "./stores/rootStore";
import LoadingComponent from "./layout/LoadingComponent";
import ModalContainer from "./common/modals/ModalContainer";
import ProfilePage from "../features/profiles/ProfilePage";
import RestaurantsDashboard from "../features/restaurants/dashboard/RestaurantsDashboard";
import RestaurantDetails from "../features/restaurants/details/RestaurantDetails";
import RestaurantBooking from "../features/restaurants/reservations/RestaurantBooking";
import { OwnerRoute } from "./layout/OwnerRoute";
import OwnerPage from "../features/owner/OwnerPage";
import OwnerRestaurantForm from "../features/owner/OwnerRestaurantOwnerRestaurantForm";
import OwnerReservations from "../features/owner/OwnerReservations";
import AccountPage from "../features/account/AccountPage";
import AdminPage from "../features/admin/AdminPage";
import UserRoute from "./layout/UserRoute";
import AdminRoute from "./layout/AdminRoute";
import AdminOwnerCreateForm from "../features/admin/AdminOwnerCreateForm";
import OwnerRestaurantPage from "../features/owner/restaurants/OwnerRestaurantPage";
import { create } from "mobx-persist";
import { combineDateAndTime } from "./common/util/util";

const hydrate = create({
  jsonify: true,
});

const App: React.FC<RouteComponentProps> = ({ location }) => {
  const rootStore = useContext(RootStoreContext);
  const { setAppLoaded, token, appLoaded } = rootStore.commonStore;
  const { getUser, getRole } = rootStore.userStore;
  // const { setSearchParameters, searchParameters} = rootStore.restaurantStore;
  const { searchParams, setPredicate } = rootStore.restaurantStore;
  const [hydrated, setHydrated] = useState(false);


  useEffect(() => {
    if (!hydrated)
      hydrate("restaurantStore", rootStore.restaurantStore).finally(() =>
        setHydrated(true)
      );
    else {
      if (searchParams.size <= 3) {
        setPredicate("searchDate", combineDateAndTime(new Date(), new Date()));
        setPredicate("people", "2");
        setPredicate("term", "");
        setPredicate("latitude", 55.7558);
        setPredicate("longitude", 37.6173);
      } else {
        var diff = new Date().getTime() - new Date(searchParams.get("searchDate")).getTime();
        if (Math.floor(diff / 60000) > 30){
          setPredicate("searchDate", combineDateAndTime(new Date(), new Date()));
          setPredicate("people", "2");
          setPredicate("term", "");
        } else
          setPredicate("searchDate", new Date(searchParams.get("searchDate")));
        setPredicate("latitude", 55.7558);
        setPredicate("longitude", 37.6173);
      }
    }
  }, [hydrated, rootStore.restaurantStore, setPredicate, searchParams]);

  useEffect(() => {
    if (token) {
      getUser()
        .then(() => getRole())
        .finally(() => setAppLoaded());
    } else {
      setAppLoaded();
    }
  }, [getUser, getRole, setAppLoaded, token]);

  if (!appLoaded) return <LoadingComponent content="Loading app..." />;

  return (
    <Fragment>
      <ModalContainer />
      <ToastContainer position="bottom-right" />
      <NavBar />
      <Route exact path="/" component={HomePage} />
      <Route
        path={"/(.+)"}
        render={() => (
          <Fragment>
            <Fragment>
              <Switch>
                <UserRoute exact path="/account" component={AccountPage} />
                <UserRoute path="/profile/:username" component={ProfilePage} />
                <OwnerRoute
                  exact
                  path={["/owner/:restaurantId"]}
                  component={OwnerRestaurantPage}
                />
                <OwnerRoute
                  path={[
                    "/owner/:restaurantId/edit",
                    "/owner/restaurant/create",
                  ]}
                  component={OwnerRestaurantForm}
                />
                <AdminRoute
                  path={["/admin/:userId/edit", "/admin/owner/create"]}
                  component={AdminOwnerCreateForm}
                />
                <OwnerRoute
                  path={"/owner/:restaurantId/reservations"}
                  component={OwnerReservations}
                />
                <OwnerRoute path="/owner" component={OwnerPage} />
                <AdminRoute path="/admin" component={AdminPage} />
                <Route
                  exact
                  path={"/restaurants/:id"}
                  component={RestaurantDetails}
                />
                <Route path={"/search"} component={RestaurantsDashboard} />
                <Route
                  path={"/book/:restaurantId"}
                  component={RestaurantBooking}
                />
                <Route component={NotFound} />
              </Switch>
            </Fragment>
          </Fragment>
        )}
      />
    </Fragment>
  );
};

export default withRouter(observer(App));
