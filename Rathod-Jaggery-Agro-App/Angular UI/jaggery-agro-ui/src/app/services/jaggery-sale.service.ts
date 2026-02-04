import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class JaggerySaleService {
  private api = `${environment.apiUrl}/api/jaggery-sales`;
private apiUrl = `${environment.apiUrl}/api/jaggery-sale-share`;
private baseUrl = `${environment.apiUrl}/api`;
  constructor(private http: HttpClient) {}

  /**
   * 🔹 GET Sales with Optional Filters
   * Accepts individual arguments to match component calls
   */
  getDropdownData(): Observable<any> {
    return this.http.get<any>(`${this.api}/dropdowns`);
  } 

  /**
   * 🔹 CREATE (POST)
   * Handles multipart/form-data for image uploads
   */
  create(formData: FormData): Observable<any> {
    return this.http.post(this.api, formData);
  }

  /**
   * 🔹 UPDATE (PUT)
   * Appends ID to the URL for the specific record
   */
  update(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.api}/${id}`, formData);
  }

  /**
   * 🔹 DELETE
   */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.api}/${id}`);
  }

  /**
   * 🔹 GET Dealer Statement & Balance
   * Specifically for the Balance Dashboard
   */
  getDealerBalance(dealerId: number): Observable<any> {
    return this.http.get<any>(`${this.api}/dealer/${dealerId}/balance`);
  }
  // 🔹 Get Dashboard Data (Index)
  getDashboardData(): Observable<any> {
    return this.http.get(`${this.apiUrl}/dashboard`);
  }
// 🔹 Get Dealers
getDealers(): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/dealers`);
}
getSalesNew(dealerId?: number, from?: string, to?: string) {

  let params: any = {};

  if (dealerId) params.dealerId = dealerId;
  if (from) params.from = from;
  if (to) params.to = to;

  return this.http.get<any[]>(`${this.baseUrl}/sales`, { params });
}


// 🔹 Get Members
getMembers(): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/members`);
}
  // 🔹 Get Dropdown Data for Create
  getSalesDropdown(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/sales-dropdown`);
  }

  // 🔹 Create New Share
  createShare(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/Create`, data);
  }

  // 🔹 Record Payment with optional Image Upload
  recordPayment(formData: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/RecordPayment`, formData);
  }

  // 🔹 Get Members linked to a specific sale
  getMembersBySale(saleId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/GetMembersBySale`, {
      params: new HttpParams().set('saleId', saleId.toString())
    });
  }
// 🔹 Update Share
updateShare(id: number, data: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/Update/${id}`, data);
}

  // 🔹 Get Specific Sale Details
  getSaleDetails(saleId: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/GetSaleDetails/${saleId}`);
  }

  // 🔹 Get Member Summary
  getMemberSummary(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/MemberSummary`);
  }

  // 🔹 Delete Share
  deleteShare(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Delete/${id}`);
  }
  // 1. Get Dealer Advances (Ganesh's 50k payment etc.)
  getDealerAdvances(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/dealer-advances`);
  }

  // 2. Get Splitwise Expenses (Labor, Diesel, etc.)
  getExpenses(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/Splitwise/expenses`);
  }

  // 3. Get Sale Shares (Profit distribution records)
  getSaleShares(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/jaggery-sale-share/dashboard`);
  }

  // Keep your existing methods like getSales, getDealers, etc.
  getSales(dealerId?: number): Observable<any> {
    let url = `${this.baseUrl}/JaggerySalesReport/data`;
    if (dealerId) {
      url += `?dealerId=${dealerId}`;
    }
    return this.http.get<any>(url); // returns Observable of full response
  }

  recordShare(data: any) {
  return this.http.post(`${this.baseUrl}/sale-shares`, data);
}
 getJaggerySalesReport(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/JaggerySalesReport/data`);
  }
}