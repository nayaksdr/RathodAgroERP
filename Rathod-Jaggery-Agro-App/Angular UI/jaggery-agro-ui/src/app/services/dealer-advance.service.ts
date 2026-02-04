import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface DealerAdvance {
  id: number;
  dealerId: number;
  dealerName: string;   // Now coming from backend
  paidById: number;
  paidByName: string;   // Now coming from backend
  paymentDate: string;
  amount: number;
  paymentMode: string;
  proofImage?: string;
  remarks?: string;
}
@Injectable({ providedIn: 'root' })
export class DealerAdvanceService {
  private api = `${environment.apiUrl}/api/dealer-advances`;

  constructor(private http: HttpClient) {}

  // Added Observable type and proper parameter handling
  get(filter: any): Observable<any[]> {
    let params = new HttpParams();
    if (filter.dealerId) params = params.set('dealerId', filter.dealerId);
    if (filter.from) params = params.set('from', filter.from);
    if (filter.to) params = params.set('to', filter.to);

    return this.http.get<any[]>(this.api, { params });
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.api}/${id}`);
  }

  create(data: FormData): Observable<any> {
    return this.http.post(this.api, data);
  }

  update(id: number, data: FormData): Observable<any> {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.api}/${id}`);
  }
}