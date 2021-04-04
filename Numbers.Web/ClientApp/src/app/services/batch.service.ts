import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BatchResponse } from '../models/batch-response.model';
import { BatchDetail } from '../models/batch-detail.model';

@Injectable({
  providedIn: 'root'
})
export class BatchService {

  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string) {
  }

  startBatch(request): Observable<string> {
    const params = new HttpParams({ fromObject: request});

    return this.http.get<string>(this.baseUrl + 'batch/startbatch',{ params });
  }

  pollBatchData(batchId: number): Observable<BatchResponse> {
    return this.http.get<BatchResponse>(this.baseUrl + 'batch/poll/' + batchId)
  }

  clear(): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'batch/clear')
  }

  lastBatch(): Observable<any> {
    return this.http.get<BatchDetail[]>(this.baseUrl + 'batch/lastbatch')
  }
}
