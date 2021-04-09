import { RootStore } from "./rootStore";
import { observable, action, runInAction } from "mobx";
import { IUserReservation } from "../models/profile";
import agent from "../api/agent";
import { toast } from "react-toastify";

export default class AccountStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable activeTab: number = 0;
  @observable userReservations: IUserReservation[] = [];
  @observable loadingReservations = false;

  @action loadUserReservations = async (predicate?: string) =>  {
    this.loadingReservations = true;
    try {
      const reservations = await agent.Profiles.listReservations(predicate!);
      runInAction(() => {
        this.userReservations = reservations;
        this.loadingReservations = false;
      })
    } catch (error) {
      toast.error('Problem loading reservations')
      runInAction(() => {
        this.loadingReservations = false;
      })
    }
  }

  @action setActiveTab = (activeIndex: number) => {
    this.activeTab = activeIndex;
  }
}
