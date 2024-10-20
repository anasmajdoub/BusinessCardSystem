import { Direction } from "../enums/Direction";
import { Gender } from "../enums/Gender";

export class BusinessCardFilter 
{
    name?: string;             
    gender?: Gender;           
    dateOfBirth?: Date;        
    email?: string;
    phone?: string;
    pageIndex: number=1;          
    pageSize: number=12;
    sortColumn?: string;        
    sortDirection: Direction=Direction.Desc;   
  }