import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { SplitwiseSummaryModel } from '../core/models/splitwise-summary.model';

@Injectable({
  providedIn: 'root'
})
export class SplitwiseService {
  // Use the API route defined in your Controller [Route("api/[controller]")]
  // Likely: http://your-ip/api/SplitwiseApi
  private baseUrl = `${environment.apiUrl}/api/Splitwise`; 

  constructor(private http: HttpClient) {}

  // ================= DASHBOARD =================
 getDashboardSummary(): Observable<SplitwiseSummaryModel> {

    return this.http.get<SplitwiseSummaryModel>(`${this.baseUrl}/summary`);
  }
  getSummary(): Observable<any> {
    return this.http.get(`${this.baseUrl}/summary`);
  }

  // ================= MEMBERS =================

  getMembers(): Observable<any> {
    return this.http.get(`${this.baseUrl}/members`);
  }

  addMember(member: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/members`, member);
  }

  // ================= EXPENSES =================

  getExpenses(): Observable<any> {
    return this.http.get(`${this.baseUrl}/expenses`);
  }

  addExpense(formData: FormData): Observable<any> {
    return this.http.post(`${this.baseUrl}/expenses`, formData);
  }

  updateExpense(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.baseUrl}/expenses/${id}`, formData);
  }

  deleteExpense(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/expenses/${id}`);
  }

  getExpenseTypes(): Observable<any> {
    return this.http.get(`${this.baseUrl}/expense-types`);
  }

  // ================= PAYMENTS =================

 
pay(data: {
  fromMemberId: number;
  toMemberId: number;
  amount: number;
}): Observable<any> {
  return this.http.post(`${this.baseUrl}/pay`, data);
}
  // ================= EXPORT =================

  exportExcel() {
    return this.http.get(`${this.baseUrl}/export/excel`, { responseType: 'blob' });
  }

  exportPdf() {
    return this.http.get(`${this.baseUrl}/export/pdf`, { responseType: 'blob' });
  }
}