import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CanePaymentSummary } from '../core/models/CanePaymentSummary';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class CanePaymentService {
  private api = `${environment.apiUrl}/api/cane-payments`;    
 private farmerApi = `${environment.apiUrl}/api/farmers`;    

  constructor(private http: HttpClient) {}

  getSummary() {
    return this.http.get<CanePaymentSummary[]>(`${this.api}/summary`);
  }

  downloadSlip(farmerId: number) {
    return this.http.get(`${this.api}/slip/${farmerId}`, {
      responseType: 'blob'
    });
  }
// ADD THIS METHOD
  getFarmersList(): Observable<any[]> {
    return this.http.get<any[]>(this.farmerApi);
  }
  exportExcel() {
    return this.http.get(`${this.api}/export/excel`, {
      responseType: 'blob'
    });
  }
}
