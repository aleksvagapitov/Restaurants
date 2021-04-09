import React from "react";
import { Tab } from "semantic-ui-react";
import { AdminOwners } from "./AdminOwners";
// import OwnerRestaurants from "./OwnerRestaurants";

const panes = [
  { menuItem: "Users" },
  { menuItem: "Owners", render: () => <AdminOwners /> }
];

const AdminTabs: React.FC = () => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
    />
  );
};

export default AdminTabs;
