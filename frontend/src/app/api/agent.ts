import axios, { AxiosResponse } from "axios";
import { history } from "../..";
import { toast } from "react-toastify";
import { IUserFormValues, IUser } from "../models/user";
import { IProfile, IPhoto } from "../models/profile";
import { IRestaurant, ICategory, IRestaurantsEnvelope } from "../models/restaurant";
import { IReservation } from "../models/reservation";

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

axios.interceptors.request.use(
  (config) => {
    const token = window.localStorage.getItem("jwt");
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

axios.interceptors.response.use(undefined, (error) => {
  if (error.message === "Network Error" && !error.response) {
    toast.error("Network error - make sure API is running!");
  }
  const { status, data, config, headers } = error.response;

  if (status === 404) {
    // history.push("/notfound");
  }
  if (
    status === 401 &&
    headers["www-authenticate"].includes(
      'Bearer error="invalid_token", error_description="The token expired at'
    )
  ) {
    window.localStorage.removeItem("jwt");
    history.push("/");
    toast.info("Your session has expired, please log in again");
  }
  if (
    status === 400 &&
    config.method === "get" &&
    data.errors.hasOwnProperty("id")
  ) {
    history.push("/notfound");
  }
  if (status === 500) {
    toast.error("Server error - check the terminal for more info!");
  }
  console.log(error.response);
  throw error.response;
});

const responseBody = (response: AxiosResponse) => response.data;

//#region for testing only
const sleep = (ms: number) => (response: AxiosResponse) =>
    new Promise<AxiosResponse>(resolve => setTimeout(() => resolve(response), ms));
//#endregion

const requests = {
  get: (url: string) => axios.get(url).then(responseBody),
  post: (url: string, body: {}) =>
    axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) =>
    axios.put(url, body).then(responseBody),
  del: (url: string) => axios.delete(url).then(responseBody),
  postForm: (url: string, file: Blob) => {
    let formData = new FormData();
    formData.append("File", file);
    return axios
      .post(url, formData, {
        headers: { "Content-type": "multipart/form-data" },
      })
      .then(responseBody);
  },
};

const User = {
  current: (): Promise<IUser> => requests.get("/user"),
  login: (user: IUserFormValues): Promise<IUser> =>
    requests.post("/user/login", user),
  register: (user: IUserFormValues): Promise<IUser> =>
    requests.post("/user/register", user),
};

const Profiles = {
  get: (username: string): Promise<IProfile> =>
    requests.get(`/profiles/${username}`),
  updateProfile: (profile: Partial<IProfile>) =>
    requests.put(`/profiles`, profile),
  uploadPhoto: (photo: Blob): Promise<IPhoto> =>
    requests.postForm(`/photos`, photo),
  setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
  deletePhoto: (id: string) => requests.del(`/photos/${id}`),
  follow: (username: string) =>
    requests.post(`/profiles/${username}/follow`, {}),
  unfollow: (username: string) => requests.del(`/profiles/${username}/follow`),
  listFollowings: (username: string, predicate: string) =>
    requests.get(`/profiles/${username}/follow?predicate=${predicate}`),
  listReservations: (predicate: string) =>
    requests.get(`/profile/reservations?predicate=${predicate}`),
};

const Restaurants = {
  list: (dateTime: string, people: string, term: string): Promise<IRestaurant[]> =>
    requests.get(`/restaurants?dateTime=${dateTime}&people=${people}&term=${term}`),
  filteredList: (params: URLSearchParams): Promise<IRestaurantsEnvelope> =>
    axios
      .get("/restaurants/filtered", { params: params })
      .then(sleep(1000)).then(responseBody),
  details: (id: string) => requests.get(`/restaurants/${id}`),
  listCategories: (): Promise<ICategory[]> => requests.get(`/restaurants/categories`),
  listReviews: (id: string, offset: number) => requests.get(`/restaurants/${id}/reviews/${offset}`),

};

const Reservations = {
  reserveAsGuest: (reservation: Partial<IReservation>) => requests.post("/reservations/reserve-as-guest", reservation),
  reserveAsUser: (reservation: Partial<IReservation>) => requests.post("/reservations/reserve-as-user", reservation)
};

const OwnerRestaurants = {
  list: (): Promise<IRestaurant[]> =>
    requests.get(`/ownerRestaurants`),
  details: (id: string) => requests.get(`/ownerRestaurants/${id}`),
  create: (restaurant: IRestaurant) => 
    requests.post("/ownerRestaurants", restaurant),
  update: (restaurant: IRestaurant) =>
  requests.put(`/ownerRestaurants/${restaurant.id}`, restaurant),
  uploadPhoto: (restaurantId: string, photo: Blob): Promise<IPhoto> =>
    requests.postForm(`/ownerRestaurantsPhotos/${restaurantId}`, photo),
  setMainPhoto: (restaurantId: string, photoId: string) => 
    requests.post(`/ownerRestaurantsPhotos/${restaurantId}/${photoId}/setMain`, {}),
  deletePhoto: (restaurantId: string, photoId: string) => 
    requests.del(`/ownerRestaurantsPhotos/${restaurantId}/${photoId}`),
}

const OwnerReservations = {
  list: (restaurantId: string): Promise<IReservation[]> =>
    requests.get(`/OwnerReservations/${restaurantId}`),
  update: (reservation: IReservation, restaurantId: string) =>
    requests.put(`/OwnerReservations/${restaurantId}/${reservation.id}`, reservation),
}

const AdminOwners = {
  create: (owner: IUserFormValues) =>
    requests.post("/admin/owners/create", owner)
}

export default {
  User,
  Profiles,
  Restaurants,
  Reservations,
  OwnerRestaurants,
  OwnerReservations,
  AdminOwners
};
