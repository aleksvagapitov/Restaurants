export interface IRestaurantsEnvelope {
    restaurants: IRestaurant[];
    restaurantCount: number;
}

export interface IRestaurant{
    id: string,
    name: string,
    city: string,
    address: string,
    postalCode: string,
    phone: string,
    latitude: number,
    longitude: number,
    photos: IPhoto[],
    image: string,
    workHours: IWorkHours[],
    description: string,
    categories: IRestaurantCategory[],
    reviews: IReview[]
}

export interface IPhoto {
    id: string,
    url: string,
    isMain: boolean
}

export interface IWorkHours {
    dayOfWeek: IDay,
    startTime: string,
    endTime: string,
}

export enum IDay {
    Sunday,
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday
  }

export interface IRestaurantCategory {
    id: string,
    category: string
}

export interface ICategory {
    text: string,
    value: string
}

export interface ISearchValues {
    dateTime: Date,
    people: string,
    term: string
}

export class SearchValues {
    dateTime: Date;
    people: string;
    term: string;

    constructor(dateTime: string, people: string, term: string)
    {
        this.dateTime = dateTime ? new Date(dateTime) : new Date()
        this.people = people ? people : "1"
        this.term = term ? term : '';
    }
}

export interface IBookValues {
    dateTime: Date,
    people: number
}

export class CategoryFormValues {
    text: string = '';
    value: string = '';

    constructor(text: string, value: string) {
        Object.assign(this.text, text);
        Object.assign(this.value, value);
    }
}

export class RestaurantFormValues {
    id?: string = undefined;
    name: string = '';
    city: string = '';
    address: string = '';
    postalCode: string = '';
    phone: string = '';
    description: string = '';
    latitude?: number = undefined;
    longitude?: number = undefined;

    constructor(init?: IRestaurant) {
        Object.assign(this, init);
    }
}

export interface IReview {
    id: string;
    body: string;
    createdAt: string;
    stars: number;
    displayName: string;
}