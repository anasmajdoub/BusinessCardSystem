import {Injectable, inject} from '@angular/core';
import {HttpClient, HttpContext, HttpErrorResponse, HttpHeaders, HttpParams} from '@angular/common/http';
import {catchError, Observable, throwError} from 'rxjs';
import { environment } from '../../../environments/environment';
 


 
@Injectable({providedIn: 'any'})
export class ApiClient {
   
  constructor(private http: HttpClient) {

  }

  private baseUrl =environment.apiUrl;

  public get ApiUrl() {
    return this.baseUrl;
  }

  public set ApiUrl(apiPrefix: string) {
 
    if (apiPrefix)
      this.baseUrl =environment.apiUrl + apiPrefix + '/';

  }

  private httpOptions = {
    headers: new HttpHeaders({'Content-Type': 'application/json'})
  };

  OnHttpError?: (error: HttpErrorResponse) => void

  DefaultUserFriendlyError(error: HttpErrorResponse): void 
  {  
     
    if(error?.error?.message)
     alert(`${this.userFriendlyErrorMessage(error.error.message)}`);
  }

  getResponseObject(errorMessage: string)
  {
    const responseStartIndex = errorMessage.indexOf('response = ') + 'response = '.length;
    const responseEndIndex = errorMessage.lastIndexOf('}');
    const responseString = errorMessage.substring(responseStartIndex, responseEndIndex + 1);
    return JSON.parse(responseString);
  }
  userFriendlyExceptionMessage(errorMessage: string) {
    return errorMessage;
  }
  userFriendlyErrorMessage(errorMessage: string) 
  {
    var response= this.getResponseObject(errorMessage);
    return   response.message;
  }

  private CatchErr<T>(caller: string) {
    return (error: any): Observable<any> => {
      this.OnHttpError ??= this.DefaultUserFriendlyError
      this.OnHttpError(error);
      return throwError(error.error);
    };
  }

  getAll<T>(path: string, queryParams?: any, catchErr?: (error: any) => Observable<T>): Observable<T> {
    let params = new HttpParams();
     if (queryParams) {
      for (const key in queryParams) {
        const isPropExists = queryParams.hasOwnProperty(key);
        const isPropNotNull = isPropExists && queryParams[key] !== null && queryParams[key] !== undefined && queryParams[key] !== '';
        if (isPropNotNull) {
          params = params.append(key, queryParams[key]);
        }
      }
    }
     
    return this.http.get<T>(this.baseUrl + path, {params}).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.get')));
  }

  getById<T>(path: string, id: number | string, catchErr?: (error: any) => Observable<T>): Observable<T> {
    return this.http.get<T>(`${this.baseUrl + path}?id=${id}`).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.getById')));
  }

  getByUrl<T>(url: string, queryString?: string, catchErr?: (error: any) => Observable<T>): Observable<T> {
    if (queryString) url = `${url}?${queryString}`;
    return this.http.get<T>(`${this.baseUrl + url}`).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.getByUrl')));
  }

  create<T>(path: string, item: T, catchErr?: (error: any) => Observable<T>): Observable<T> {
    return this.http.post<T>(this.baseUrl + path, item, this.httpOptions).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.create')));
  }

  update<T>(path: string, item: T, catchErr?: (error: any) => Observable<T>): Observable<T> {
    return this.http.post<T>(this.baseUrl + path, item, this.httpOptions).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.update')));
  }

  delete<T>(path: string, id: number | string, catchErr?: (error: any) => Observable<T>): Observable<T> {
      return this.http.delete<T>(`${this.baseUrl + path}?id=${id}`).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.delete')));
  }

  deleteByUrl<T>(url: string, queryString?: string, catchErr?: (error: any) => Observable<T>): Observable<T> {
    if (queryString) url = `${url}?${queryString}`;
    return this.http.delete<T>(`${this.baseUrl + url}`).pipe(catchError(catchErr || this.CatchErr<T>('ApiClient.deleteByUrl')));
  }

  getBlob(url: string, options: {
    headers?: HttpHeaders | {
      [header: string]: string | string[];
    };
    context?: HttpContext;
    observe?: 'body';
    params?: HttpParams | {
      [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>;
    };
    reportProgress?: boolean;
    responseType: 'blob';
    withCredentials?: boolean;
  }): Observable<Blob> {
    return this.http.get(url, options);
  }

  uploadFile(path: string, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file); // Attach the file to FormData
    return this.http.post<any>(`${this.baseUrl}${path}`, formData);
  }

}
