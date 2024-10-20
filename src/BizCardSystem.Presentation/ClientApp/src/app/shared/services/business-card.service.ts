import { Injectable } from '@angular/core';
import { BaseService } from '../Core/BaseService';
import { Observable } from 'rxjs';
 
import { indexResult, ResultOf } from '../Core/Result';
import { BusinessCard } from '../../modules/businesscards/BusinessCard';
import { BusinessCardRequset } from '../../modules/businesscards/BusinessCardRequset';

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService extends BaseService {
  protected override baseUrl: string="api/BusinessCards";
  constructor() {
    super();
   }
   GetAll(filter:any): Observable<indexResult<BusinessCard>> {
    return this.usingApiUrl(this.baseUrl).getAll('GetAll', filter);
  }
 Create(BusinessCardRequset:any):Observable<ResultOf<boolean>>{
   return this.usingApiUrl(this.baseUrl).create('Create', BusinessCardRequset);
 }
 Delete(id:number) : Observable<ResultOf<boolean>> {
   return this.usingApiUrl(this.baseUrl).delete('DeleteById', id);
 }
 ExportToCSV(id:number) : Observable<Blob> {
   let url = `${this.usingApiUrl(this.baseUrl).ApiUrl}ExportToCsv/?id=${id}`;
   return this.usingApiUrl(this.baseUrl).getBlob(url, {responseType: 'blob' as 'blob'});
 }
 
 ExportToXML(id:number) : Observable<Blob> {
   let url = `${this.usingApiUrl(this.baseUrl).ApiUrl}ExportToXml/?id=${id}`;
   return this.usingApiUrl(this.baseUrl).getBlob(url, {responseType: 'blob' as 'blob'});
 }
 uploadFile(file:File) : Observable<ResultOf<BusinessCardRequset>> {

   return this.usingApiUrl(this.baseUrl).uploadFile("CreateBusinessCardByFile",file);
 }
}
