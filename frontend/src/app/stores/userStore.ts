import { observable, computed, action, runInAction } from "mobx";
import { IUser, IUserFormValues, IRole } from "../models/user";
import agent from "../api/agent";
import { RootStore } from "./rootStore";
import { history } from "../..";

export default class UserStore {
    rootStore: RootStore;
    constructor(rootStore: RootStore){
        this.rootStore = rootStore;
    }

    @observable user: IUser | null = null;
    @observable role: IRole | null = null;

    @computed get isLoggedIn() {return !!this.user}
    @computed get isUser() {return this.role === IRole.User}
    @computed get isOwner() {return this.role === IRole.Owner}
    @computed get isAdmin() {return this.role === IRole.Admin}

    @action login = async (values: IUserFormValues) => {
        try {
            const user = await agent.User.login(values);
            runInAction(() => {
                this.user = user;
            })
            this.rootStore.commonStore.setToken(user.token);
            this.rootStore.modalStore.closeModal();
        } catch (error) {
            throw error
        }
    }

    @action register = async (values: IUserFormValues) => {
        try {
            const user = await agent.User.register(values);
            this.rootStore.commonStore.setToken(user.token);
            this.rootStore.modalStore.closeModal();
            // history.push('/');
        } catch (error) {
            throw error
        }
    } 

    @action getUser = async () => {
        try {
            const user = await agent.User.current();
            runInAction(() => {
                this.user = user
            })
        } catch (error) {
        }
    }

    @action getRole = async () => {
        try {
            const user = await agent.User.current();
            runInAction(() => {
                let jwtData = user.token.split('.')[1];
                let decodedJwtJsonData = window.atob(jwtData);
                let decodedJwtData = JSON.parse(decodedJwtJsonData)
                this.role = decodedJwtData.role
            })
        } catch (error) {
        }
    }

    @action logout = () => {
        this.rootStore.commonStore.setToken(null);
        this.user = null;
        this.role = null;
        history.push('/');
    }
}