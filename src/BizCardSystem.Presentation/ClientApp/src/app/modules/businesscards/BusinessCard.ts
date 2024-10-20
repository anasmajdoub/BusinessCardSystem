import { Gender } from "../enums/Gender";
import { Address } from "./Address";

export interface BusinessCard {
    readonly id: number; 
    name: string;
    gender: Gender;
    dateofBirth: string;
    email: string;
    phone: string;
    photo: string;
    address: string;
  }