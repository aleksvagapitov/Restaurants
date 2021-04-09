import React from "react";
import { Tab } from "semantic-ui-react";
import OwnerRestaurantEditForm from "./OwnerRestaurantEditForm";
import OwnerRestaurantPhotos from "./OwnerRestaurantPhotos";

interface IProps {
    setActiveTab: (activeIndex: any) => void;
  }

  const panes = [
    { menuItem: "Edit", render: () => <OwnerRestaurantEditForm /> },
    { menuItem: "Photos", render: () => <OwnerRestaurantPhotos /> },
    { menuItem: "Reviews", render: () => "Not Made Yet" },
    { menuItem: "Schedule", render: () => "Schedule" },
  ];

const OwnerRestaurantTabs: React.FC<IProps> = ({setActiveTab}) => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(e, data) => setActiveTab(data.activeIndex)}
    />
  );
};

export default OwnerRestaurantTabs;
