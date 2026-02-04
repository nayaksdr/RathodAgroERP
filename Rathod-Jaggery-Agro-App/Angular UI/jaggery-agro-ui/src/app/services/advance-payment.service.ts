import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdvancePayment } from '../core/models/AdvancePayment';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AdvancePaymentService {
  // FIX: Added /api/ before the endpoint name to match your Backend [Route]
  private api = `${environment.apiUrl}/api/advance-payments`;

  constructor(private http: HttpClient) {}

  // Added Observable return types to all methods for consistency
  getAll(): Observable<AdvancePayment[]> {
    return this.http.get<AdvancePayment[]>(this.api);
  }

  getFormData(): Observable<any[]> {
    return this.http.get<any[]>(`${this.api}/form-data`);
  }

  get(id: number): Observable<AdvancePayment> {
    return this.http.get<AdvancePayment>(`${this.api}/${id}`);
  }

  create(data: AdvancePayment): Observable<any> {
    return this.http.post(this.api, data);
  }

  update(id: number, data: AdvancePayment): Observable<any> {
    return this.http.put(`${this.api}/${id}`, data);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.api}/${id}`);
  }
}