import { inject } from "@angular/core";
import { ApiClient } from "../Api/api-client.service";

export abstract class BaseService {
    protected httpService: ApiClient = inject(ApiClient);
    protected abstract baseUrl: string;
  
    protected usingApiUrl(url: string): ApiClient {
      this.httpService.ApiUrl =  url;
      return this.httpService;
    }
  }
  