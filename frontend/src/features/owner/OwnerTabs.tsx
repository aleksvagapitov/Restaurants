import React from "react";
import { Tab } from "semantic-ui-react";
import OwnerRestaurants from "./OwnerRestaurants";

const panes = [
  { menuItem: "Restaurants", render: () => <OwnerRestaurants /> }
];

const OwnerTabs: React.FC = () => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
    />
  );
};

export default OwnerTabs;
