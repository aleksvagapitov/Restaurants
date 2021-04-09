import { RootStore } from "./rootStore";
import { observable, reaction, action, runInAction } from "mobx";
import agent from "../api/agent";
import { history } from "../..";
import { IRestaurant, IPhoto } from "../models/restaurant";
import { toast } from "react-toastify";

export default class OwnerRestaurant {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;

    reaction(
      () => this.activeTab,
      (activeTab) => {
        if (activeTab === 3 || activeTab === 4) {
          //const predicate = activeTab === 3 ? "followers" : "following";
          //this.loadFollowings(predicate);
        } else {
          //this.followings = [];
        }
      }
    );
  }

  @observable activeTab: number = 0;
  @observable loadingInitial = false;
  @observable restaurant: IRestaurant | null = null;
  @observable submitting = false;
  @observable uploadingPhoto = false;
  @observable loading = false;

  @action setActiveTab = (activeIndex: number) => {
    this.activeTab = activeIndex;
  };

  @action loadRestaurant = async (id: string) => {
    this.loadingInitial = true;
    try {
      let restaurant = await agent.OwnerRestaurants.details(id);
      runInAction("getting restaurant", () => {
        this.restaurant = restaurant;
        this.loadingInitial = false;
      });
      return restaurant;
    } catch (error) {
      runInAction("get restaurant error", () => {
        this.loadingInitial = false;
      });
      console.log(error);
    }
  };

  @action editRestaurant = async (restaurant: IRestaurant) => {
    this.submitting = true;
    try {
      console.log(restaurant);
      await agent.OwnerRestaurants.update(restaurant);
      runInAction("update restaurant", () => {
        //this.restaurants.push(restaurant);
        this.submitting = false;
      });
      history.push("/owner");
    } catch (error) {
      runInAction("update restaurant error", () => {
        this.submitting = false;
      });
      //toast.error("Problem submitting data");
      //console.log(error.response);
    }
  };

  @action uploadPhoto = async (restaurantId: string, file: Blob) => {
    this.uploadingPhoto = true;
    try {
      const photo = await agent.OwnerRestaurants.uploadPhoto(restaurantId, file);
      runInAction(() => {
        if (this.restaurant) {
          this.restaurant.photos.push(photo);
        }
        this.uploadingPhoto = false;
      });
    } catch (error) {
      toast.error("Problem uploading photo");
      runInAction(() => {
        this.uploadingPhoto = false;
      });
    }
  };

  @action setMainPhoto = async (restaurantId: string ,photo: IPhoto) => {
    this.loading = true;
    try {
      await agent.OwnerRestaurants.setMainPhoto(restaurantId, photo.id);
      runInAction(() => {
        this.restaurant!.photos.find((a) => a.isMain)!.isMain = false;
        this.restaurant!.photos.find((a) => a.id === photo.id)!.isMain = true;
        this.restaurant!.image = photo.url;
        this.loading = false;
      });
    } catch (error) {
      toast.error("Problem setting photo as main");
      runInAction(() => {
        this.loading = false;
      });
    }
  };

  @action deletePhoto = async (restaurantId: string, photo: IPhoto) => {
    this.loading = true;
    try {
      await agent.OwnerRestaurants.deletePhoto(restaurantId ,photo.id);
      runInAction(() => {
        this.restaurant!.photos = this.restaurant!.photos.filter(
          (a) => a.id !== photo.id
        );
        this.loading = false;
      });
    } catch (error) {
      toast.error("Problem deleting the photo");
      runInAction(() => {
        this.loading = false;
      });
    }
  };
}
