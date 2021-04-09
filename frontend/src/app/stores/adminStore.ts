import { RootStore } from "./rootStore";
import { action, observable } from "mobx";
import { history } from "../..";
import { IUserFormValues, IUser } from "../models/user";
import agent from "../api/agent";

export default class AdminStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable loadingOwners = false;
  @observable loadingInitial = false;
  @observable owners: IUser[] = [];

  @action create = async (values: IUserFormValues) => {
    try {
        await agent.AdminOwners.create(values);
        history.push('/admin');
    } catch (error) {
        throw error
    }
} 
}
