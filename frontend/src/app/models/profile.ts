import { IStatus } from "./reservation";

export interface IProfile {
    displayName: string,
    username: string,
    biography: string,
    image: string,
    following: boolean,
    followersCount: number,
    followingCount: number,
    photos: IPhoto[]
}

export interface IPhoto {
    id: string,
    url: string,
    isMain: boolean
}

export interface IUserReservation {
    id: string,
    restaurantName: string,
    restaurantPhoto: string,
    dateTime: Date,
    people: number,
    occasion: string,
    specialRequest: string,
    status: IStatus
}