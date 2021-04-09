import React, { useContext, useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Dropdown, Container } from "semantic-ui-react";
import { RootStoreContext } from "../../../app/stores/rootStore";

const RestaurantFilters = () => {
  const rootStore = useContext(RootStoreContext);
  const { categories, setPredicateFromArray, loadCategories } = rootStore.restaurantStore;

  useEffect(() => {
    setPredicateFromArray("categories", [])
    loadCategories();
  }, [loadCategories, setPredicateFromArray]);

  return (
    <Container>
      <Container>
      <Dropdown 
            onChange={(e, data) => setPredicateFromArray("categories", data.value)}
            multiple
            search
            selection
            placeholder="Categories"
            options={categories}
        />
    </Container>
      </Container>
  );
};

export default observer(RestaurantFilters);
