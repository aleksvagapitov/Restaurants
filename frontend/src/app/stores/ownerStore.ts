import { observable, action, runInAction } from "mobx";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";
import { IRestaurant } from "../models/restaurant";
import { IReservation, IStatus } from "../models/reservation";

export default class OwnerStore {
    rootStore: RootStore;
    constructor(rootStore: RootStore){
        this.rootStore = rootStore;
    }

    @observable loadingRestaurants = false;
    @observable loadingReservations = false;
    @observable restaurants: IRestaurant[] = [];
    @observable reservations: IReservation[] = [];
    @observable reservation: IReservation | null = null;
    @observable submitting = false;
    
    @action listRestaurants = async () => {
        this.loadingRestaurants = true;
        try {
            const restaurants = await agent.OwnerRestaurants.list();
            runInAction(() => {
                this.restaurants = restaurants;
                this.loadingRestaurants = false;
            })
        } catch (error) {
            runInAction (() => {
                this.loadingRestaurants = false;
            })
        }
    }

    @action createRestaurant = async (restaurant: IRestaurant) => {
        this.submitting = true;
        try {
          await agent.OwnerRestaurants.create(restaurant);
          runInAction("create restaurant", () => {
            this.restaurants.push(restaurant);
            this.submitting = false;
          });
          history.push("/owner");
        } catch (error) {
          runInAction("create restaurant error", () => {
            this.submitting = false;
          });
          //toast.error("Problem submitting data");
          //console.log(error.response);
        }
      };

      @action listReservations = async (restaurantId: string) => {
        this.loadingReservations = true;
        try {
            const reservations = await agent.OwnerReservations.list(restaurantId);
            runInAction(() => {
                this.reservations = reservations;
                this.loadingReservations = false;
            })
        } catch (error) {
            runInAction (() => {
                this.loadingReservations = false;
            })
        }
    }

    @action editReservation = async (reservation: IReservation, restaurantId: string, status: IStatus) => {
      this.submitting = true;
        try {
          reservation.status = status;
          await agent.OwnerReservations.update(reservation, restaurantId);
          runInAction("update restaurant", () => {
            this.submitting = false;
          });
          //history.push(`/owner/${restaurantId}/reservations`);
        } catch (error) {
          runInAction("update restaurant error", () => {
            this.submitting = false;
          });
        }
    }
};    