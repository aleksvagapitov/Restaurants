import UserStore from "./userStore";
import { createContext } from "react";
import { configure } from "mobx";
import CommonStore from "./commonStore";
import ModalStore from "./modalStore";
import ProfileStore from "./profileStore";
import RestaurantStore from "./restaurantStore";
import ReservationStore from "./reservationStore";
import OwnerStore from "./ownerStore";
import AccountStore from "./accountStore";
import AdminStore from "./adminStore";
import { create } from 'mobx-persist'
import OwnerRestaurant from "./ownerRestaurantStore";

configure({ enforceActions: 'always' });

const hydrate = create({
    jsonify: true
})

export class RootStore {
    userStore = new UserStore(this);
    accountStore = new AccountStore(this);
    commonStore = new CommonStore(this);
    modalStore = new ModalStore(this);
    profileStore = new ProfileStore(this);
    restaurantStore = new RestaurantStore(this);
    reservationStore = new ReservationStore(this);
    ownerRestaurant = new OwnerRestaurant(this);
    ownerStore = new OwnerStore(this);
    adminStore = new AdminStore(this);

    constructor() {
        hydrate("restaurantStore", this.restaurantStore);
        hydrate("reservationStore", this.reservationStore);
    }
}


export const RootStoreContext = createContext(new RootStore());