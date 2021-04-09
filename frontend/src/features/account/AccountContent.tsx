import React from "react";
import { Tab } from "semantic-ui-react";
import AccountReservations from "./AccountReservations";

interface IProps {
  setActiveTab: (activeIndex: any) => void;
}

const panes = [
  { menuItem: "Reservations", render: () => <AccountReservations /> },
  
];

const AccountContent: React.FC<IProps> = ({ setActiveTab }) => {
  return (
    <Tab
      menu={{ fluid: true, vertical: true }}
      menuPosition="right"
      panes={panes}
      onTabChange={(e, data) => setActiveTab(data.activeIndex)}
    />
  );
};

export default AccountContent;
