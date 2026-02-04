import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CaneAdvance } from '../core/models/CaneAdvance';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CaneAdvanceService {
  private api = `${environment.apiUrl}/api/cane-advances`;

  constructor(private http: HttpClient) {}

  /** Get all advances with mapped details */
  getAll(): Observable<CaneAdvance[]> {
    return this.http.get<CaneAdvance[]>(this.api);
  }

  /** Get dropdown lists for Farmers and Members */
  getFormData(): Observable<{ farmers: any[]; members: any[] }> {
    return this.http.get<{ farmers: any[]; members: any[] }>(`${this.api}/form-data`);
  }

  /** Get a single record by ID */
  get(id: number): Observable<CaneAdvance> {
    return this.http.get<CaneAdvance>(`${this.api}/${id}`);
  }

  /** * Create new advance 
   * Note: Browser automatically sets 'Content-Type': 'multipart/form-data' 
   */
  save(formData: FormData): Observable<any> {
    return this.http.post(this.api, formData);
  }

  /** Update existing advance with file support */
  update(id: number, formData: FormData): Observable<any> {
    return this.http.put(`${this.api}/${id}`, formData);
  }

  /** Delete record */
  delete(id: number): Observable<any> {
    return this.http.delete(`${this.api}/${id}`);
  }

  /** * Optional: Fetch the proof document URL or Blob 
   * Useful if you want to preview the document in the table
   */
  getProofDocument(id: number): Observable<Blob> {
    return this.http.get(`${this.api}/${id}/proof`, { responseType: 'blob' });
  }
  /** Get the full URL for the proof document */
getProofUrl(fileName: string): string {
  return `${environment.apiUrl}/uploads/proofs/${fileName}`;
}
}