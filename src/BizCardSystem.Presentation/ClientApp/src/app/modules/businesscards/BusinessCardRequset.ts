import { Gender } from "../enums/Gender";
import { Address } from "./Address";

export interface BusinessCardRequset{
    name: string;
    gender: Gender;
    dateofBirth: Date;
    email: string;
    phone: string;
    photo: string;
    address: Address;
}