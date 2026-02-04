import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
// DTOs / Interfaces
export interface JaggerySaleShareStatus {
  id: number;
  jaggerySaleId: number;
  name: string;
  shareAmount: number;
  paidAmount: number;
  pendingAmount: number;
  status: 'Paid' | 'Pending';
}

export interface CreateJaggeryShareDto {
  JaggerySaleId: number;
  SplitMemberIds: number[];
  PayingMemberId: number;
  PaidAmount: number;
}

export interface RecordPaymentDto {
  SaleId: number;
  FromMemberId: number;
  ToMemberId: number;
  Amount: number;
  PaymentMode: string;
  ProofImage?: File;
}

export interface JaggerySale {
  id: number;
  totalAmount: number;
  saleDate: string;
}

export interface Member {
  id: number;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class JaggerySaleShareService {

  
    private api = `${environment.apiUrl}/api/jaggery-sale-share`;       

  constructor(private http: HttpClient) {}

  // Dashboard
  getDashboard(): Observable<JaggerySaleShareStatus[]> {
    return this.http.get<JaggerySaleShareStatus[]>(`${this.api}/dashboard`);
  }

  // Create Share
  createShare(dto: CreateJaggeryShareDto): Observable<string> {
    return this.http.post<string>(`${this.api}/create`, dto);
  }

  // Record Payment
  recordPayment(dto: RecordPaymentDto): Observable<string> {
    const formData = new FormData();
    formData.append('SaleId', dto.SaleId.toString());
    formData.append('FromMemberId', dto.FromMemberId.toString());
    formData.append('ToMemberId', dto.ToMemberId.toString());
    formData.append('Amount', dto.Amount.toString());
    formData.append('PaymentMode', dto.PaymentMode);
    if (dto.ProofImage) {
      formData.append('ProofImage', dto.ProofImage, dto.ProofImage.name);
    }

    return this.http.post<string>(`${this.api}/record-payment`, formData);
  }

  // Get All Sales
  getSales(): Observable<JaggerySale[]> {
    return this.http.get<JaggerySale[]>(`${this.api}/sales`);
  }

  // Get All Members
  getMembers(): Observable<Member[]> {
    return this.http.get<Member[]>(`${this.api}/members`);
  }
}
