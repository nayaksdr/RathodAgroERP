import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { LaborPayment } from '../core/models/LaborPayment'; // ✅ Use the model here

// Define an interface for the API response to match the actual structure
export interface LaborPaymentResponse {
  data: LaborPayment[];  // The array of labor payments
  summary: {  // Adjust this based on your summary structure (e.g., totals, counts)
    totalPayments?: number;  // Example fields; replace with actual API fields
    totalAmount?: number;
    // Add more fields as needed based on your API's summary object
  };
}

@Injectable({
  providedIn: 'root'
})
export class LaborPaymentService {

  private api = `${environment.apiUrl}/api/labor-payments`;

  constructor(private http: HttpClient) { }

  /**
   * Get labor payments with optional filters
   * @param params - { laborId, from, to }
   * @returns Observable of the full API response object
   */
  getPayments(params: any = {}): Observable<LaborPaymentResponse> {  // ✅ Updated return type
    let httpParams = new HttpParams();
    
    Object.keys(params).forEach(key => {
      if (params[key] !== null && params[key] !== undefined && params[key] !== '') {  // ✅ Fixed: Added undefined check
        httpParams = httpParams.set(key, params[key]);
      }
    });

    console.log('Calling API:', this.api, 'Params:', httpParams.toString());

    return this.http.get<LaborPaymentResponse>(this.api, { params: httpParams });  // ✅ Updated type
  }

  /**
   * Generate PDF payslip
   */
  generateSlip(data: any): Observable<Blob> {
    return this.http.post(`${this.api}/generate-slip`, data, {
      responseType: 'blob'
    });
  }
}