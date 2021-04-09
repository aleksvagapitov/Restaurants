import React, { useContext, SyntheticEvent, useState } from "react";
import { Dropdown } from "semantic-ui-react";
import { RootStoreContext } from "../../app/stores/rootStore";

export interface ICity {
  text: string;
  value: string;
}

export const CityDropdown = () => {
  const rootStore = useContext(RootStoreContext);
  const { setPredicate } = rootStore.restaurantStore;

  const [cityName, setCityName] = useState("Moscow");

  const changeLocation = (event: SyntheticEvent, data: object) => {
    const parsedData = data as ICity;
    setPredicate("latitude", parsedData.value.split(" ")[0]);
    setPredicate("longitude", parsedData.value.split(" ")[1])
    setCityName(parsedData.text);
  };

  return (
    <Dropdown
      text={cityName}
      icon="location arrow"
      floating
      labeled
      button
      className="icon"
    >
      <Dropdown.Menu>
        <Dropdown.Item
          text="Moscow"
          value="55.7558 37.6173"
          onClick={changeLocation}
        />
        <Dropdown.Item
          text="Saint-Petersburg"
          value="59.9311 30.3609"
          onClick={changeLocation}
        />
      </Dropdown.Menu>
    </Dropdown>
  );
};
