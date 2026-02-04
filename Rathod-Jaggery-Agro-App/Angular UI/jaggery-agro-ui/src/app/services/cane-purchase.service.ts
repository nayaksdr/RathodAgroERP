import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { CanePurchase } from '../core/models/CanePurchase';

/* =======================
   MODELS
======================= */
export interface CanePurchaseSummary {
  farmerId: number;
  farmerName: string;
  totalTons: number;
  totalAmount: number;
  totalPaid: number;
  caneWeightImagePath?: string;
}

@Injectable({ providedIn: 'root' })
export class CanePurchaseService {
  // Fixed: Ensure the URL points correctly to your environment variable
  private api = `${environment.apiUrl}/api/cane-purchases`;
  private farmerApi = `${environment.apiUrl}/api/farmers`; // Path for the dropdown

  constructor(private http: HttpClient) {}

  /* =======================
      FARMER DROPDOWN DATA
  ======================= */
  // Added this to populate your dropdown in the purchase form
  getFarmersList(): Observable<any[]> {
    return this.http.get<any[]>(this.farmerApi);
  }

  /* =======================
      DASHBOARD / SUMMARY
  ======================= */
  getSummary(): Observable<CanePurchaseSummary[]> {
    return this.http.get<CanePurchaseSummary[]>(`${this.api}/summary`);
  }

  /* =======================
      LIST & FILTER (Server-side)
  ======================= */
  getAll(filter?: {
    farmerId?: number;
    fromDate?: string;
    toDate?: string;
  }): Observable<CanePurchase[]> {
    let params = new HttpParams();
    
    // Optimized: Only append if the value exists to avoid sending "undefined" as strings
    if (filter) {
      if (filter.farmerId) params = params.set('farmerId', filter.farmerId.toString());
      if (filter.fromDate) params = params.set('fromDate', filter.fromDate);
      if (filter.toDate) params = params.set('toDate', filter.toDate);
    }

    return this.http.get<CanePurchase[]>(this.api, { params });
  }

  /* =======================
      CRUD OPERATIONS
  ======================= */
  getById(id: number): Observable<CanePurchase> {
    return this.http.get<CanePurchase>(`${this.api}/${id}`);
  }

  create(payload: Partial<CanePurchase>): Observable<any> {
    return this.http.post(this.api, payload);
  }

  // Changed: Use Partial here too in case you don't send the full object on update
  update(id: number, payload: Partial<CanePurchase>): Observable<any> {
    return this.http.put(`${this.api}/${id}`, payload);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.api}/${id}`);
  }

  /* =======================
      IMAGE UPLOAD
  ======================= */
  uploadCaneWeightImage(id: number, file: File): Observable<any> {
    const formData = new FormData();
    formData.append('image', file);
    // Note: Most backends expect a POST or PATCH for file uploads
    return this.http.post(`${this.api}/${id}/upload-image`, formData);
  }
}