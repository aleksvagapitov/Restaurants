export interface IReservation {
    id: string,
    restaurantId: string,
    firstName: string,
    lastName: string,
    phoneNumber: string,
    email: string,
    dateAndTime: Date,
    people: number,
    occasion: string,
    specialRequest: string,
    status: IStatus
}

export enum IStatus {
    Pending,
    Approved,
    Cancelled
  }
