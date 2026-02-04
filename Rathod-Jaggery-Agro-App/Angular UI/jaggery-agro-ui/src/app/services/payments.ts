import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import{ environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PaymentService {

   private api = `${environment.apiUrl}/api/payments`; 
  
  constructor(private http: HttpClient) {}

  getAll(from?: string, to?: string) {
    return this.http.get<any[]>(this.api, {
      params: { from: from || '', to: to || '' }
    });
  }

  generate(data: any) {
    return this.http.post<any[]>(`${this.api}/generate`, data);
  }

  downloadSlip(id: number) {
    return this.http.get(`${this.api}/${id}/slip`, {
      responseType: 'blob'
    });
  }
}
