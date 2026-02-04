import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class JaggeryReportService {
  private apiUrl = `${environment.apiUrl}/api/JaggerySalesReport`;

  constructor(private http: HttpClient) {}

  // Get JSON data for the table
  getReportData(dealerId?: number, from?: string, to?: string): Observable<any> {
    let params = new HttpParams();
    if (dealerId) params = params.set('dealerId', dealerId.toString());
    if (from) params = params.set('from', from);
    if (to) params = params.set('to', to);
    
    return this.http.get(`${this.apiUrl}/data`, { params });
  }

  // Generate Download URLs
  getExportUrl(type: 'excel' | 'pdf', dealerId?: number, from?: string, to?: string): string {
    const baseUrl = `${this.apiUrl}/export/${type}`;
    const params = new URLSearchParams();
    if (dealerId) params.append('dealerId', dealerId.toString());
    if (from) params.append('from', from);
    if (to) params.append('to', to);
    
    return `${baseUrl}?${params.toString()}`;
  }
  
  // ✅ GET DEALERS METHOD (This is what you asked)
  getDealers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/dealers`);
  }
}