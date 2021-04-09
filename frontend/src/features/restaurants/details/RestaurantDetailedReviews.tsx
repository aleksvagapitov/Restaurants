import React, { useEffect, useContext, useState } from "react";
import { observer } from "mobx-react-lite";
import { RootStoreContext } from "../../../app/stores/rootStore";
import RestaurantDetailedReviewsList from "./RestaurantDetailedReviewsList";
import RestaurantDetailedReviewsSummary from "./RestaurantDetailedReviewsSummary";
import { Container, Pagination, PaginationProps } from "semantic-ui-react";
import { IRestaurant } from "../../../app/models/restaurant";

interface IProps {
  restaurant: IRestaurant;
}

export const RestaurantDetailedReviews: React.FC<IProps> = ({ restaurant }) => {
  const rootStore = useContext(RootStoreContext);
  const {
    loadReviews,
    totalReviewPages,
  } = rootStore.restaurantStore;

  const [activePage, setActivePage] = useState(1);
  const pageChange = (
    e: React.MouseEvent<HTMLAnchorElement>,
    pageInfo: PaginationProps
  ) => {
    const activePage = pageInfo.activePage ? +pageInfo.activePage : 1;
    setActivePage(activePage);
  };

  useEffect(() => {
    loadReviews(restaurant.id, activePage - 1);
  }, [loadReviews, restaurant.id, activePage]);

  return (
    <Container>
      <Container>
        <RestaurantDetailedReviewsSummary restaurant={restaurant} />
        
        <RestaurantDetailedReviewsList />
        <Pagination
          activePage={activePage}
          onPageChange={pageChange}
          totalPages={totalReviewPages}
        />
      </Container>
    </Container>
  );
};

export default observer(RestaurantDetailedReviews);
