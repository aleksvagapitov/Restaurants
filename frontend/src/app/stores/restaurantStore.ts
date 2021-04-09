import { RootStore } from "./rootStore";
import { observable, action, runInAction, computed } from "mobx";
import { persist } from "mobx-persist";
import { IRestaurant, ICategory, IReview } from "../models/restaurant";
import agent from "../api/agent";

const LIMIT = 25;

export default class RestaurantStore {
  rootStore: RootStore;
  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @observable restaurants: IRestaurant[] = [];
  @observable reviews: IReview[] = [];
  @observable categories: ICategory[] = [];
  @observable frontPageRestaraunts = new Map<string, IRestaurant[]>();
  @observable restaurant: IRestaurant | null = null;
  @observable loadingRestaurants = false;
  @observable loadingCategories = false;
  //#region New Filters Observables
  @observable restaurantCount = 0;
  @observable reviewsCount = 0;
  @observable page = 0;
  @persist("map") @observable searchParams = new Map();
  //#endregion
  @observable loadingInitial = false;
  @observable loadingReviews = false;
  @observable submitting = false;

  @computed get totalPages() {
    return Math.ceil(this.restaurantCount/ LIMIT);
  }

  @computed get totalReviewPages() {
    return Math.ceil(this.reviewsCount/ LIMIT);
  }

  @action setPage = (page: number) => {
    this.page = page
  }

  @computed get searchParamsValues() {
    return Array.from(this.searchParams.values());
  }

  @action setPredicate = (predicate: string, value: string | Date | number | null | boolean | undefined) => { 
    this.page = 0;
    this.searchParams.set(predicate, value);
  }

  @action setPredicateFromArray = (predicate: string, value: string | number | boolean | (string | number | boolean)[] | undefined) => { 
    this.page = 0;
    this.searchParams.set(predicate, value);
  }

  @computed get filterParams() {
    const params = new URLSearchParams();
    params.append("limit", String(LIMIT));
    params.append("offset", `${this.page ? this.page * LIMIT : 0}`);
    this.searchParams.forEach((value, key) => {
        if (key === 'searchDate'){
          params.append(key, value.toISOString())
        } else if (key === 'categories'){
          console.log(value.toString());
          params.append(key, value.toString())
        }else {
          params.append(key, value);
        }
        console.log(key, value.toString());
    });
    return params;
  }

  @action loadRestaurantsFiltered = async () => {
    this.loadingInitial = true;
    try {
      const restaurantsEnvelope = await agent.Restaurants.filteredList(
        this.filterParams
      );
      const { restaurants, restaurantCount } = restaurantsEnvelope;
      runInAction("loading restaurants", () => {
        this.restaurants = restaurants;
        this.restaurantCount = restaurantCount;
        this.loadingInitial = false;
      });
    } catch (error) {
      runInAction("load restaurants error", () => {
        this.loadingInitial = false;
      });
    }
  };

  @action loadReviews = async (id: string, offset: number) => {
    this.loadingReviews = true;
    try {
      const reviewsEnvelope = await agent.Restaurants.listReviews(id, offset ? offset * LIMIT : 0);
      const { reviews, reviewsCount } = reviewsEnvelope;
      runInAction("loading reviews", () => {
        this.reviews = reviews;
        this.reviewsCount = reviewsCount;
        this.loadingReviews = false;
      });
    } catch (error) {
      runInAction("load restaurants error", () => {
        this.loadingReviews = false;
      });
    }
  };

  @action loadCategories = async () => {
    this.loadingCategories = true;
    try {
      const categories = await agent.Restaurants.listCategories();
      runInAction(() => {
        this.categories = categories;
        this.loadingCategories = false;
      });
    } catch (error) {
      runInAction(() => {
        this.loadingCategories = false;
      });
    }
  };

  @action loadRestaurant = async (id: string) => {
    this.loadingInitial = true;
    try {
      const restaurant = await agent.Restaurants.details(id);
      runInAction("getting restaurant", () => {
        this.restaurant = restaurant;
        this.loadingInitial = false;
      });
      return restaurant;
    } catch (error) {
      runInAction("get restaurant error", () => {
        this.loadingInitial = false;
      });
    }
  };

  @action listFrontPageRestaurants = async (
    dateTime: Date,
    people: string,
    term: string
  ) => {
    this.loadingRestaurants = true;
    try {
      const restaurants = await agent.Restaurants.list(
        dateTime.toISOString(),
        people,
        term
      );
      runInAction(() => {
        if (!this.frontPageRestaraunts.has(term)) {
          this.frontPageRestaraunts.set(term, restaurants);
          this.loadingRestaurants = false;
        }
      });
    } catch (error) {
      runInAction(() => {
        this.loadingRestaurants = false;
      });
    }
  };
}
