import { Direction } from "src/app/Shared/Core/Enums/Direction";
import { Gender } from "src/app/Shared/Core/Enums/Gender";
export interface BusinessCard{
    readonly id: number; 
    name: string;
    gender: Gender;
    dateOfBirth: string;
    email: string;
    phone: string;
    photo: string;
    address: string;
}
export class BusinessCardFilter{

    name?: string;
    gender?: Gender;
    dateOfBirth?: string;
    email?: string;
    phone?: string;
    pageIndex: number;
    pageSize: number;
    sortColumn?: string;
    sortDirection?: Direction;
 
constructor() {
    this.pageIndex=1;
    this.pageSize=10;
}
}
export interface Address{
    street: string;
    city: string;
    state: string;
    zipCode: string;
    country: string;
}
export interface BusinessCardRequset{
    name: string;
    gender: Gender;
    dateOfBirth: Date;
    email: string;
    phone: string;
    photo: string;
    address: Address;
}
