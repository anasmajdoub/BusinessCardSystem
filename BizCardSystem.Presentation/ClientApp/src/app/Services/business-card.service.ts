import { Injectable } from '@angular/core';
import { ApiClient } from '../Shared/Api/api-client.service';
import { BusinessCardFilter,BusinessCard } from '../business-card/modules/businessCard';
import { Observable, filter } from 'rxjs';
import { BaseService } from '../Shared/Core/BaseService';
import { indexResult, ResultOf } from '../Shared/Core/Result';

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService extends BaseService{
  protected override baseUrl: string="api/BusinessCards";

  constructor() 
  {
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
  uploadFile(file:File) : Observable<Blob> {
 
    return this.usingApiUrl(this.baseUrl).uploadFile("CreateBusinessCardByFile",file);
  }
}
