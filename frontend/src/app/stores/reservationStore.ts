import { RootStore } from "./rootStore";
import { observable, action, runInAction } from "mobx";
import { history } from "../..";
import agent from "../api/agent";
import { IReservation } from "../models/reservation";
import uuid from "uuid";
import { combineDateAndTime } from "../common/util/util";
import { persist } from "mobx-persist";

class ReservationParameters {
  @persist @observable dateTime: string = new Date().toString();
  @persist @observable people: string = "1";
}

export default class ReservationStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable reserving = false;
  @persist("object", ReservationParameters) @observable reservationParameters = new ReservationParameters();

  @action setReservationParameters = async (dateTime: Date, people: number) => {
    this.reservationParameters.dateTime = dateTime.toString();
    this.reservationParameters.people = people.toString();
}

  @action ReserveAction = async (values: any, reservationParameters: ReservationParameters, restaurantId: string) => {
    const dateAndTime = combineDateAndTime(new Date(reservationParameters.dateTime), new Date(reservationParameters.dateTime));
    const { ...reservation } = values;
    if (restaurantId) {
      let newGuestBooking = {
        ...reservation,
        id: uuid(),
        dateAndTime: dateAndTime, 
        people: parseInt(reservationParameters.people), 
        restaurantId: restaurantId,
      };
      this.Reserve(newGuestBooking);
    }
  };

  @action Reserve = async (reservation: IReservation) => {
    if (this.rootStore.commonStore.token && this.rootStore.commonStore.token !== "null"){
      this.rootStore.userStore.getUser().finally(() => {
        this.ReserveAsUser(reservation);
      })
    } else {
      this.ReserveAsGuest(reservation);
    }
  };

  @action ReserveAsUser = async (reservation: IReservation) => {
    this.reserving = true;
    try {
      await agent.Reservations.reserveAsUser(reservation);
      runInAction("create reservation", () => {
        this.reserving = false;
      });
      history.push("/");
    } catch (error) {
      runInAction("create reservation error", () => {
        this.reserving = false;
      });
      throw error;
    }
  };

  @action ReserveAsGuest = async (reservation: IReservation) => {
    this.reserving = true;
    try {
      await agent.Reservations.reserveAsGuest(reservation);
      runInAction("create reservation", () => {
        this.reserving = false;
      });
      history.push("/");
    } catch (error) {
      runInAction("create reservation error", () => {
        this.reserving = false;
      });
      throw error;
    }
  };
}
